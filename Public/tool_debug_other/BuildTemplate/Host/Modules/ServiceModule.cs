using Autofac;
using Infrastructure.EfDataAccess;
using InfrastructureBase;
using InfrastructureBase.Data.Nest;
using Client.ServerSymbol.Events;
using System.Linq;

namespace Host.Modules
{
    public class ServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(BaseCommon.GetProjectAssembliesArray()).Where(x => !new[] { "Microsoft", "System" }.Any(y => x.AssemblyQualifiedName.Contains(y)))
                .AsImplementedInterfaces().Where(x => !(x is IEventHandler))
                .InstancePerLifetimeScope();
            //事件订阅器需要独立注册因为其接口相同
            BaseCommon.RegisterAllEventHandlerInAutofac(BaseCommon.GetProjectAssembliesArray(), builder);
            //注入其他基础设施依赖
             builder.RegisterType<LocalEventBus>().As<ILocalEventBus>().InstancePerLifetimeScope();

            //事务拦截
            var interceptorServiceTypes = new List<Type>();
            builder.RegisterType<UnitOfWorkInterceptor>();
            builder.RegisterType<UnitOfWorkAsyncInterceptor>();
            interceptorServiceTypes.Add(typeof(UnitOfWorkInterceptor));

            //服务
            var assemblyServices = Assembly.Load("ApplicationService");
            builder.RegisterAssemblyTypes(assemblyServices)
            .Where(a => a.Name.EndsWith("Service"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .PropertiesAutowired()// 属性注入
            .InterceptedBy(interceptorServiceTypes.ToArray())
            .EnableInterfaceInterceptors();



            //仓储
            var assemblyRepository = Assembly.Load("Infrastructure");
            builder.RegisterAssemblyTypes(assemblyRepository)
            .Where(a => a.Name.EndsWith("Repo"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .PropertiesAutowired();// 属性注入
        }
    }
}
