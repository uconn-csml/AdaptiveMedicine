using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace AdaptiveMedicine.Actors.Interfaces.ModelManager {
   public interface IModelManagerActor: IActor {
      Task<bool> ConfigureModelsAsync();
   }
}
