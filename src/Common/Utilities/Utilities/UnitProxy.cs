using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace Common.UnitProxy {
   public static class UnitProxy {

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static ActorType Create<ActorType>(Uri serviceUri) where ActorType: IActor {
         var newActorId = ActorId.CreateRandom();
         return ActorProxy.Create<ActorType>(newActorId, serviceUri);
      }
   }
}
