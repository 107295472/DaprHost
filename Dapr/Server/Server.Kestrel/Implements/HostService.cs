using Autofac;
using Common.Implements;
using Microsoft.Extensions.Hosting;
using ProxyGenerator.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Kestrel.Implements
{
    internal class HostService : IHostedService
    {
        public HostService(ILifetimeScope lifetimeScope)
        {
            IocContainer.BuilderIocContainer(lifetimeScope);
            RemoteProxyGenerator.InitRemoteMessageSenderDelegate();//初始化消息发送代理
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
