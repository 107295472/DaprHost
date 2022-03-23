using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hangfire;
using IocModule;
using JobService;
using JobService.Modules;
using Server.Kestrel.Implements;

IConfiguration Configuration = default;
var builder = Application.CreateBuilder(config =>
{
    config.Port = 80;
    config.PubSubCompentName = "pubsub";
    config.StateStoreCompentName = "statestore";
    config.TracingHeaders = "Authentication,AuthIgnore";
});
CStartup.ConfigureServices(builder.Services);
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
builder.Services.AddLogging(configure =>
{
    configure.AddConfiguration(Configuration.GetSection("Logging"));
    configure.AddConsole();
});
GlobalConfiguration.Configuration.UseRedisStorage(Configuration.GetSection("RedisConnection").Value,new Hangfire.Redis.RedisStorageOptions {Db=10 });
builder.Services.AddHangfire(x => { });
builder.Services.AddHangfireServer();
builder.Services.AddHostedService<CronJobService>();//注册并运行所有的周期作业
builder.Services.AddAutofac();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
var app = builder.Build();
CStartup.Configure(app, app.Services);
await app.RunAsync();