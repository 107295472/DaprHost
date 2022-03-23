﻿using Autofac;
using Client.ServerSymbol;
using Client.ServerSymbol.Events;
using Common.Implements;
using ProxyGenerator.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace ProxyGenerator.Implements
{
    public class RemoteProxyGenerator
    {
        static Type ThisType = typeof(RemoteProxyGenerator);
        static object ThisObj = Activator.CreateInstance(ThisType);
        static List<RemoteDispatchProxyBase> Proxies = new List<RemoteDispatchProxyBase>();
        /// <summary>
        /// 注册所有的远程服务代理
        /// </summary>
        public static void CreateRemoteProxyAndRegisterInIocContainer(ContainerBuilder builder)
        {
            //获取所有标记为remote的servie
            var remoteservice = ReflectionHelper.GetTypesByAttributes(true, typeof(RemoteServiceAttribute));
            //获取所有标记为remote的method构造具体的proxy
            remoteservice.ToList().ForEach(x =>
            {
                var attr = ReflectionHelper.GetAttributeProperyiesByType<RemoteServiceAttribute>(x);

                //if(ReflectionHelper.GetTypeByInterface(x) != null)
                //{
                //    Console.WriteLine(attr.ServerName);
                //}
                var proxyInstance = ThisType.GetMethod(nameof(CreateProxyInstance)).MakeGenericMethod(x).Invoke(ThisObj, null) as RemoteDispatchProxyBase;
                if (ReflectionHelper.GetTypeByInterface(x) == null || x.GetInterface(nameof(IActorService)) != null)
                {
                    //Console.WriteLine("ref:xxxxxxxxxxxxxxxxxx");
                    if (x.GetInterface(nameof(IActorService)) != null)
                        builder.RegisterInstance(proxyInstance).As(x).Named($"{x.FullName}ActorProxy", x);
                    else
                        builder.RegisterInstance(proxyInstance).As(x);
                    proxyInstance.InitRemoteRouters(x, attr.HostName, attr.ServerName ?? x.Name, ReflectionHelper.GetMethodByFilter(x, typeof(RemoteFuncAttribute)));
                    Proxies.Add(proxyInstance);

                }
            });
        }
        public static void InitRemoteMessageSenderDelegate()
        {
            Proxies.ForEach(x => x.InitSenderDelegate());
        }
        public T CreateProxyInstance<T>()
        {
            return DispatchProxy.Create<T, RemoteDispatchProxy<T>>();
        }
    }
}
