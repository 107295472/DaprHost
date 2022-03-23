using System;
using System.Collections.Generic;
using System.Text;

namespace Client.ServerSymbol.Events
{
    /// <summary>
    /// 事件订阅器标记物
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class EventHandlerFuncAttribute : Attribute
    {
        public EventHandlerFuncAttribute(string topic)
        {
            Topic = topic;
        }
        public string Topic { get; }
    }
}
