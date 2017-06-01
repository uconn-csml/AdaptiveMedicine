using System;
using System.Runtime.CompilerServices;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace AdaptiveMedicine.Common.Utilities {
   public static class UnitProxy {

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static ActorType Create<ActorType>(Uri serviceUri) where ActorType: IActor {
         var newActorId = ActorId.CreateRandom();
         return ActorProxy.Create<ActorType>(newActorId, serviceUri);
      }
   }
}
