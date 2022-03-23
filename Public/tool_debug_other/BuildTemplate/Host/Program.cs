using Autofac;
using Autofac.Extensions.DependencyInjection;
using Host.Modules;
using Infrastructure.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using IocModule;
using Mesh.Dapr;
using ProxyGenerator.Implements;
using Server.Kestrel.Implements;
using System.IO;
using System.Threading.Tasks;

namespace Host
{
    class Program
    {
        private static IConfiguration _configuration { get; set; }
        static async Task Main(string[] args)
        {
            await CreateDefaultHost(args).Build().RunAsync();
        }

        static IHostBuilder CreateDefaultHost(string[] args) => new HostBuilder()
                .ConfigureWebHostDefaults(webhostbuilder => {
                    //注册成为oxygen服务节点
                    webhostbuilder.StartOxygenServer<OxygenActorStartup>((config) => {
                        config.Port = 80;
                        config.PubSubCompentName = "pubsub";
                        config.StateStoreCompentName = "statestore";
                        config.TracingHeaders = "Authentication";
                    });
                })
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json");
                    _configuration = config.Build();
                })
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    //注入oxygen依赖
                    builder.RegisterModule();
                    //注入业务依赖
                    builder.RegisterModule(new ServiceModule());
                })
                .ConfigureServices((context, services) =>
                {
                    //注册自定义HostService
                    services.AddHostedService<CustomerService>();
                    //注册全局拦截器
                    LocalMethodAopProvider.RegisterPipelineHandler(AopHandlerProvider.ContextHandler, AopHandlerProvider.BeforeSendHandler, AopHandlerProvider.AfterMethodInvkeHandler, AopHandlerProvider.ExceptionHandler);
                    //注册鉴权拦截器
                    AuthenticationHandler.RegisterAllFilter();
                    
                      //uuid
                    BaseCommon.RegGlobalID(Convert.ToUInt16(Configuration.GetSection("Guid").Value));
                    //freesql
                    IFreeSql fsql = new FreeSqlBuilder()
                    .UseConnectionString(DataType.MySql, Configuration.GetSection("MySql").Value)
                    .UseAutoSyncStructure(true) //自动同步实体结构到数据库，FreeSql不会扫描程序集，只有CRUD时才会生成表。
                    .Build(); //请务必定义成 Singleton 单例模式
                    services.AddSingleton(fsql);
                    services.AddScoped<UnitOfWorkManager>();
                    fsql.Aop.CurdBefore += (s, e) => {
                        //Console.WriteLine(e.Sql);   
                    };
                    services.AddFreeRepository();
                    
                    services.AddLogging(configure =>
                    {
                        configure.AddConfiguration(_configuration.GetSection("Logging"));
                        configure.AddConsole();
                    });
                    services.AddAutofac();
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }
}