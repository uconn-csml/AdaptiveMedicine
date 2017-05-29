using System;
using System.Threading.Tasks;
using AdaptiveMedicine.Common.Utilities;
using AdaptiveMedicine.Experiments.Actors.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace AdaptiveMedicine.Experiments.API.Controllers {

   [Route("api/[controller]")]
   public class ParticipationController: Controller {

      [HttpGet("{actorId}")]
      public async Task<IActionResult> Get(string actorId) {
         var actor = ActorProxy.Create<IExperimentManagerActor>(new ActorId(actorId), ExperimentManagerActor.ServiceName.ToServiceUri());
         var result = await actor.AddPatient(DateTime.Now, null);

         return Ok($"[{result}] => {actorId}");
      }
   }
}
