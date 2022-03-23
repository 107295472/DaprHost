﻿using Autofac;
using Autofac.Core;
using Autofac.Core.Lifetime;
using Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Common.Implements.InProcessEventBus
{
    public abstract class InProcessPiplineBase
    {
        public virtual Channel<object> Pipline { get; set; }
        public virtual Func<object, ILifetimeScope, Task> EventHandler { get; set; }
    }

    public class InProcessPipline<T> : InProcessPiplineBase
    {
        public InProcessPipline() { }
        private readonly ILogger logger;
        private static ISharingLifetimeScope Container;
        public InProcessPipline(ILifetimeScope lifetimeScope, Func<T, ILifetimeScope, Task> eventHandler)
        {
            Pipline = Channel.CreateUnbounded<T>();
            EventHandler = eventHandler;
            Container = ((LifetimeScope)lifetimeScope).RootLifetimeScope;
            logger = lifetimeScope.Resolve<ILogger>();
            _ = SubscribeHandleInvoke();
        }
        public async Task SubscribeHandleInvoke()
        {
            await foreach (var message in Pipline.Reader.ReadAllAsync())
            {
                try
                {
                    using var lifescope = Container.BeginLifetimeScope();
                    await EventHandler(message, lifescope);
                }
                catch (Exception e)
                {
                    logger.LogError($"进程内订阅器消息处理异常：{e.Message}，调用堆栈：{e.StackTrace}");
                }
            }
        }
        public new Channel<T> Pipline { get; set; }
        public new Func<T, ILifetimeScope, Task> EventHandler { get; set; }
    }
}
