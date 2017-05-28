using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdaptiveMedicine.Actors.Base.Statechart;
using AdaptiveMedicine.Actors.Base.Statechart.Attributes;
using AdaptiveMedicine.Actors.Base.Statechart.Interfaces;
using AdaptiveMedicine.Actors.Interfaces.AlgorithmManager;
using AdaptiveMedicine.Actors.Interfaces.ExperimentManager;
using AdaptiveMedicine.Actors.Interfaces.ModelManager;
using AdaptiveMedicine.Actors.Interfaces.NetworkManager;
using AdaptiveMedicine.Actors.Interfaces.PerformanceManager;
using Common.ServiceUri;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace AdaptiveMedicine.Actors.ExperimentManager {
   [ActorService(Name = Interfaces.ExperimentManager.ServiceNames.ExperimentManagerActor)]
   [StatePersistence(StatePersistence.Persisted)]
   internal class ExperimentManagerActor: StatechartActor, IExperimentManagerActor {
      public const string ParametersLabel = "Experiment.Parameters";

      public ExperimentManagerActor(ActorService actorService, ActorId actorId)
          : base(actorService, actorId) {
      }

      protected override Task OnActivateAsync() {
         ActorEventSource.Current.ActorMessage(this, "Actor activated.");
         return Task.FromResult(true);
      }

      async Task<bool> IExperimentManagerActor.CreateExperiment(DateTime timeStamp, object parameters) {
         await DispatchEventAsync(new StatechartEvent(Events.Initialize, timeStamp, parameters));
         return true;
      }

      /* Statechart Events & States */
      enum Events { Initialize, Delete, AddPatient, Error }
      enum States { Uninitialized, Initialized, Illegal }


      [InitialState(States.Uninitialized)]
      private sealed class Uninitialized: StatechartState {
         #region Singleton Pattern
         private static readonly Lazy<Uninitialized> _Lazy = new Lazy<Uninitialized>(() => new Uninitialized());
         public static Uninitialized Instance { get { return _Lazy.Value; } }
         private Uninitialized() { }
         #endregion

         [Transition(Events.Initialize, States.Initialized)]
         public async Task<IEnumerable<IEvent>> SetConfigurationAsync(IEvent anEvent, Actor actor) {
            // Save all the parameters related to the experiment so that they can be used when adding a new patient.
            var parameters = anEvent.Input as object;
            await actor.StateManager.SetStateAsync<object>(ParametersLabel, parameters);

            // Set up the parameters for the algorithm manager.
            await ActorProxy.Create<IAlgorithmManagerActor>(actor.Id, Interfaces.AlgorithmManager.ServiceNames.AlgorithmManagerActor.ToServiceUri()).ConfigureModelsAsync();
            return null;
         }

         [Transition(Events.Delete)]
         [Transition(Events.AddPatient, States.Illegal)]
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

         [Transition(Events.Initialize)]
         public async Task<IEnumerable<IEvent>> UpdateConfigurationAsync(IEvent anEvent, Actor actor) {
            // Update the parameters related to the experiment.
            var parameters = anEvent.Input as object;
            await actor.StateManager.SetStateAsync<object>(ParametersLabel, parameters);

            // Updates the parameters for the algorithm manager.
            await ActorProxy.Create<IAlgorithmManagerActor>(actor.Id, Interfaces.AlgorithmManager.ServiceNames.AlgorithmManagerActor.ToServiceUri()).ConfigureModelsAsync();
            return null;
         }

         [Transition(Events.Delete, States.Uninitialized)]
         [Transition(Events.Error, States.Illegal)]
         public Task<IEnumerable<IEvent>> DoNothingAsync(IEvent anEvent, Actor actor) {
            return Task.FromResult<IEnumerable<IEvent>>(null);
         }

         [Transition(Events.AddPatient)]
         public async Task<IEnumerable<IEvent>> AddPatientAsync(IEvent anEvent, Actor actor) {
            var parameters = await actor.StateManager.TryGetStateAsync<object>(ParametersLabel);
            var forwardedEvents = new List<IEvent>();

            if (parameters.HasValue) {
               var patientId = anEvent.Input as ActorId;
               var configuringManagers = new Task<bool>[] {
                  ActorProxy.Create<IModelManagerActor>(patientId, Interfaces.ModelManager.ServiceNames.ModelManagerActor.ToServiceUri()).ConfigureModelsAsync(),
                  ActorProxy.Create<INetworkManagerActor>(patientId, Interfaces.NetworkManager.ServiceNames.NetworkManagerActor.ToServiceUri()).ConfigureModelsAsync(),
                  ActorProxy.Create<IPerformanceManagerActor>(patientId, Interfaces.PerformanceManager.ServiceNames.PerformanceManagerActor.ToServiceUri()).ConfigureModelsAsync()
               };
               await Task.WhenAll(configuringManagers);

            } else {
               forwardedEvents.Add(new StatechartEvent(Events.Error, anEvent.Id));
            }

            return forwardedEvents;
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
            ActorEventSource.Current.ActorMessage(actor, "Something wrong happened to this experiment.");
            return base.EntryActionAsync(actor);
         }

         [Transition(Events.Initialize, States.Uninitialized)]
         public Task<IEnumerable<IEvent>> ForwardEventAsync(IEvent anEvent, Actor actor) {
            return Task.FromResult<IEnumerable<IEvent>>(new IEvent[] { anEvent });
         }

         [Transition(Events.Delete, States.Uninitialized)]
         [Transition(Events.AddPatient)]
         [Transition(Events.Error)]
         public Task<IEnumerable<IEvent>> DoNothingAsync(IEvent anEvent, Actor actor) {
            return Task.FromResult<IEnumerable<IEvent>>(null);
         }
      }

   }
}
