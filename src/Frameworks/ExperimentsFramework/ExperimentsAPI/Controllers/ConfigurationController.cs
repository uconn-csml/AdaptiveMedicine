using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdaptiveMedicine.Common.Utilities;
using AdaptiveMedicine.Experiments.Actors.Interfaces;
using AdaptiveMedicine.Experiments.Actors.ServiceNames;
using AdaptiveMedicine.Experiments.AlgorithmActor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace AdaptiveMedicine.Experiments.API.Controllers {
   using ConfigOptions = ExperimentManagerActor.ConfigurationOptions;

   [Route("api/[controller]")]
   public class ConfigurationController : Controller {

      [HttpGet]
      public async Task<IActionResult> Get() {
         var modelsInfo = new List<ConfigOptions.ModelInfo> {
            new ConfigOptions.ModelInfo { Order = 1, Algorithm = AlgorithmCatalog.NLMS.ToString() },
            new ConfigOptions.ModelInfo { Order = 2, Algorithm = AlgorithmCatalog.NLMS.ToString() },
            new ConfigOptions.ModelInfo { Order = 3, Algorithm = AlgorithmCatalog.NLMS.ToString() },
            new ConfigOptions.ModelInfo { Order = 5, Algorithm = AlgorithmCatalog.NLMS.ToString() },
            new ConfigOptions.ModelInfo { Order = 9, Algorithm = AlgorithmCatalog.NLMS.ToString() }
         };
         var config = new ConfigOptions { ModelsInfo = modelsInfo };

         var randomActorId = new ActorId($"{ActorId.CreateRandom().ToString()}");
         var actor = ActorProxy.Create<IExperimentManagerActor>(randomActorId, ExperimentManagerService.Name.ToServiceUri());
         var result = await actor.ConfigurateAsync(DateTime.Now, config);

         return Ok($"[{result}] => {randomActorId}");
      }
   }
}
