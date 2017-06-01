using System;
using System.Threading.Tasks;
using AdaptiveMedicine.Common.Utilities;
using AdaptiveMedicine.Experiments.Actors.Interfaces;
using AdaptiveMedicine.Experiments.Actors.ServiceNames;
using AdaptiveMedicine.Experiments.ExperimentManagerActor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace AdaptiveMedicine.Experiments.API.Controllers {

   [Route("api/[controller]")]
   public class ParticipationController: Controller {

      [HttpGet("{experimentId}/{participantId}")]
      public async Task<IActionResult> Get(string experimentId, string participantId) {
         var participant = new ParticipantDetails {
            Id = participantId
         };

         var actor = ActorProxy.Create<IExperimentManagerActor>(new ActorId(experimentId), ExperimentManagerService.Name.ToServiceUri());
         var result = await actor.AddParticipantAsync(DateTime.Now, participant);

         return Ok($"[{result}] => {experimentId}");
      }
   }
}
