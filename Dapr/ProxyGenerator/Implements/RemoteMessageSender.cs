using Client.ServerSymbol.Actors;
using Common;
using Common.Implements;
using Common.Interface;
using Microsoft.Extensions.Configuration;
using Client.ServerSymbol;
using Client.ServerSymbol.Events;
using ProxyGenerator.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProxyGenerator.Implements
{
    internal class RemoteMessageSender : IRemoteMessageSender
    {
        private readonly ISerialize serialize;
        private readonly ILogger logger;
        private static string daprPort = "";
        private static string basepath = "";
        static Lazy<HttpClient> HttpClient = new Lazy<HttpClient>(() =>
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Connection.Add("keep-alive");
            return client;
        });
        public RemoteMessageSender(ISerialize serialize, ILogger logger)
        {
            this.serialize = serialize;
            this.logger = logger;
        }
        //检测dapr sidecar存活
        static bool Readyless = false;
        async Task<bool> ReadylessCheck()
        {

            //basepath += $"{daprPort}/";
            daprPort = Environment.GetEnvironmentVariable("DAPR_HTTP_PORT");
            if (daprPort == "")
            {
                logger.LogWarn($"请指定Dapr端口");
                return false;
            }
            basepath = $"http://localhost:{daprPort}/";
            //#if DEBUG
            //            basepath = $"http://localhost:{daprPort}/";
            //#endif
            //            basepath = $"{ Environment.GetEnvironmentVariable("DOCKER_BASEPATH")}:{daprPort}";
            //Console.WriteLine(daprPort+"****************");
            int reTry = 0;
            //进程内dapr边车端口
            if (!Readyless)
            {
                while (reTry < 2)
                {
                    try
                    {
                        if (!Readyless)
                            Readyless = (await HttpClient.Value.GetAsync($"{basepath}v1.0/healthz")).IsSuccessStatusCode;
                        if (Readyless)
                        {
                            //Console.WriteLine("*****心跳成功*******");
                            break;
                        }
                        else
                        {
                            logger.LogWarn($"Dapr尚未准备就绪!");
                            await Task.Delay(100);
                        }
                    }
                    catch (Exception e)
                    {
                        logger.LogWarn($"Dapr尚未准备就绪,{e.Message}");
                        break;
                    }
                    finally { Interlocked.Increment(ref reTry); }
                }
            }
            return Readyless;
        }
        public async Task<T> SendMessage<T>(string hostName, string serverName, object input, SendType sendType) where T : new()
        {
            T result = default;
            //logger.LogInfo($"host:{hostName},server:{serverName},sendType:{Convert.ToInt32(sendType)}");
            if (await ReadylessCheck())
            {
                try
                {
                    var sendMessage = BuildMessage(hostName, serverName, input, sendType);
                    var responseMessage = await HttpClient.Value.SendAsync(sendMessage);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        if (sendType == SendType.publish || sendType == SendType.setState || sendType == SendType.delState)
                            return new T();//事件和状态操作只要返回200代表发送成功
                        return ReceiveMessage<T>(sendType, await responseMessage.Content.ReadAsByteArrayAsync());
                    }
                    else
                    {
                        logger.LogError($"客户端调用http请求异常,状态码：{responseMessage?.StatusCode},请求内容:{sendMessage}，回调内容:{await responseMessage.Content.ReadAsStringAsync()}");
                    }
                }
                catch (Exception e)
                {
                    logger.LogError($"客户端调用异常：{e.Message},接口地址：{serverName},调用堆栈{e.StackTrace}");
                }
            }
            return result;
        }
        internal HttpRequestMessage BuildMessage(string host, string url, object data, SendType sendType)
        {
            //            basepath += $"{daprPort}/";
            //#if DEBUG
            //            daprPort = Environment.GetEnvironmentVariable("DAPR_HTTP_PORT", EnvironmentVariableTarget.Process);
            //            basepath = $"http://localhost:{daprPort}";
            //#endif  //集群内地址：localhost:3500
            //todo: 由于event和actor会被dapr拦截使用Text.Json进行序列化封装，导致无法使用messagepack序列/反序列化,所以暂时只能采用json
            //进程内dapr边车端口
            //var port = Environment.GetEnvironmentVariable("DAPR_HTTP_PORT", EnvironmentVariableTarget.Process);
            //logger.LogInfo("port11:" + port);

            HttpRequestMessage request;
            switch (sendType)
            {
                case SendType.invoke:
                    url = $"{basepath}v1.0/invoke/{host}/method{url}";
                    request = new HttpRequestMessage(HttpMethod.Post, url) { Version = new Version(1, 1) };
                    var bytedata = serialize.Serializes(data ?? new byte[0]);
                    request.Content = new ByteArrayContent(bytedata);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue($"application/x-msgpack");
                    AddTraceHeader(request);
                    return request;
                case SendType.publish:
                    url = $"{basepath}v1.0/publish/{host}{url}";
                    request = new HttpRequestMessage(HttpMethod.Post, url) { Version = new Version(1, 1) };
                    var stringjson = serialize.SerializesJson(data);
                    request.Content = new StringContent(stringjson);
                    return request;
                case SendType.actors:
                    url = $"{basepath}v1.0/actors/{host}/{((ActorSendDto)data).ActorId}/method{url}";
                    request = new HttpRequestMessage(HttpMethod.Post, url) { Version = new Version(1, 1) };
                    stringjson = serialize.SerializesJson(data, true);//actor json原样发送
                    request.Content = new StringContent(stringjson);
                    AddTraceHeader(request);
                    return request;
                case SendType.setState:
                    url = $"{basepath}v1.0/state/{host}";
                    request = new HttpRequestMessage(HttpMethod.Post, url) { Version = new Version(1, 1) };
                    stringjson = serialize.SerializesJson(data);
                    request.Content = new StringContent(stringjson);
                    return request;
                case SendType.getState:
                    url = $"{basepath}v1.0/state/{host}{url}";
                    request = new HttpRequestMessage(HttpMethod.Get, url) { Version = new Version(1, 1) };
                    return request;
                case SendType.delState:
                    url = $"{basepath}v1.0/state/{host}{url}";
                    request = new HttpRequestMessage(HttpMethod.Delete, url) { Version = new Version(1, 1) };
                    return request;
            }
            return default;
        }
        internal object ReceiveMessage(SendType sendType, byte[] data, Type type)
        {
            if (sendType == SendType.invoke)
                return serialize.Deserializes(type, data) ?? null;
            else
                return serialize.DeserializesJson(type, Encoding.UTF8.GetString(data));
        }
        internal T ReceiveMessage<T>(SendType sendType, byte[] data) where T : new()
        {
            if (sendType == SendType.invoke)
                return serialize.Deserializes<T>(data) ?? new T();
            else
                return serialize.DeserializesJson<T>(Encoding.UTF8.GetString(data));
        }
        internal void AddTraceHeader(HttpRequestMessage httpRequest)
        {
            if (!string.IsNullOrEmpty(DaprConfig.GetCurrent().TracingHeaders))
            {
                foreach (var headername in DaprConfig.GetCurrent().TracingHeaders.Split(","))
                {
                    foreach (var currentHeader in HttpContextExtension.ContextWapper.Value.HttpContext.Request.Headers)
                    {
                        if (currentHeader.Key == headername)
                            httpRequest.Headers.Add(headername, currentHeader.Value.ToArray());
                    }
                }
            }
        }
        public async Task<object> SendMessage(string hostName, string serverName, object input, SendType sendType, Type type)
        {
            object result = default;
            if (await ReadylessCheck())
            {
                try
                {
                    var sendMessage = BuildMessage(hostName, serverName, input, sendType);
                    var responseMessage = await HttpClient.Value.SendAsync(sendMessage);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        if (sendType == SendType.publish || sendType == SendType.setState || sendType == SendType.delState)
                            return new object();//事件和状态操作只要返回200代表发送成功
                        return ReceiveMessage(sendType, await responseMessage.Content.ReadAsByteArrayAsync(), type);
                    }
                    else
                    {
                        logger.LogError($"客户端调用http请求异常,状态码：{responseMessage?.StatusCode},请求内容:{sendMessage}，回调内容:{await responseMessage.Content.ReadAsStringAsync()}");
                    }
                }
                catch (Exception e)
                {
                    logger.LogError($"客户端调用异常：{e.Message},接口地址：{serverName},调用堆栈{e.StackTrace}");
                }
            }
            return result;
        }
    }
}
