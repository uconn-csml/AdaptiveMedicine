using System;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace AdaptiveMedicine.Experiments.Actors.Interfaces {
   public interface IExperimentManagerActor: IActor {
      Task<bool> Create(DateTime timeStamp, object parameters);
      Task<bool> AddPatient(DateTime timeStamp, object parameters);
   }
}
