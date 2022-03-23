using Domain.Specification;
using IApplicationService.AccountService;
using IApplicationService.AccountService.Dtos.Input;
using InfrastructureBase;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Repository;
using ApplicationService.Dtos;
using IApplicationService.AccountService.Dtos.Output;
using IApplicationService.Base.AccessToken;
using IApplicationService.AccountService.Dtos.Event;
using Infrastructure.Filter;
using InfrastructureBase.AopFilter;
using FreeSql;
using Domain.Entities;
using Mapster;
using InfrastructureBase.Helpers;
using IApplicationService.Base;
using IApplicationService.Base.AppEvent;
using IApplicationService.GoodsService.Dtos.Event;
using Domain.Events;
using Client.ServerProxyFactory.Interface;

namespace ApplicationService
{
    public class AccountUseCaseService : IAccountUseCaseService
    {
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        private readonly IUserRepository repo;
        public AccountUseCaseService(IUserRepository sql, IEventBus eventBus, IStateManager stateManager)
        {
            this.repo = sql;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
         }
        [TestMethodFilter]
        public async Task<ApiResult> InitRoleBasedAccessControler(InitUserOauthDto input)
        {
            if (!await stateManager.GetState<bool>(new RoleBaseInitCheckCache()))
            {
                await stateManager.SetState(new RoleBaseInitCheckCache(true));
                await eventBus.SendEvent(EventTopicDictionary.Account.InitTestUserSuccess, new LoginSuccessDto() { Token = input.OauthData ?? "" });
                if (!string.IsNullOrEmpty(input.OauthData))
                {
                    var data = System.Text.Json.JsonSerializer.Deserialize<InitUserOauthDto.Github>(input.OauthData);
                    await stateManager.SetState(new OauthStateStore(data));
                    return ApiResult.Ok(new DefLoginAccountResponse { LoginName = data.login, Password = "x1234567" }, $"权限初始化成功,已创建超管角色和默认登录账号");
                }
            }
            var oauth = await stateManager.GetState<InitUserOauthDto.Github>(new OauthStateStore());
            return ApiResult.Ok(new DefLoginAccountResponse { LoginName = oauth?.login ?? "eshopadmin", Password = "x1234567" }, $"权限初始化成功,已创建超管角色和默认登录账号");
        }
        [Tran]
        public async Task<ApiResult> AccountRegister(CreateAccountDto input)
        {
            //var account = new Account();
            //account.CreateAccount(input.LoginName, input.NickName, input.Password, Common.GetMD5SaltCode);

            //await new UniqueAccountIdSpecification(repo).IsSatisfiedBy(account);//验证
            //repo.Insert(account);
            //if (account.Roles.Any())
            //    repo.Orm.GetRepository<UserRole>().Insert(account.Roles.Select(x => new UserRole() { AccountId = account.Id, RoleId = x }));
            //var user = new User();
            //user.AccountId = account.Id;
            //user.Gender = UserGender.Male;
            //repo.Orm.GetRepository<User>().Insert(user);
            //await eventBus.SendEvent(EventTopicDictionary.Account.ATest, "*******注册成功*****");
            //写日志
            //var logdb = Mongo.GetRepository<EventLog_1>(nameof(EventLog_1));
            //await logdb.AddAsync(new EventLog_1 { UserID = user.Id,AccountID=account.Id,Remark = "注册成功" });
            return  ApiResult.Ok("用户注册成功");
        }
        [Tran]
        public async Task<ApiResult> AccountCreate(UserAddInput input)
        {
            var user =input.Adapt<UserEntity>();
            user.Password = BaseCommon.GetMD5SaltCode(user.Password, new object[] { user.Id });
            await new UniqueAccountIdSpecification(repo).IsSatisfiedBy(user);

            return await repo.Add(user);
            //return ApiResult.Ok("用户创建成功");
        }
        [AuthenticationFilter]
        public async Task<ApiResult> AccountUpdate(UpdateAccountDto input)
        {
            //using var tran = await unitofWork.BeginTransactionAsync();
            //var account = await accountRepository.GetAsync(input.ID);
            //if (account == null)
            //    throw new ApplicationServiceException("所选用户不存在!");
            //account.UpdateNicknameOrPassword(input.NickName, input.Password);
            //account.SetRoles(input.Roles);
            //account.User.CreateOrUpdateUser(input.User?.UserName, "", input.User?.Address, input.User?.Tel, input.User?.Gender == null ? UserGender.Unknown : (UserGender)input.User?.Gender, input.User?.BirthDay);
            ////accountRepository.Update(account);
            //if (await new RoleValidityCheckSpecification(roleRepository).IsSatisfiedBy(account))
            //    await unitofWork.CommitAsync(tran);
            //await BuildLoginCache(account);
            return ApiResult.Ok("用户信息更新成功");
        }
        [AuthenticationFilter]
        public async Task<ApiResult> AccountDelete(AccountDeleteDto input)
        {
            //var account = await accountRepository.GetAsync(input.AccountId);
            //if (account == null)
            //    throw new ApplicationServiceException("所选用户不存在!");
            ////accountRepository.Delete(account);
            //if (await new AccountDeleteCheckSpecification(HttpContextExt.Current.User.Id).IsSatisfiedBy(account))
            //    await unitofWork.CommitAsync();
            //await stateManager.DelState(new AccountLoginCache(account.Id));
            return ApiResult.Ok("用户信息删除成功");
        }
        public async Task<ApiResult> AccountLogin(AuthLoginInput input)
        {

            var ip = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces().Select(p => p.GetIPProperties()).SelectMany(p => p.UnicastAddresses)
                   .Where(p => p.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !System.Net.IPAddress.IsLoopback(p.Address))
                   .FirstOrDefault()?.Address.ToString();
            return ApiResult.Ok($"主机---{ip}");
            var account = await repo.Select.Where(x => x.UserName == input.UserName).FirstAsync();
          
            if (account == null)
                throw new ApplicationServiceException("登录账号不存在!");
            if (MD5Encrypt.Encrypt32(input.Password) != account.Password)
            {
                throw new ApplicationServiceException("密码错误!");
            }
            await BuildLoginCache(account);
            var loginToken = BaseCommon.GetMD5SaltCode(Guid.NewGuid().ToString());
            await stateManager.SetState(new AccountLoginAccessToken(loginToken, new AccessTokenItem(account.Id)));
            await eventBus.SendEvent(EventTopicDictionary.Account.LoginSucc, new LoginAccountSuccessEvent(loginToken));

            return ApiResult.Ok(new AccountLoginResponse(loginToken));

            //var account = await repo.Select.Where(x => x.UserName == input.UserName).FirstAsync();
            //if (account == null)
            //    throw new ApplicationServiceException("登录账号不存在!");
            ////account.User = await repo.Orm.Select<User>().Where(x => x.AccountId == account.Id).FirstAsync() ?? new User();
            //if (MD5Encrypt.Encrypt32(input.Password) != account.Password){
            //    throw new ApplicationServiceException("密码错误!");
            //}
            //await BuildLoginCache(account);
            //var loginToken = Common.GetMD5SaltCode(Guid.NewGuid().ToString());
            //await stateManager.SetState(new AccountLoginAccessToken(loginToken, new AccessTokenItem(account.Id)));
            //await eventBus.SendEvent(EventTopicDictionary.Account.LoginSucc, new LoginAccountSuccessEvent(loginToken));
        }
        public async Task<ApiResult> AccountLoginOut()
        {
            if (HttpContextExt.Current.User == null)
                throw new ApplicationServiceException("登录用户不存在!");
            await stateManager.DelState(new AccountLoginAccessToken(HttpContextExt.Current.User.Id.ToString()));
            return ApiResult.Ok("用户登出成功");
        }
        /// <summary>
        /// 用户分配角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Tran]
        public async Task<ApiResult> AssignRole(AssignRoleDto input)
        {
            var accountRole = new List<UserRole>();
            for (int i = 0; i < input.RoleID.Count; i++)
            {
                accountRole.Add(new UserRole() {
                UserId= input.AccountID,
                RoleId= input.RoleID[i]
                });
            }
            repo.Orm.Delete<UserRole>().Where(x =>x.UserId == input.AccountID).ExecuteAffrows();
            await repo.Orm.GetRepository<UserRole>().InsertAsync(accountRole);
            return ApiResult.Ok();
        }
        [AuthenticationFilter]
        public async Task<ApiResult> SupplementaryAccountInfo(SupplementaryUserDto input)
        {
            //var account = await accountRepository.GetAsync(HttpContextExt.Current.User.Id);
            //if (account == null)
            //    throw new ApplicationServiceException("登录用户不存在!");
            //account.User.CreateOrUpdateUser(input.UserName, input.UserImage, input.Address, input.Tel, (UserGender)input.Gender, input.BirthDay);
            ////accountRepository.Update(account);
            //await unitofWork.CommitAsync();
            //await BuildLoginCache(account);
            return ApiResult.Ok("用户信息完善成功");
        }
        [AuthenticationFilter]
        public async Task<ApiResult> LockOrUnLockAccount(LockOrUnLockAccountDto input)
        {
            //var account = await accountRepository.GetAsync(input.ID);
            //if (account == null)
            //    throw new ApplicationServiceException("所选用户不存在!");
            //account.ChangeAccountLockState(HttpContextExt.Current.User.Id);
            ////accountRepository.Update(account);
            //await unitofWork.CommitAsync();
            //await BuildLoginCache(account);
            return ApiResult.Ok("用户锁定/解锁成功");
        }
        private async Task BuildLoginCache(UserEntity account)
        {
            var per = await repo.GetPermissionsAsync(account.Id);
            await stateManager.SetState(new AccountLoginCache(account.Id, new CurrentUser(account.Id,account.UserName,account.NickName,per,account.TenantId)));
        }
        private async Task<List<string>> GetAccountPermissions(long id)
        {
            var roles = await repo.Orm.Select<Role, UserRole>()
               .LeftJoin((a, b) => a.Id == b.RoleId).Where((a,b)=>b.UserId == id).ToListAsync();
            //if (roles.Any(x => x.SuperAdmin))
            //    return null;
            var roleids = roles.Select(y => y.Id);
            return await repo.Orm.Select<RolePermission, Permission>()
                .LeftJoin((a, b) => a.PermissionId == b.Id).Where((a, b) => roleids.Contains(a.RoleId)).ToListAsync((a, b) => b.Path);
        }
    }
}
