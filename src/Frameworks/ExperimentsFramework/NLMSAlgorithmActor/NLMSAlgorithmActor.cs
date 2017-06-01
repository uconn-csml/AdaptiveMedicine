using System;
using System.Threading.Tasks;
using AdaptiveMedicine.Common.Actors;
using AdaptiveMedicine.Experiments.Actors.Interfaces;
using AdaptiveMedicine.Experiments.Actors.ServiceNames;
using AdaptiveMedicine.Experiments.AlgorithmActor;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace AdaptiveMedicine.Experiments.Actors {
   [ActorService(Name = NLMSAlgorithmService.Name)]
   [StatePersistence(StatePersistence.Persisted)]
   internal class NLMSAlgorithmActor: StatechartActor, IAlgorithmActor {

      public NLMSAlgorithmActor(ActorService actorService, ActorId actorId)
          : base(actorService, actorId) {
      }

      protected override Task OnActivateAsync() {
         ActorEventSource.Current.ActorMessage(this, "Actor activated.");
         return base.OnActivateAsync();
      }

      public Task<bool> ConfigurateAsync(DateTime timeStamp, ConfigurationOptions config) {
         return Task.FromResult(true);
      }

      public Task<bool> ProcessNewSignalAsync(DateTime timeStamp, double value) {
         return Task.FromResult(true);
      }

   }
}
