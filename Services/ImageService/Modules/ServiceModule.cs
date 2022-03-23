﻿using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;
using System.Linq;
using System.Runtime.Loader;
using InfrastructureBase;

namespace ImageService.Modules
{
    public class ServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(BaseCommon.GetProjectAssembliesArray()).Where(x => !BaseCommon.IsSystemType(x))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
