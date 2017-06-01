using System;
using System.Threading.Tasks;
using AdaptiveMedicine.Experiments.ModelManagerActor;
using Microsoft.ServiceFabric.Actors;

namespace AdaptiveMedicine.Experiments.Actors.Interfaces {
   public interface IModelManagerActor: IActor {
      Task<bool> ConfigurateAsync(DateTime timeStamp, ConfigurationOptions config);
   }
}
