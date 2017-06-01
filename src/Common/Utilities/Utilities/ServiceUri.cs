using System;
using System.Fabric;
using System.Runtime.CompilerServices;

namespace AdaptiveMedicine.Common.Utilities {
   public static class ServiceUri {

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static Uri ConstructFrom(string serviceName) {
         return new Uri($"{FabricRuntime.GetActivationContext().ApplicationName}/{serviceName}");
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static Uri ToServiceUri(this string serviceName) {
         return new Uri($"{FabricRuntime.GetActivationContext().ApplicationName}/{serviceName}");
      }
   }
}