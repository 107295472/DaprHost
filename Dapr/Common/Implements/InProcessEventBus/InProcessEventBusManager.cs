﻿using Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Implements.InProcessEventBus
{
    public class InProcessEventBusManager : IInProcessEventBus
    {

        public async Task SendEvent<T>(string topic, T input)
        {
            var channel = InProcessPiplineFactory.Get<T>(topic);
            if (channel != default)
            {
                await channel.Pipline.Writer.WriteAsync(input);
            }
        }
    }
}
