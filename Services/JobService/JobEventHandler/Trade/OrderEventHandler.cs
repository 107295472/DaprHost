﻿using Client.ServerProxyFactory.Interface;
using Client.ServerSymbol.Events;
using Hangfire;
using IApplicationService.Base.AppEvent;
using IApplicationService.TradeService.Dtos.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobService.JobEventHandler.Trade
{
    public class OrderEventHandler : IEventHandler
    {
        [EventHandlerFunc(EventTopicDictionary.Order.CreateOrderSucc)]
        public async Task<DefaultEventHandlerResponse> CancelOrderJob(EventHandleRequest<OperateOrderSuccDto> input)
        {
            var jobid = BackgroundJob.Schedule<IEventBus>(x => x.SendEvent(EventTopicDictionary.Order.ExpireCancelOrder, input.GetData()), TimeSpan.FromSeconds(60 * 5));
            return await Task.FromResult(DefaultEventHandlerResponse.Default());
        }
    }
}
