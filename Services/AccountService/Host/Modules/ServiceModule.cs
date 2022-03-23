using Autofac;
using Autofac.Extras.DynamicProxy;
using Client.ServerSymbol.Events;
using InfrastructureBase;
using InfrastructureBase.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

            //事务拦截
            var interceptorServiceTypes = new List<Type>();
            builder.RegisterType<UnitOfWorkInterceptor>();
            builder.RegisterType<UnitOfWorkAsyncInterceptor>();
            interceptorServiceTypes.Add(typeof(UnitOfWorkInterceptor));


            //builder.RegisterModule(new InfrastructureBase.Module());
            //var baseServices = Assembly.Load("InfrastructureBase");
            //builder.RegisterAssemblyTypes(baseServices)
            //.Where(a => a.Name.EndsWith("Service") || a.Name.EndsWith("Provider"))
            //.AsImplementedInterfaces()
            //.InstancePerLifetimeScope()http://localhost:3500/
            //.PropertiesAutowired();// 属性注入




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
            .Where(a => a.Name.EndsWith("Repository"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .PropertiesAutowired();// 属性注入

            //泛型注入
            builder.RegisterGeneric(typeof(RepositoryBase<>)).As(typeof(IRepositoryBase<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(RepositoryBase<,>)).As(typeof(IRepositoryBase<,>)).InstancePerLifetimeScope();
        }
    }
}
