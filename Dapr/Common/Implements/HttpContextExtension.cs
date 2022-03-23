using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Implements
{
    public static class HttpContextExtension
    {
        public static AsyncLocal<HttpContextWapper> ContextWapper = new AsyncLocal<HttpContextWapper>();
    }
}
