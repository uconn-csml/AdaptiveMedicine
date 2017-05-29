using System;
using System.Threading.Tasks;
using AdaptiveMedicine.Common.Utilities;
using AdaptiveMedicine.Experiments.Actors.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace AdaptiveMedicine.Experiments.API.Controllers {

   [Route("api/[controller]")]
   public class ConfigurationController : Controller {

      [HttpGet]
      public async Task<IActionResult> Get() {
         var randomActorId = ActorId.CreateRandom();
         var actor = ActorProxy.Create<IExperimentManagerActor>(randomActorId, ExperimentManagerActor.ServiceName.ToServiceUri());
         var result = await actor.Create(DateTime.Now, null);

         return Ok($"[{result}] => {randomActorId.ToString()}");
      }
   }
}
