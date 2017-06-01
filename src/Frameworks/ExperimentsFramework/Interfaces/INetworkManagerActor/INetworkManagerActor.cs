using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace AdaptiveMedicine.Experiments.Actors.Interfaces {
   public interface INetworkManagerActor: IActor {
      Task<bool> ConfigureModelsAsync();
   }
}
