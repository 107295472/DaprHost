using AgileConfig.Client;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Host.Db;
using Host.Modules;
using Infrastructure.Http;
using InfrastructureBase;
using InfrastructureBase.AopFilter;
using InfrastructureBase.Cache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mesh.Dapr;
using Host;
using Server.Kestrel.Implements;
using IocModule;
using ProxyGenerator.Implements;

IConfiguration? Configuration = default;
//配置中心
var client = new ConfigClient();
client.ConnectAsync().GetAwaiter().GetResult();
client.ConfigChanged += Client_ConfigChanged;
App.Client = client;
var builder = Application.CreateBuilder(config =>
{
    config.Port = 8001;
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
builder.Services.AddMemoryCache();
//builder.Services.InjectionCached<L1Cache, L2Cache>();
//注册自定义HostService
builder.Services.AddHostedService<CustomerService>();

//uuid
BaseCommon.RegGlobalID(Convert.ToUInt16(Configuration.GetSection("uuid").Value));
//db
builder.Services.AddDbAsync();
#region 缓存

if (AppConfig.CacheType == CacheType.Redis)
{
    var csredis = new CSRedis.CSRedisClient(AppConfig.RedisConnStr);
    RedisHelper.Initialization(csredis);
    builder.Services.AddSingleton<ICache, RedisCache>();
}
else
{
    builder.Services.AddSingleton<ICache, MemoryCache>();
}

#endregion 缓存
//注册全局拦截器
LocalMethodAopProvider.RegisterPipelineHandler(AopHandlerProvider.ContextHandler, AopHandlerProvider.BeforeSendHandler, AopHandlerProvider.AfterMethodInvkeHandler, AopHandlerProvider.ExceptionHandler);
//注册鉴权拦截器
AccountAuthenticationHandler.RegisterAllFilter();
//注册自定义Attribute AOP拦截器(需要注册全局拦截器才有效)
AopFilterManager.RegisterAllFilter();
builder.Services.AddLogging(configure =>
{
    configure.AddConfiguration(Configuration.GetSection("Logging"));
    configure.AddConsole();
});
builder.Services.AddAutofac();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
var app = builder.Build();
ActorStartup.Configure(app, app.Services);
await app.RunAsync();




static void Client_ConfigChanged(ConfigChangedArg obj)
{
    if (obj != null)
    {
        //App.Load();
    }
}
