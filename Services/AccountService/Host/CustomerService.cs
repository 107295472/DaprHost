using Castle.Core.Configuration;
using Client.ServerProxyFactory.Interface;
using Consul;
using IApplicationService.AccountService.Dtos.Input;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Helpers;
using Microsoft.Extensions.Hosting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Host
{
    public class CustomerService : IHostedService
    {
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        public CustomerService(IEventBus eventBus, IStateManager stateManager)
        {
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await RegConsul();
            //_ = Task.Delay(20 * 1000).ContinueWith(async t =>
            //{
            //    var port = Environment.GetEnvironmentVariable("DAPR_HTTP_PORT");
            //    Console.WriteLine($"端口号：{port}");
            //});
            //_ = Task.Delay(20 * 1000).ContinueWith(async t => await stateManager.SetState(new PermissionState() { Key = "account", Data = AuthenticationManager.AuthenticationMethods }));
            await Task.CompletedTask;
        }
        public  async Task RegConsul()
        {
            ConsulClientConfiguration c=new ConsulClientConfiguration();
            c.Address = new Uri("http://192.168.10.21:8500");
            using (var client = new ConsulClient(c))
            {
                
                var putPair = new KVPair("acc")
                {
                    Value = Encoding.UTF8.GetBytes(AppSettings.Configuration["HostIP"])
                };

                var putAttempt = await client.KV.Put(putPair);
            }
        }
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
