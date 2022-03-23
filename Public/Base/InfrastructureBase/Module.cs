using Autofac;
using Common.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfrastructureBase
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly).Where(a => a.Name.EndsWith("Service") || a.Name.EndsWith("Provider"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
