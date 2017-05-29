using System.Threading.Tasks;
using AdaptiveMedicine.Experiments.Actors.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace AdaptiveMedicine.Experiments.Actors {
   [ActorService(Name = Experiments.NetworkManagerActor.ServiceName)]
   [StatePersistence(StatePersistence.Persisted)]
   internal class NetworkManagerActor: Actor, INetworkManagerActor {

      public NetworkManagerActor(ActorService actorService, ActorId actorId)
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
