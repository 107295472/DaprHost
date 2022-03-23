﻿using Autofac;
using Autofac.Extensions.DependencyInjection;
using Host.Modules;
using Infrastructure.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mesh.Dapr;
using InfrastructureBase.AopFilter;
using Host;
using Saga;
using Saga.PubSub.Dapr;
using Saga.Store.Dapr;
using IApplicationService.Base.AppEvent;
using IApplicationService.PublicService.Dtos.Event;
using InfrastructureBase.Http;
using Server.Kestrel.Implements;
using Client.ServerProxyFactory.Interface;
using IocModule;
using ProxyGenerator.Implements;

IConfiguration Configuration = default;
var builder = Application.CreateBuilder(config =>
{
    config.Port = 8010;
    config.PubSubCompentName = "pubsub";
    config.StateStoreCompentName = "statestore";
    config.TracingHeaders = "Authentication,AuthIgnore";
    config.UseCors = true;
});
ActorStartup.ConfigureServices(builder.Services);
builder.Host.ConfigureAppConfiguration((hostContext, config) =>
{
    config.SetBasePath(Directory.GetCurrentDirectory());
    config.AddJsonFile("appsettings.json");
    Configuration = config.Build();
}).ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule();
    //注入业务依赖
    builder.RegisterModule(new ServiceModule());
});
builder.Services.AddHttpClient();
//注册自定义HostService
builder.Services.AddHostedService<CustomerService>();
//注册全局拦截器
LocalMethodAopProvider.RegisterPipelineHandler(AopHandlerProvider.ContextHandler, AopHandlerProvider.BeforeSendHandler, AopHandlerProvider.AfterMethodInvkeHandler, AopHandlerProvider.ExceptionHandler);
//注册鉴权拦截器
GoodsAuthenticationHandler.RegisterAllFilter();
//注册自定义Attribute AOP拦截器(需要注册全局拦截器才有效)
AopFilterManager.RegisterAllFilter();
builder.Services.AddLogging(configure =>
{
    configure.AddConfiguration(Configuration.GetSection("Logging"));
    configure.AddConsole();
});
builder.Services.AddAutofac();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Services.AddSaga(new SagaConfiguration("EshopSample", "GoodsService", null, null, new IApplicationService.Sagas.CreateOrder.Configuration()));
builder.Services.AddSagaStore();
var app = builder.Build();
app.RegisterSagaHandler(async (ss,error) =>
{
    
    //当出现补偿异常的saga流时，会触发这个异常处理器，需要人工进行处理(持久化消息/告警通知等等)
    //此处作为演示，我将会直接导入到事件异常服务
    await HttpContextExt.Current.RequestService.Resolve<IEventBus>().SendEvent(EventTopicDictionary.Common.EventHandleErrCatch,
                   new EventHandlerErrDto($"Saga流[{error.SourceTopic}]事件补偿异常", error.SourceDataJson, error.SourceException.Message, error.SourceException.StackTrace, false));
});
ActorStartup.Configure(app, app.Services);
await app.RunAsync();
