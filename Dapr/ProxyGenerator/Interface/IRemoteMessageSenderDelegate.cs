﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ProxyGenerator.Interface
{
    public interface IRemoteMessageSenderDelegate
    {
        object Excute(string hostName, string serviceName, object val, SendType sendType);
    }
}
