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
            var host = Environment.GetEnvironmentVariable("HostIP");
            if (host == null)
                throw new ApplicationException("请设置宿主机HostIP变量");

            var apisixUrl= Environment.GetEnvironmentVariable("ApisixUrl");
            if(apisixUrl == null)
                throw new ApplicationException("请设置网关ApisixUrl变量");

            await RegConsul(apisixUrl,host);
            //_ = Task.Delay(20 * 1000).ContinueWith(async t =>
            //{
            //    var port = Environment.GetEnvironmentVariable("DAPR_HTTP_PORT");
            //    Console.WriteLine($"端口号：{port}");
            //});
            //_ = Task.Delay(20 * 1000).ContinueWith(async t => await stateManager.SetState(new PermissionState() { Key = "account", Data = AuthenticationManager.AuthenticationMethods }));
            await Task.CompletedTask;
        }
        public  async Task RegConsul(string apisix,string host)
        {
            ConsulClientConfiguration c=new ConsulClientConfiguration();
            c.Address = new Uri(apisix);
            using var client = new ConsulClient(c);
            string json ="{\"weight\": 1, 	\"max_fails\": 2, 	\"fail_timeout\": 1 }";
            string key = $"upstreams/webpages/{host}";
            var result=await client.KV.Get(key);
            if (result.Response==null)
            {
                var putPair = new KVPair(key)
                {
                    Value = Encoding.UTF8.GetBytes(json)
                };
                _= await client.KV.Put(putPair);
            }
           
        }
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
