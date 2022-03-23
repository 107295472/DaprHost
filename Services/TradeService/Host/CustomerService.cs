using Client.ServerProxyFactory.Interface;
using IApplicationService.AccountService.Dtos.Input;
using InfrastructureBase.AuthBase;
using Microsoft.Extensions.Hosting;
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
            //_ = Task.Delay(20 * 1000).ContinueWith(async t => await stateManager.SetState(new PermissionState() { Key = "trade", Data = AuthenticationManager.AuthenticationMethods }));
            await Task.CompletedTask;
        }
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
