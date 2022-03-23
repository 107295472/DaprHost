using Client.ServerProxyFactory.Interface;
using Client.ServerSymbol.Events;
using Common;
using ProxyGenerator.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ServerProxyFactory.Implements
{
    public class EventBus : IEventBus
    {
        private readonly IRemoteMessageSender messageSender;
        public EventBus(IRemoteMessageSender messageSender)
        {
            this.messageSender = messageSender;
        }
        public async Task<DefaultResponse> SendEvent<T>(string topic, T input)
        {
            return await messageSender.SendMessage<DefaultResponse>(DaprConfig.GetCurrent().PubSubCompentName, $"/{topic}", input, SendType.publish);
        }
    }
}