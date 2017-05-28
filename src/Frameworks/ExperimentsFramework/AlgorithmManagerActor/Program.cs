using System;
using System.Threading;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace AdaptiveMedicine.Actors.AlgorithmManager {
   internal static class Program {
      private static void Main() {
         try {
            ActorRuntime.RegisterActorAsync<AlgorithmManagerActor>(
               (context, actorType) => new ActorService(context, actorType)).GetAwaiter().GetResult();
            Thread.Sleep(Timeout.Infinite);
         } catch (Exception e) {
            ActorEventSource.Current.ActorHostInitializationFailed(e.ToString());
            throw;
         }
      }
   }
}
