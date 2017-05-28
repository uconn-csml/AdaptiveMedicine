using System;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace AdaptiveMedicine.Actors.Interfaces.ExperimentManager {
   public interface IExperimentManagerActor: IActor {
      Task<bool> CreateExperiment(DateTime timeStamp, object parameters);
   }
}
