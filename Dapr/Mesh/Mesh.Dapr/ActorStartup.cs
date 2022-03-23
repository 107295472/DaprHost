using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Common.Interface;
using Server.Kestrel.Implements;
using Server.Kestrel.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesh.Dapr
{
    public class ActorStartup : CStartup
    {
        public static new void ConfigureServices(IServiceCollection service)
        {
            CStartup.ConfigureServices(service);
            ActorServiceFactory.RegisterActorService(service);
        }
        public static new void Configure(IApplicationBuilder appBuilder, IServiceProvider serviceProvider)
        {
            CStartup.Configure(appBuilder, serviceProvider);
            ActorServiceFactory.UseActorService(appBuilder, serviceProvider.GetService<ILifetimeScope>());
        }
    }
}
