using System;
using System.Threading.Tasks;
using AdaptiveMedicine.Experiments.ExperimentManagerActor;
using Microsoft.ServiceFabric.Actors;

namespace AdaptiveMedicine.Experiments.Actors.Interfaces {
   public interface IExperimentManagerActor: IActor {
      Task<bool> ConfigurateAsync(DateTime timeStamp, ConfigurationOptions config);
      Task<bool> AddParticipantAsync(DateTime timeStamp, ParticipantDetails participant);
   }
}
