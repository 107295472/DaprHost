using Autofac;
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
        public CustomerService(IEventBus eventBus, IStateManager stateManager,ILifetimeScope lifetimeScope)
        {
            this.eventBus = eventBus;
            this.stateManager = stateManager;
            //注册本地事件总线订阅器
            //EventHandleRunner.RegisterAndRun<IEsGoodsRepository, Domain.Entities.Goods>(lifetimeScope, EventTopicDictionary.Goods.Loc_WriteToElasticsearch, nameof(IEsGoodsRepository.WriteToElasticsearch));
            //EventHandleRunner.RegisterAndRun<IEsGoodsRepository, Domain.Entities.Goods>(lifetimeScope, EventTopicDictionary.Goods.Loc_RemoveToElasticsearch, nameof(IEsGoodsRepository.RemoveToElasticsearch));
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //_ = Task.Delay(20 * 1000).ContinueWith(async t => await stateManager.SetState(new PermissionState() { Key = "goods", Data = AuthenticationManager.AuthenticationMethods }));
            await Task.CompletedTask;
        }
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
