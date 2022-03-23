using Common.Interface;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Kestrel.Interface
{
    public interface IServerHandler
    {
        void BuildHandler(IApplicationBuilder app, ISerialize serialize);
    }
}
