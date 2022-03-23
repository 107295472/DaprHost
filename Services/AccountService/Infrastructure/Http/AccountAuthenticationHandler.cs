using Autofac;
using Client.ServerProxyFactory.Interface;
using Domain.Entities;
using IApplicationService.AccountService.Dtos.Output;
using IApplicationService.Base.AccessToken;
using InfrastructureBase;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Cache;
using InfrastructureBase.Extensions;
using InfrastructureBase.Http;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Http
{
    public class AccountAuthenticationHandler : AuthenticationManager
    {
        public static new void RegisterAllFilter()
        {
            AuthenticationManager.RegisterAllFilter();
        }
        public override async Task AuthenticationCheck(string routePath)
        {
            if (!HttpContextExt.Current.GetAuthIgnore())
            {
                var authMethod = AuthenticationMethods.FirstOrDefault(x => x.Path.Equals(routePath));
                if (authMethod != null)
                {
                    var accountInfo = await GetAccountInfo(HttpContextExt.Current.RequestService.Resolve<IStateManager>());
                    //var casbin= HttpContextExt.Current.RequestService.Resolve<IEnforceService>();
                    HttpContextExt.SetUser(accountInfo);
                    //if (!HttpContextExt.Current.User.IgnorePermission && authMethod.CheckPermission && !HttpContextExt.Current.GetAuthIgnore() && HttpContextExt.Current.User.Permissions != null && !HttpContextExt.Current.User.Permissions.Contains(routePath))
                    //    throw new InfrastructureException("当前登录用户缺少使用该接口的必要权限,请重试!");
                    //var accountInfo = await GetAccountInfo(HttpContextExt.Current.RequestService.Resolve<IStateManager>());
                    //if (App.Casbin.Enforce(accountInfo.LoginName, authMethod.Path))
                    //{
                    //    Console.WriteLine("Power:success");
                    //}
                    //else
                    //{
                    //    throw new InfrastructureException("当前登录用户无权限!");
                    //}
                    //HttpContextExt.SetUser(accountInfo);

                    if (authMethod.CheckPermission && !HttpContextExt.Current.GetAuthIgnore() && !await Valid(accountInfo.Id, authMethod.Path))
                        throw new InfrastructureException("无权限!");
                }
            }
        }
        private async Task<bool> Valid(long id, string path)
        {
            var repo = HttpContextExt.Current.RequestService.Resolve<IFreeSql>();
            var Cache = HttpContextExt.Current.RequestService.Resolve<ICache>();
            var key = string.Format(CacheKey.UserPermissions, id);
            var permissions = await Cache.GetOrSetAsync(key, async () =>
            {
                return await repo.GetRepository<ApiEntity>()
                .Where(a => repo.Select<UserRoleEntity, RolePermissionEntity, PermissionApiEntity>()
                .InnerJoin((b, c, d) => b.RoleId == c.RoleId && b.UserId == id)
                .InnerJoin((b, c, d) => c.PermissionId == d.PermissionId)
                .Where((b, c, d) => d.ApiId == a.Id).Any())
                .ToListAsync<UserPermissionsOutput>();
            });

            var valid = permissions.Any(m =>
                m.Path.NotNull() && m.Path.EqualsIgnoreCase($"{path}"));
            //&& m.HttpMethods.NotNull() && m.HttpMethods.Split(',').Any(n => n.NotNull() && n.EqualsIgnoreCase(routePath))
            //);
            return valid;
        }
        private async Task<CurrentUser> GetAccountInfo(IStateManager stateManager)
        {
            var token = HttpContextExt.Current.HttpContext.Request.Headers.FirstOrDefault(x => x.Key == "authentication").Value;
            if(token=="")
                throw new InfrastructureException("授权登录Token已过期,请重新登录!");
            var usertoken = await stateManager.GetState<AccessTokenItem>(new AccountLoginAccessToken(token));
            if (usertoken == null)
                throw new InfrastructureException("授权登录Token已过期,请重新登录!");
            var userinfo = await stateManager.GetState<CurrentUser>(new AccountLoginCache(usertoken.Id));
            if (userinfo == null)
                throw new InfrastructureException("登录用户信息已过期,请重新登录!");
            //else if (userinfo.State == 1)
            //    throw new InfrastructureException("登录用户已被锁定,请重新登录!");

            //if (!usertoken.LoginAdmin)
            //    userinfo.Permissions = null;
            return userinfo;
        }
    }
}
