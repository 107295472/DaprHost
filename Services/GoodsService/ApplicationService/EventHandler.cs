using Autofac;
using Client.ServerProxyFactory.Interface;
using Client.ServerSymbol.Events;
using Domain.Entities;
using Domain.Repository;
using FreeSql;
using IApplicationService.AccountService.Dtos.Event;
using IApplicationService.Base.AppEvent;
using IApplicationService.GoodsService.Dtos.Event;
using IApplicationService.TradeService;
using IApplicationService.TradeService.Dtos.Input;
using IApplicationService.TradeService.Dtos.Output;
using InfrastructureBase;
using InfrastructureBase.Http;
using InfrastructureBase.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationService
{
    public class EventHandler : IEventHandler
    {
        private readonly IEventBus eventBus;
        private readonly ILocalEventBus localEventBus;
        private readonly IStateManager stateManager;
        private readonly IBaseRepository<Goods> repo;
        public EventHandler(IEventBus eventBus, ILocalEventBus localEventBus, IStateManager stateManager)
        {
            this.eventBus = eventBus;
            this.localEventBus = localEventBus;
            this.stateManager = stateManager;
        }

        [EventHandlerFunc(EventTopicDictionary.Account.InitTestUserSuccess)]
        public async Task<DefaultEventHandlerResponse> EventHandleSetDefMallSetting(EventHandleRequest<LoginSuccessDto> input)
        {
            return await new DefaultEventHandlerResponse().RunAsync(nameof(EventHandleSetDefMallSetting), input.GetDataJson(), async () =>
            {
                Console.WriteLine("test*****************");
                
            });
        }
        [EventHandlerFunc(EventTopicDictionary.Goods.UpdateGoodsToEs)]
        public async Task<DefaultEventHandlerResponse> EventHandleEventUpdateGoodsToEs(EventHandleRequest<UpdateGoodsToEsDto> input)
        {
            return await new DefaultEventHandlerResponse().RunAsync(nameof(EventHandleEventUpdateGoodsToEs), input.GetDataJson(), async () =>
            {
                Console.WriteLine($"调用了：{input.GetData()}");
            });
        }
        [EventHandlerFunc(EventTopicDictionary.Account.ATest)]
        public async Task<DefaultEventHandlerResponse> AtestControler(EventHandleRequest<string> input)
        {
            return await new DefaultEventHandlerResponse().RunAsync(nameof(AtestControler), input.GetDataJson(), async () =>
            {
                Console.WriteLine(input.GetData());

            });
        }
    }
}
