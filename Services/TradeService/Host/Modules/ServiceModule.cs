using Autofac;
using Client.ServerSymbol.Events;
using InfrastructureBase;
using System.Linq;

namespace Host.Modules
{
    public class ServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(BaseCommon.GetProjectAssembliesArray()).Where(x => !BaseCommon.IsSystemType(x))
                .AsImplementedInterfaces().Where(x => !(x is IEventHandler))
                .InstancePerLifetimeScope();
            //事件订阅器需要独立注册因为其接口相同
            BaseCommon.RegisterAllEventHandlerInAutofac(BaseCommon.GetProjectAssembliesArray(), builder);
            //注入其他基础设施依赖
            builder.RegisterType<LocalEventBus>().As<ILocalEventBus>().InstancePerLifetimeScope();
        }
    }
}
