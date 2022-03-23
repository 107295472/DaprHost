using Client.ServerProxyFactory.Interface;
using Client.ServerSymbol.Events;
using Domain.Entities;
using IApplicationService.Base.AppEvent;
using IApplicationService.PublicService.Dtos.Event;
using InfrastructureBase.Data;
using InfrastructureBase.Object;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ApplicationService
{
    public class EventHandler : IEventHandler
    {
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        private readonly ILogger<EventHandler> logger;
        public EventHandler(IEventBus eventBus, IStateManager stateManager, ILogger<EventHandler> logger)
        {
            this.logger = logger;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }
        [EventHandlerFunc(EventTopicDictionary.Common.EventHandleErrCatch)]
        public async Task<DefaultEventHandlerResponse> EventHandleErrCatch(EventHandleRequest<EventHandlerErrDto> input)
        {
            try
            {
                var entity = input.GetData().CopyTo<EventHandlerErrDto, EventHandleErrorInfo>();
                var logdb=Mongo.GetRepository<EventHandleErrorInfo>(nameof(EventHandleErrorInfo));
                await logdb.AddAsync(entity);
            }
            catch (Exception e)
            {
                logger.LogError($"事件订阅器异常处理持久化失败,异常信息:{e.Message}");
            }
            return DefaultEventHandlerResponse.Default();
        }
        [EventHandlerFunc(EventTopicDictionary.Account.InitTestUserSuccess)]
        public async Task<DefaultEventHandlerResponse> EventHandleSetDefMallSetting(EventHandleRequest<string> input)
        {
            return await new DefaultEventHandlerResponse().RunAsync(nameof(EventHandleSetDefMallSetting), input.GetDataJson(), async () =>
            {

            });
        }
    }
}
