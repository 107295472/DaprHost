﻿using Client.ServerSymbol;
using Common.Implements;
using ProxyGenerator.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProxyGenerator.Implements
{
    public class RemoteDispatchProxy<T> : RemoteDispatchProxyBase
    {
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            var router = RemoteRouters.FirstOrDefault(x => x.Key.Equals(targetMethod.Name));
            if (router != null)
            {
                if (args.Any())
                    return router.SenderDelegate.Excute(router.HostName, router.RouterName, args[0], router.SendType);
                else
                    return router.SenderDelegate.Excute(router.HostName, router.RouterName, null, router.SendType);
            }
            else
            {
                return null;
            }
        }
    }
    public abstract class RemoteDispatchProxyBase : DispatchProxy
    {
        internal void InitRemoteRouters(Type interfaceType, string hostName, string routerName, IEnumerable<MethodInfo> remoteMethods)
        {
            RemoteRouters = new List<RemoteRouter>();
            remoteMethods.ToList().ForEach(x =>
            {
                if (x.ReturnParameter.ParameterType.GenericTypeArguments[0] == typeof(string))
                    throw new Exception($"由于string类型不包含无参构造函数,无法为返回类型为Task<string>的方法创建代理,请改用Task<dynamic>,接口:{x.DeclaringType.Name},方法名:{x.Name}");
                var funcAttr = ReflectionHelper.GetAttributeProperyiesByMethodInfo<RemoteFuncAttribute>(x);
                //生成服务调用代理
                if (funcAttr.FuncType == FuncType.Actor || funcAttr.FuncType == FuncType.Invoke)
                {
                    RemoteRouters.Add(new RemoteRouter()
                    {
                        Key = x.Name,
                        HostName = funcAttr.FuncType == FuncType.Actor ? $"{interfaceType.Name}ActorImpl" : hostName,
                        RouterName = funcAttr.FuncType == FuncType.Actor ? $"/{x.Name}" : $"/{routerName}/{x.Name}".ToLower(),
                        InputType = x.GetParameters().FirstOrDefault()?.ParameterType,
                        SendType = funcAttr.FuncType == FuncType.Invoke ? SendType.invoke : funcAttr.FuncType == FuncType.Actor ? SendType.actors : SendType.invoke,
                        MethodInfo = typeof(IRemoteMessageSender).GetMethod("SendMessage", new Type[] { typeof(string), typeof(string), typeof(object), typeof(SendType) }).MakeGenericMethod(x.ReturnParameter.ParameterType.GenericTypeArguments[0]),
                    });
                }
            });
        }
        internal void InitSenderDelegate()
        {
            RemoteRouters.ForEach(x => x.SenderDelegate = BuildSenderDelegate(x.MethodInfo, x.InputType));
        }
        protected class RemoteRouter
        {
            internal string Key { get; set; }
            internal string HostName { get; set; }
            internal string RouterName { get; set; }
            internal Type InputType { get; set; }
            internal MethodInfo MethodInfo { get; set; }
            internal SendType SendType { get; set; }
            internal IRemoteMessageSenderDelegate SenderDelegate { get; set; }
        }
        protected List<RemoteRouter> RemoteRouters { get; set; }

        IRemoteMessageSenderDelegate BuildSenderDelegate(MethodInfo methodInfo, Type inputType)
        {
            return (IRemoteMessageSenderDelegate)Activator.CreateInstance(typeof(RemoteMessageSenderDelegate<,>).MakeGenericType(inputType ?? typeof(object), methodInfo.ReturnType), methodInfo, IocContainer.Resolve<IRemoteMessageSender>());
        }
    }
}
