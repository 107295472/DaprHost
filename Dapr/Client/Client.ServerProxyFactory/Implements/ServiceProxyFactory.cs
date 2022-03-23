using Client.ServerProxyFactory.Interface;
using Common.Implements;
using Client.ServerSymbol.Events;
using ProxyGenerator.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Client.ServerProxyFactory.Implements
{
    public class ServiceProxyFactory : IServiceProxyFactory
    {
        public T CreateProxy<T>() where T : class
        {
            return IocContainer.Resolve<T>();
        }
        public T CreateActorProxy<T>() where T : class
        {
            return IocContainer.ResolveNamed<T>($"{typeof(T).FullName}ActorProxy");
        }
    }
}
