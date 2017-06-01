using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.ServiceFabric.Services.Runtime;

namespace AdaptiveMedicine.Experiments.API {
   internal static class Program {
      private static void Main() {
         try {
            ServiceRuntime.RegisterServiceAsync("ExperimentsAPIType",
               context => new ExperimentsAPI(context)).GetAwaiter().GetResult();

            ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(ExperimentsAPI).Name);
            Thread.Sleep(Timeout.Infinite);
         } catch (Exception e) {
            ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
            throw;
         }
      }
   }
}
