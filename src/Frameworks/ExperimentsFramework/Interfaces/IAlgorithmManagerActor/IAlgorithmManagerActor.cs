using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace AdaptiveMedicine.Experiments.Actors.Interfaces {
   public interface IAlgorithmManagerActor: IActor {
      Task<bool> ConfigureModelsAsync();
   }
}
