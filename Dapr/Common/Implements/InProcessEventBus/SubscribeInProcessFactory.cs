using Autofac;
using Common.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Implements.InProcessEventBus
{
    public class SubscribeInProcessFactory : ISubscribeInProcessFactory
    {
        public bool RegisterEventHandler<T>(string topic, ILifetimeScope lifetimeScope, Func<T, ILifetimeScope, Task> eventHandler)
        {
            return InProcessPiplineFactory.Add(topic, lifetimeScope, eventHandler);
        }
    }
}
