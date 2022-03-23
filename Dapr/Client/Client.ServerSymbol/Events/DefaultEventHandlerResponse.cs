﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Client.ServerSymbol.Events
{
    /// <summary>
    /// 默认的事件处理回调类
    /// </summary>
    public class DefaultEventHandlerResponse
    {
        public string status { get => "SUCCESS"; set => status = value; }
        public static DefaultEventHandlerResponse Default()
        {
            return new DefaultEventHandlerResponse();
        }
    }
    /// <summary>
    /// 默认的事件发送回调类
    /// </summary>
    public class DefaultResponse
    {

    }
}
