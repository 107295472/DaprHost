using Client.ServerProxyFactory.Interface;
using Domain.Entities;
using FreeSql;
using IApplicationService.AccountService;
using IApplicationService.AccountService.Dtos.Output;
using IApplicationService.Base;
using IApplicationService.Base.AppQuery;


using InfrastructureBase.AuthBase;
using InfrastructureBase.Data;
using InfrastructureBase.Object;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationService
{
    public class RoleQueryService : IRoleQueryService
    {
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        private readonly IBaseRepository<Role> repo;
        public RoleQueryService(IBaseRepository<Role> sql, IEventBus eventBus, IStateManager stateManager)
        {
            this.repo = sql;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }

        [AuthenticationFilter]
        public async Task<ApiResult> GetRoleList(PageQueryInputBase input)
        {
            var Data = await repo.Select.Count(out var Total).Page(input.Page, input.Limit).OrderBy(x=>x.Name)
                .ToListAsync(x=>new GetRoleListResponse()
            {
                RoleId = x.Id,
                RoleName = x.Name
                });
            var roleIds = Data.Select(x => x.RoleId);
            var permissions = await repo.Orm.Select<RolePermission, Permission>()
                .LeftJoin((a,b)=>a.PermissionId==b.Id).Where((a,b)=> roleIds.Contains(a.RoleId)).ToListAsync((a,b)=> new { b.Id, a.RoleId });

            Data.ForEach(x => x.Permissions = permissions.Where(y => y.RoleId == x.RoleId).Select(y => y.Id).ToList());
            return ApiResult.Ok(new PageQueryResonseBase<GetRoleListResponse>(Data, Total));
        }

        [AuthenticationFilter]
        public async Task<ApiResult> GetAllRoles()
        {
            return await ApiResult.Ok(repo.Select.ToListAsync(x => new { RoleId = x.Id, RoleName = x.Name })).Async();
        }
    }
}
