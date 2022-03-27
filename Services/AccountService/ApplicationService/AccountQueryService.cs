using ApplicationService.Dtos;
using Domain.Entities;
using Domain.Enums;
using Domain.Repository;
using IApplicationService.AccountService;
using IApplicationService.AccountService.Dtos.Input;
using IApplicationService.AccountService.Dtos.Output;
using IApplicationService.Base.AppQuery;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Http;
using InfrastructureBase.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Infrastructure.Filter;
using IApplicationService.GoodsService;
using Domain.ValueObject;
using IApplicationService.GoodsService.Dtos.Input;
using FreeSql;
using InfrastructureBase.AopFilter;
using InfrastructureBase;
using IApplicationService.Base;
using InfrastructureBase.Cache;
using Mapster;
using InfrastructureBase.Helpers;
using DomainBase.Input;
using DomainBase.Output;
using Client.ServerProxyFactory.Interface;
using DomainBase.Entities;

namespace ApplicationService
{
    public class AccountQueryService : IAccountQueryService
    {
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        private readonly IUserRepository repo;
        private readonly IGoodsQueryService goserver;
        private readonly ICache Cache;
        public AccountQueryService(IUserRepository re, ICache c,IGoodsQueryService server, IEventBus eventBus, IStateManager stateManager)
        {
            this.repo = re;
            this.eventBus = eventBus;
            this.goserver = server;
            this.stateManager = stateManager;
            Cache = c;
        }
        private async Task<IList<UserPermissionsOutput>> GetPermissions(long uid)
        {
            return await repo.Orm.Select<ApiEntity>()
               .Where(a => repo.Orm.Select<UserRoleEntity, RolePermissionEntity, PermissionApiEntity>()
               .InnerJoin((b, c, d) => b.RoleId == c.RoleId && b.UserId == uid)
               .InnerJoin((b, c, d) => c.PermissionId == d.PermissionId)
               .Where((b, c, d) => d.ApiId == a.Id).Any())
               .ToListAsync<UserPermissionsOutput>();
        }
        async Task<List<OrderGoodsSnapshot>> GetGoodsListByIds(IEnumerable<long> input)
        {
            return (await goserver.GetGoodsListByIds(new GetGoodsListByIdsDto(input))).GetData<List<OrderGoodsSnapshot>>();
        }
        [AuthenticationFilter(false)]
        public async Task<ApiResult> GetUserInfo()
        {
            //        var config = new TypeAdapterConfig();
            //        config.NewConfig<CreatePermissionDto, CreatePermissionTmpDto>()
            //.Map(dest => dest.ServerName, src => src.SrvName)
            //.Map(dest => dest.PermissionName, src => src.FuncName);
            var authUserInfoOutput = new AuthUserInfoOutput { };

            long uid = HttpContextExt.Current.User.Id;
            //用户信息
            authUserInfoOutput.User = await repo.GetAsync<AuthUserProfileDto>(uid);
           var  _permissionRepository = repo.Orm.GetRepository<PermissionEntity>();
            //用户菜单
            authUserInfoOutput.Menus = await _permissionRepository.Select
                .Where(a => new[] { PermissionType.Group, PermissionType.Menu }.Contains(a.Type))
                .Where(a =>
                    _permissionRepository.Orm.Select<RolePermissionEntity>()
                    .InnerJoin<UserRoleEntity>((b, c) => b.RoleId == c.RoleId && c.UserId == uid)
                    .Where(b => b.PermissionId == a.Id)
                    .Any()
                )
                .OrderBy(a => a.ParentId)
                .OrderBy(a => a.Sort)
                .ToListAsync(a => new AuthUserMenuDto { ViewPath = a.View.Path });

            //用户权限点
            authUserInfoOutput.Permissions = await _permissionRepository.Select
                .Where(a => a.Type == PermissionType.Dot)
                .Where(a =>
                    _permissionRepository.Orm.Select<RolePermissionEntity>()
                    .InnerJoin<UserRoleEntity>((b, c) => b.RoleId == c.RoleId && c.UserId == uid)
                    .Where(b => b.PermissionId == a.Id)
                    .Any()
                )
                .ToListAsync(a => a.Code);
            return  ApiResult.Ok(authUserInfoOutput);
        }
        [TestMethodFilter]
        public async Task<ApiResult> CheckRoleBasedAccessControler()
        {
            if (await stateManager.GetState<bool>(new RoleBaseInitCheckCache()))
            {
                var oauth = await stateManager.GetState<InitUserOauthDto.Github>(new OauthStateStore());
                return ApiResult.Ok(new DefLoginAccountResponse { LoginName = oauth?.login ?? "eshopadmin", Password = "x1234567" });
            }
            else
                return ApiResult.Ok(false);
        }

        [AuthenticationFilter]
        public async Task<ApiResult> GetAccountList(PageQueryInputBase input)
        {
            var Data = await repo.Orm.Select<Account, User>()
                .LeftJoin((a, b) => a.Id == b.Id).Count(out var Total).Page(input.Page, input.Limit).ToListAsync<GetAccountListResponse>();
            var accoundIds = Data.Select(x => x.Id).ToList();

            var roles = await repo.Orm.Select<Role, UserRole>()
                .LeftJoin((a, b) => a.Id == b.RoleId).Where((a, b) => accoundIds.Contains(b.UserId))
                .ToListAsync((a,b)=>new { b.UserId, a.Id, a.Name});
            Data.ForEach(account =>
            {
                account.Roles = roles.Where(role => role.UserId == account.Id).Select(role => new GetAccountListResponse.RoleItem() { RoleId = role.Id, RoleName = role.Name});
            });
            return ApiResult.Ok(new PageQueryResonseBase<GetAccountListResponse>(Data, Total));
        }

        [AuthenticationFilter]
        public async Task<ApiResult> GetAccountUserNameByIds(GetAccountUserNameByIdsDto input)
        {
            var query = repo.Orm.Select<User>().Where(x => input.Ids.Contains(x.Id)).ToListAsync(x => new GetAccountUserNameByIdsResponse { AccountId = x.Id, Name = x.UserName });
            return await ApiResult.Ok(query).Async();
        }
        [AuthenticationFilter(false)]
        public async Task<ApiResult> GetPage(PageInput<UserEntity> input)
        {
            var list = await repo.Orm.GetRepository<UserEntity>().Select
            .WhereDynamicFilter(input.DynamicFilter)
            .Count(out var total)
            .OrderByDescending(true, a => a.Id)
            .IncludeMany(a => a.Roles.Select(b => new RoleEntity { Name = b.Name }))
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync();

            var data = new PageOutput<UserListOutput>()
            {
                List =list.Adapt<List<UserListOutput>>(),
                Total = total
            };

            return ApiResult.Ok(data);
        }
        public async Task<IList<UserPermissionsOutput>> GetPermissionsAsync()
        {
            var key = string.Format(CacheKey.UserPermissions,HttpContextExt.Current.User.Id);
            var result = await Cache.GetOrSetAsync(key, async () =>
            {
                return await repo.Orm.GetRepository<ApiEntity>()
                .Where(a => repo.Orm.Select<UserRoleEntity, RolePermissionEntity, PermissionApiEntity>()
                .InnerJoin((b, c, d) => b.RoleId == c.RoleId && b.UserId ==HttpContextExt.Current.User.Id)
                .InnerJoin((b, c, d) => c.PermissionId == d.PermissionId)
                .Where((b, c, d) => d.ApiId == a.Id).Any())
                .ToListAsync<UserPermissionsOutput>();
            });
            return result;
        }
        public async Task<ApiResult> GetMockAccount()
        {
            var result = await repo.GetAsync(x => x.UserName == "user");

            return ApiResult.Ok(result);
        }
        public async Task<ApiResult> SlbTest()
        {
            var ip = IPHelper.GetIP(HttpContextExt.Current.HttpContext.Request);
            return await ApiResult.Ok(ip).Async();
        }
    }
}
