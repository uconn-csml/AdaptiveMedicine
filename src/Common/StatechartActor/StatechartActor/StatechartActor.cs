using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AdaptiveMedicine.Common.Statechart.Attributes;
using AdaptiveMedicine.Common.Statechart.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace AdaptiveMedicine.Common.Actors {
   public abstract class StatechartActor: Actor, IStatechart {
      public const string CurrentStateLabel = "Statechart.CurrentState";
      public const string PastEventsLabel = "Statechart.PastEvents";
      private static readonly ConditionalWeakTable<Type, StatesMap> _StatesPerStatechart = new ConditionalWeakTable<Type, StatesMap>();

      private class StatesMap {
         public string Initial { get; }
         public IReadOnlyDictionary<string, IState> List { get; }

         public StatesMap(string initialState, IDictionary<string, IState> statesList) {
            Initial = initialState;
            List = new ReadOnlyDictionary<string, IState>(statesList);
         }
      }

      public StatechartActor(ActorService actorService, ActorId actorId)
         : base(actorService, actorId) {

         var thisType = this.GetType();
         if (!_StatesPerStatechart.TryGetValue(thisType, out StatesMap statesMap)) {
            string initialState = null;
            var statesList = new Dictionary<string, IState>();

            var potentialStates = new List<Type>();
            var currentType = thisType;
            while(currentType != null && currentType != typeof(StatechartActor)) {
               potentialStates.AddRange(currentType.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic));
               currentType = currentType.BaseType;
            }

            foreach (var potentialState in potentialStates) {
               if (potentialState.IsClass && potentialState.GetInterfaces().Contains(typeof(IState))) {

                  var stateAttributes = potentialState.GetCustomAttributes<StateAttribute>();
                  foreach (var stateAttribute in stateAttributes) {
                     if (!String.IsNullOrWhiteSpace(stateAttribute.Type)) {
                        statesList[stateAttribute.Type] = (IState)potentialState.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static).GetValue(null);
                     }
                  }

                  var initialStateAttributes = potentialState.GetCustomAttributes<InitialStateAttribute>();
                  if (initialStateAttributes.Count() == 1) {
                     initialState = initialStateAttributes.First().Type;
                     if (!String.IsNullOrWhiteSpace(initialState)) {
                        statesList[initialState] = (IState)potentialState.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static).GetValue(null);
                     }
                  }

               }
            }

            _StatesPerStatechart.GetValue(thisType, _ => new StatesMap(initialState, statesList));
         }
      }

      protected override async Task OnActivateAsync() {
         if (_StatesPerStatechart.TryGetValue(this.GetType(), out StatesMap statesMap)) {
            var currentState = await StateManager.TryGetStateAsync<string>(CurrentStateLabel);
            if (!currentState.HasValue || !statesMap.List.ContainsKey(currentState.Value)) {
               await StateManager.SetStateAsync<string>(CurrentStateLabel, statesMap.Initial);
            }

            var pastEvents = await StateManager.TryGetStateAsync<Dictionary<string, DateTime>>(PastEventsLabel);
            if (!pastEvents.HasValue) {
               await StateManager.SetStateAsync<Dictionary<string, DateTime>>(PastEventsLabel, new Dictionary<string, DateTime>());
            }
         } else {
            // throw
         }

         await base.OnActivateAsync();
         return;
      }

      public async Task DispatchEventAsync(IEvent anEvent) {
         if (_StatesPerStatechart.TryGetValue(this.GetType(), out StatesMap statesMap)) {
            var pastEvents = await StateManager.TryGetStateAsync<Dictionary<string, DateTime>>(PastEventsLabel);
            if (pastEvents.HasValue) {
               if (pastEvents.Value.TryGetValue(anEvent.Type.ToString(), out DateTime lastEventId) && anEvent.Id <= lastEventId) {
                  return; // We already processed this event or we haven't but we already processed a newer one.
               }
            } else {
               // throw
            }

            var currentState = await StateManager.TryGetStateAsync<string>(CurrentStateLabel);
            if (currentState.HasValue) {
               var theEvents = new List<IEvent>();
               var iterationState = currentState.Value;

               theEvents.Add(anEvent);
               while (theEvents.Count > 0) {
                  var iterationEvent = theEvents[0];
                  if (statesMap.List.TryGetValue(iterationState, out IState exitState)) {

                     var transition = exitState.GetActivatedTransition(iterationEvent);
                     if (transition != null) {

                        if (transition.TargetState != null && transition.TargetState != iterationState) {
                           await exitState.ExitActionAsync(this);
                        }

                        var chainEvents = await transition.Action(anEvent, this);

                        if (transition.TargetState != null && transition.TargetState != iterationState) {
                           if (statesMap.List.TryGetValue(transition.TargetState, out IState entryState)) {
                              await entryState.EntryActionAsync(this);
                              iterationState = transition.TargetState;
                           } else {
                              //throw
                           }

                           
                        }

                        theEvents.RemoveAt(0);
                        if (chainEvents != null) {
                           theEvents.AddRange(chainEvents);
                        }

                     } else {
                        // throw
                     }
                  } else {
                     // throw
                  }
               }

               await StateManager.SetStateAsync<string>(CurrentStateLabel, iterationState);
               pastEvents.Value[anEvent.Type.ToString()] = anEvent.Id;
               await StateManager.SetStateAsync<Dictionary<string, DateTime>>(PastEventsLabel, pastEvents.Value);
            } else {
               // throw
            }
         } else {
            // throw
         }
      }

   }
}