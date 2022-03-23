using Client.ServerSymbol.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ServerProxyFactory.Interface
{
    public interface IEventBus
    {
        Task<DefaultResponse> SendEvent<T>(string topic, T input);
    }
}
