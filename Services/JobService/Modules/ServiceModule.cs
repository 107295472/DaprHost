using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;
using System.Linq;
using System.Runtime.Loader;
using Microsoft.Extensions.Hosting;
using InfrastructureBase;

namespace JobService.Modules
{
    public class ServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            
            builder.RegisterAssemblyTypes(BaseCommon.GetProjectAssembliesArray()).Where(x => !BaseCommon.IsSystemType(x))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            //事件订阅器需要独立注册因为其接口相同
            BaseCommon.RegisterAllEventHandlerInAutofac(BaseCommon.GetProjectAssembliesArray(), builder);
        }
    }
}
