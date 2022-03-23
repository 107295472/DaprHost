using Autofac;
using Client.ServerProxyFactory.Interface;
using Client.ServerSymbol.Events;
using DomainBase;
using IApplicationService.Base.AppEvent;
using IApplicationService.PublicService.Dtos.Event;
using IApplicationService.TradeService.Dtos.Event;
using InfrastructureBase.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureBase.Object
{
    public static class DefaultEventHandlerResponseExtension
    {
        public static async Task<DefaultEventHandlerResponse> RunAsync(this DefaultEventHandlerResponse handleResult, string handleName, string eventJson, Func<Task> invokeAsync)
        {
            try
            {
                await invokeAsync();
            }
            catch (Exception e)
            {
                await HttpContextExt.Current.RequestService.Resolve<IEventBus>().SendEvent(EventTopicDictionary.Common.EventHandleErrCatch,
                    new EventHandlerErrDto(handleName, eventJson, e.Message, e.StackTrace, !(e is ApplicationServiceException || e is DomainException || e is InfrastructureException)));
            }
            return DefaultEventHandlerResponse.Default();
        }
    }
}
