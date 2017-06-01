using System.Collections.Generic;
using System.Fabric;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace AdaptiveMedicine.Experiments.API {
   internal sealed class ExperimentsAPI : StatelessService {
      public ExperimentsAPI(StatelessServiceContext context)
         : base(context) {
      }

      protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners() {
         return new ServiceInstanceListener[] {
            new ServiceInstanceListener(serviceContext =>
               new WebListenerCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) => {
                  ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting WebListener on {url}");

                  return new WebHostBuilder().UseWebListener()
                     .ConfigureServices(
                        services => services
                        .AddSingleton<StatelessServiceContext>(serviceContext))
                     .UseContentRoot(Directory.GetCurrentDirectory())
                     .UseStartup<Startup>()
                     .UseApplicationInsights()
                     .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                     .UseUrls(url)
                     .Build();
               })
            )
         };
      }
   }
}