using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using AdaptiveMedicine.Actors.Interfaces.ModelManager;

namespace AdaptiveMedicine.Actors.ModelManager {
   [StatePersistence(StatePersistence.Persisted)]
   internal class ModelManagerActor: Actor, IModelManagerActor {

      public ModelManagerActor(ActorService actorService, ActorId actorId)
          : base(actorService, actorId) {
      }

      protected override Task OnActivateAsync() {
         ActorEventSource.Current.ActorMessage(this, "Actor activated.");
         return Task.FromResult(true);
      }

      public Task<bool> ConfigureModelsAsync() {
         return Task.FromResult<bool>(true);
      }

   }
}
