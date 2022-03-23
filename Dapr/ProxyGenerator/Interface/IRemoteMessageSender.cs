﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProxyGenerator.Interface
{
    public interface IRemoteMessageSender
    {
        Task<T> SendMessage<T>(string hostName, string serverName, object input, SendType sendType) where T : new();
        Task<object> SendMessage(string hostName, string serverName, object input, SendType sendType, Type type);
    }
}
