using Domain.Entities;
using Domain.Enums;
using Domain.Repository;
using FreeSql;
using IApplicationService.AccountService;
using IApplicationService.AccountService.Dtos;
using IApplicationService.AccountService.Dtos.Input;
using IApplicationService.AccountService.Dtos.Output;
using IApplicationService.Base;
using IApplicationService.Base.AppQuery;
using Infrastructure;
using Mapster;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Data;
using InfrastructureBase.Http;
using InfrastructureBase.Object;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using InfrastructureBase.Extensions;
using Client.ServerProxyFactory.Interface;
using DomainBase.Entities;

namespace ApplicationService
{
    public class PermissionQueryService : IPermissionQueryService
    {
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        private readonly IRepositoryBase<PermissionEntity> repo;
        public PermissionQueryService(IRepositoryBase<PermissionEntity> sql, IEventBus eventBus, IStateManager stateManager)
        {
            this.repo = sql;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }

        public async Task<ApiResult> GetList(PermissionApiDto input)
        {
            if (input.end.HasValue)
            {
                input.end = input.end.Value.AddDays(1);
            }
            var repoApi = repo.Orm.GetRepository<PermissionApiEntity, long>();
            var data = await repo
                .WhereIf(input.key.NotNull(), a => a.Path.Contains(input.key) || a.Label.Contains(input.key))
                .WhereIf(input.start.HasValue && input.end.HasValue, a => a.CreatedTime.Value.BetweenEnd(input.start.Value, input.end.Value))
                .OrderBy(a => a.ParentId)
                .OrderBy(a => a.Sort)
                .ToListAsync(a => new PermissionListOutput { ApiPaths = string.Join(";", repoApi.Where(b => b.PermissionId == a.Id).ToList(b => b.Api.Path)) });
            return ApiResult.Ok(data);
        }
        [AuthenticationFilter]
        public async Task<ApiResult> GetInitPermissionApilist()
        {
            var result = new List<AuthenticationInfo>();
            result.AddRange(await stateManager.GetState<List<AuthenticationInfo>>(new PermissionState() { Key = "account" }) ?? new List<AuthenticationInfo>());
            result.AddRange(await stateManager.GetState<List<AuthenticationInfo>>(new PermissionState() { Key = "goods" }) ?? new List<AuthenticationInfo>());
            result.AddRange(await stateManager.GetState<List<AuthenticationInfo>>(new PermissionState() { Key = "trade" }) ?? new List<AuthenticationInfo>());
            result.AddRange(await stateManager.GetState<List<AuthenticationInfo>>(new PermissionState() { Key = "public" }) ?? new List<AuthenticationInfo>());
            return await ApiResult.Ok(result).Async();
        }

        [AuthenticationFilter]
        public async Task<ApiResult> GetPermissionList(PageQueryInputBase input)
        { 
            var Data = await repo.Orm.Select<Permission>().LeftJoin(a=>a.ParentId==a.Id).Count(out var Total).Page(input.Page,input.Limit).ToListAsync<GetPermissionListResponse>();
            return ApiResult.Ok(new PageQueryResonseBase<GetPermissionListResponse>(Data, Total));
        }

        [AuthenticationFilter(false)]
        public async Task<ApiResult> GetAllPermissions()
        {
            var permissions =await repo.Orm.Select<Permission>().ToListAsync();
            var result = permissions.Where(x => x.ParentId == 0).Select(x => new AllPermissionResponse()
            {
                Id = x.Id,
                Label = x.Label,
                Child = permissions.Where(y => y.ParentId == x.Id).Select(y => new AllPermissionResponse()
                {
                    Id = y.Id,
                    Label = y.Label
                })
            }).ToList();
            return ApiResult.Ok(result);
        }
        [AuthenticationFilter(false)]
        public async Task<ApiResult> GetPermCode()
        {
            var authUserInfoOutput = new AuthUserInfoOutput { };
            ////用户信息
            //authUserInfoOutput.User = await repo.Orm.Select<User>().Where(x=>x.Id== HttpContextExt.Current.User.Id).ToOneAsync<AuthUserProfileDto>();

            ////用户菜单
            //authUserInfoOutput.Menus = await repo.Select
            //    .Where(a => new[] { Domain.Entities.PermissionType.Group, Domain.Entities.PermissionType.Menu }.Contains(a.Type))
            //    .Where(a =>
            //        repo.Orm.Select<RolePermission>()
            //        .InnerJoin<UserRole>((b, c) => b.RoleId == c.RoleId && c.UserId == HttpContextExt.Current.User.Id)
            //        .Where(b => b.PermissionId == a.Id)
            //        .Any()
            //    )
            //    .OrderBy(a => a.ParentId)
            //    .OrderBy(a => a.Sort)
            //    .ToListAsync(a => new AuthUserMenuDto { ViewPath = a.View.Path });

            //用户权限点
            List<string> Permissions = await repo.Select
                .Where(a => a.Type == PermissionType.Dot)
                .Where(a =>
                    repo.Orm.Select<RolePermission>()
                    .InnerJoin<UserRole>((b, c) => b.RoleId == c.RoleId && c.UserId == HttpContextExt.Current.User.Id)
                    .Where(b => b.PermissionId == a.Id)
                    .Any()
                )
                .ToListAsync(a => a.Code);
            return  ApiResult.Ok(Permissions);
        }
    }
}
