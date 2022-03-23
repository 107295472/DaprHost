﻿using Autofac;
using Common.Implements;
using Common.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Client.ServerSymbol;
using ProxyGenerator.Implements;
using Server.Kestrel.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Unicode;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Kestrel.Implements
{
    internal class RequestDelegate<Tobj, Tin, Tout> : BaseRequestDelegate where Tin : class, new() where Tout : class
    {
        private readonly ILogger logger;
        private readonly IMessageHandler messageHandler;
        private readonly bool noInput;
        public RequestDelegate(string serverName, MethodInfo method, ILogger logger, IMessageHandler messageHandler)
        {
            Path = new PathString($"/{serverName}/{method.Name}".ToLower());
            if (typeof(Tin) == typeof(object))
            {
                noInput = true;
                NoInputMethodDelegate = RequestDelegateFactory.CreateMethodDelegate<Tobj, Task<Tout>>(method);
            }
            else
            {
                noInput = false;
                MethodDelegate = RequestDelegateFactory.CreateMethodDelegate<Tobj, Tin, Task<Tout>>(method);
            }
            this.logger = logger;
            this.messageHandler = messageHandler;
        }
        internal Func<Tobj, Tin, Task<Tout>> MethodDelegate { get; set; }
        internal Func<Tobj, Task<Tout>> NoInputMethodDelegate { get; set; }
        internal override async Task Excute(HttpContext ctx, ILifetimeScope scope)
        {
            IocContainer.BuilderIocContainer(scope);//仅在当前请求内创建上下文模型
            byte[] result = new byte[0];
            ctx.Response.ContentType = "application/json";
            var messageType = MessageType.Json;
            try
            {
                if (ctx.Request.ContentType == "application/x-msgpack")
                {
                    ctx.Response.ContentType = "application/x-msgpack";
                    messageType = MessageType.MessagePack;
                }
                //if (ctx.Request.ContentType == null)
                //{
                //    ctx.Response.ContentType = "text/html";
                //    messageType = MessageType.Html;
                //}
                HttpContextExtension.ContextWapper.Value = new HttpContextWapper(Path, scope, ctx);
                Tout localCallbackResult = null;
                if (noInput)
                {
                    localCallbackResult = await LocalMethodAopProvider.UsePipelineHandler(scope, HttpContextExtension.ContextWapper.Value, NoInputMethodDelegate);
                }
                else
                {
                    var messageobj = await messageHandler.ParseMessage<Tin>(ctx, messageType);
                    if (messageobj == default(Tin))
                        throw new FormatException($"参数反序列化失败,接口地址{Path},入参类型：{typeof(Tin).Name}");
                    localCallbackResult = await LocalMethodAopProvider.UsePipelineHandler(scope, messageobj, HttpContextExtension.ContextWapper.Value, MethodDelegate);
                }
                if (localCallbackResult != null)
                {
                    result = messageHandler.BuildMessage(localCallbackResult, messageType);
                }
            }
            catch (Exception e)
            {
                logger.LogError($"服务端消息处理异常: {e.GetBaseException()?.Message ?? e.Message}");
                ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                if (e is FormatException)
                    await ctx.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(e.Message));
            }
            finally
            {
                await ctx.Response.Body.WriteAsync(result, 0, result.Length);
                IocContainer.DisposeIocContainer();//注销上下文
            }
        }
    }
}
