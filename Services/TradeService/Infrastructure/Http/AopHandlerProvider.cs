using Autofac;
using Common.Implements;
using DomainBase;
using IApplicationService.Base;
using InfrastructureBase;
using InfrastructureBase.Http;
using InfrastructureBase.Object;
using Client.ServerProxyFactory.Interface;
using Client.ServerSymbol.Events;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Http
{
    public class AopHandlerProvider
    {
        public static void ContextHandler(HttpContextWapper HttpContext)
        {
            HttpContextExt.SetCurrent(HttpContext);//注入http上下文给本地业务上下文对象
        }
        public static async Task BeforeSendHandler(object param,HttpContextWapper HttpContext)
        {
            await new TradeAuthenticationHandler().AuthenticationCheck(HttpContextExt.Current.RoutePath);//授权校验
            //方法前拦截器，入参校验
            if (param != null)
                CustomModelValidator.Valid(param);
            HttpContext.HttpContext.Request.Headers.Remove("AuthIgnore");
            HttpContext.HttpContext.Request.Headers.Add("AuthIgnore", "true");
            await Task.CompletedTask;
        }
        public static async Task AfterMethodInvkeHandler(object result)
        {
            await Task.CompletedTask;
        }

        public static async Task<object> ExceptionHandler(Exception exception)
        {
            //异常处理
            if (exception is ApplicationServiceException || exception is DomainException || exception is InfrastructureException)
            {
                return await ApiResult.Err(exception.Message).Async();
            }
            else
            {
                Console.WriteLine("系统异常：" + exception.Message);
                return await ApiResult.Err().Async();
            }
        }
    }
}
