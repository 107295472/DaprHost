using IApplicationService.AccountService.Dtos.Input;
using IApplicationService.AppEvent;
using InfrastructureBase.AuthBase;
using Microsoft.Extensions.Hosting;
using Client.ServerProxyFactory.Interface;
using Client.ServerSymbol.Events;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Host
{
    public class CustomerService : IHostedService
    {
        private readonly IEventBus eventBus;
        public CustomerService(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //_ = Task.Delay(20 * 1000).ContinueWith(async t => await stateManager.SetState(new PermissionState() { Key = "account", Data = AuthenticationManager.AuthenticationMethods }));
            //await Task.CompletedTask;
        }
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
