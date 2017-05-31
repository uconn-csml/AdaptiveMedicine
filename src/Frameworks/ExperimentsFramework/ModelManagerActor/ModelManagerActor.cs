using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdaptiveMedicine.Common.Actors;
using AdaptiveMedicine.Common.Statechart.Attributes;
using AdaptiveMedicine.Common.Statechart.Interfaces;
using AdaptiveMedicine.Common.Utilities;
using AdaptiveMedicine.Experiments.Actors.Interfaces;
using AdaptiveMedicine.Experiments.Actors.ServiceNames;
using AdaptiveMedicine.Experiments.AlgorithmActor;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace AdaptiveMedicine.Experiments.Actors {
   using AlgorithmConfigOptions = Experiments.AlgorithmActor.ConfigurationOptions;
   using ConfigOptions = Experiments.ModelManagerActor.ConfigurationOptions;

   [ActorService(Name = ModelManagerService.Name)]
   [StatePersistence(StatePersistence.Persisted)]
   internal class ModelManagerActor: StatechartActor, IModelManagerActor {
      public const string ConfigurationLabel = "Models.Configuration";
      public const string ModelsListLabel = "Models.List";

      public ModelManagerActor(ActorService actorService, ActorId actorId)
          : base(actorService, actorId) {
      }

      protected override Task OnActivateAsync() {
         ActorEventSource.Current.ActorMessage(this, "Actor activated.");
         return base.OnActivateAsync();
      }

      public async Task<bool> ConfigurateAsync(DateTime timeStamp, ConfigOptions config) {
         await DispatchEventAsync(new StatechartEvent(Events.Initialize, timeStamp, config));
         return true;
      }

      /* Statechart Events & States */
      enum Events { Initialize, Delete, Error }
      enum States { Uninitialized, Initialized, Illegal }


      [InitialState(States.Uninitialized)]
      private sealed class Uninitialized: StatechartState {
         #region Singleton Pattern
         private static readonly Lazy<Uninitialized> _Lazy = new Lazy<Uninitialized>(() => new Uninitialized());
         public static Uninitialized Instance { get { return _Lazy.Value; } }
         private Uninitialized() : base() { }
         #endregion

         [Transition(Events.Initialize, States.Initialized)]
         public async Task<IEnumerable<IEvent>> SetConfigurationAsync(IEvent anEvent, Actor actor) {
            var forwardedEvents = new List<IEvent>();

            var config = anEvent.Input as ConfigOptions;
            if (config != null) {

               var modelsList = new Dictionary<string, string>();
               var configuringAlgorithms = new List<Task<bool>>();

               foreach (var modelInfo in config.ModelsInfo) {
                  var algorithmConfig = new AlgorithmConfigOptions {
                     Order = modelInfo.Order,
                     Algorithm = modelInfo.Algorithm
                  };

                  var algorithmId = $"{actor.Id}:{Guid.NewGuid()}";
                  while (modelsList.ContainsKey(algorithmId)) {
                     algorithmId = $"{actor.Id}:{Guid.NewGuid()}";
                  }

                  var serviceUri = algorithmConfig.Algorithm.GetServiceName().ToServiceUri();

                  configuringAlgorithms.Add(
                     ActorProxy.Create<IAlgorithmActor>(new ActorId(algorithmId), serviceUri)
                        .ConfigurateAsync(anEvent.Id, algorithmConfig));

                  modelsList[algorithmId] = algorithmConfig.Algorithm;
               }

               await Task.WhenAll(configuringAlgorithms);
               await actor.StateManager.SetStateAsync<Dictionary<string, string>>(ModelsListLabel, modelsList);

            } else {
               forwardedEvents.Add(new StatechartEvent(Events.Error, anEvent.Id));
            }

            return forwardedEvents;
         }

         [Transition(Events.Delete)]
         [Transition(Events.Error, States.Illegal)]
         public Task<IEnumerable<IEvent>> DoNothingAsync(IEvent anEvent, Actor actor) {
            return Task.FromResult<IEnumerable<IEvent>>(null);
         }
      }

      [State(States.Initialized)]
      private sealed class Initialized: StatechartState {
         #region Singleton Pattern
         private static readonly Lazy<Initialized> _Lazy = new Lazy<Initialized>(() => new Initialized());
         public static Initialized Instance { get { return _Lazy.Value; } }
         private Initialized() : base() { }
         #endregion

         [Transition(Events.Initialize, States.Illegal)]
         [Transition(Events.Delete, States.Uninitialized)]
         [Transition(Events.Error, States.Illegal)]
         public Task<IEnumerable<IEvent>> DoNothingAsync(IEvent anEvent, Actor actor) {
            return Task.FromResult<IEnumerable<IEvent>>(null);
         }
      }

      [State(States.Illegal)]
      private sealed class Illegal: StatechartState {
         #region Singleton Pattern
         private static readonly Lazy<Illegal> _Lazy = new Lazy<Illegal>(() => new Illegal());
         public static Illegal Instance { get { return _Lazy.Value; } }
         private Illegal() : base() { }
         #endregion

         public override Task<bool> EntryActionAsync(Actor actor) {
            ActorEventSource.Current.ActorMessage(actor, "Something wrong happened to this manager.");
            return base.EntryActionAsync(actor);
         }

         [Transition(Events.Initialize, States.Uninitialized)]
         public Task<IEnumerable<IEvent>> ForwardEventAsync(IEvent anEvent, Actor actor) {
            return Task.FromResult<IEnumerable<IEvent>>(new IEvent[] { anEvent });
         }

         [Transition(Events.Delete, States.Uninitialized)]
         [Transition(Events.Error)]
         public Task<IEnumerable<IEvent>> DoNothingAsync(IEvent anEvent, Actor actor) {
            return Task.FromResult<IEnumerable<IEvent>>(null);
         }
      }
   }
}
