using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Common.Implements;
using ProxyGenerator.Implements;
using ProxyGenerator.Interface;

namespace ProxyGenerator
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly).Where(x => !ReflectionHelper.IsSystemType(x))
                .AsImplementedInterfaces().Where(x => !(x is IRemoteMessageSenderDelegate))
                .InstancePerLifetimeScope();
            RemoteProxyGenerator.CreateRemoteProxyAndRegisterInIocContainer(builder);
        }
    }
}
