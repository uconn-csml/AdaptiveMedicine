using System;
using System.Threading.Tasks;
using AdaptiveMedicine.Experiments.AlgorithmActor;
using Microsoft.ServiceFabric.Actors;

namespace AdaptiveMedicine.Experiments.Actors.Interfaces {
   public interface IAlgorithmActor: IActor {
      Task<bool> ConfigurateAsync(DateTime timeStamp, ConfigurationOptions config);
      Task<bool> ProcessNewSignalAsync(DateTime timeStamp, double value);
   }
}
