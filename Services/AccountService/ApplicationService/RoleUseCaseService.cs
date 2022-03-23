using Domain;
using Domain.Entities;
using Domain.Repository;
using Domain.Specification;
using FreeSql;
using IApplicationService.AccountService.Dtos.Input;
using InfrastructureBase.Object;
using InfrastructureBase;
using InfrastructureBase.AopFilter;
using InfrastructureBase.AuthBase;
using System;
using System.Linq;
using System.Threading.Tasks;
using IApplicationService.Base;
using IApplicationService.AccountService;
using Client.ServerProxyFactory.Interface;

namespace ApplicationService
{
    public class RoleUseCaseService : IRoleUseCaseService
    {

        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        private readonly IBaseRepository<Role,long> repo;
        public RoleUseCaseService(IBaseRepository<Role,long> sql, IEventBus eventBus, IStateManager stateManager)
        {
            this.repo = sql;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }

        [Tran,AuthenticationFilter]
        public async Task<ApiResult> RoleCreate(RoleCreateDto input)
        {
            var role = input.CopyTo<RoleCreateDto, Role>();        
           await  repo.InsertAsync(role);
            //var rp = repo.Orm.GetRepository<RolePermission>();

            //rp.Delete(rp.Where(x => x.RoleId == role.Id).ToList());
            //var repoPer = repo.Orm.GetRepository<Permission>();
            //if (role.Permissions != null && role.Permissions.Any())
            //{
            //    await rp.InsertAsync(repoPer.Where(x => x.ParentId > 0 && role.Permissions.Select(x=>x.Id).Contains(x.Id)).ToList(x => new RolePermission() { RoleId = role.Id, PermissionId = x.Id }));
            //}
            return ApiResult.Ok("角色创建成功");
        }
        [AuthenticationFilter]
        public async Task<ApiResult> RoleUpdate(RoleUpdateDto input)
        {
            var role = await repo.GetAsync(input.RoleId);
            if (role == null)
                throw new ApplicationServiceException("所选角色不存在!");
            //role.SetRole(input.RoleName,input.Permissions);
            repo.Update(role);
            return ApiResult.Ok("角色更新成功");
        }

        [AuthenticationFilter]
        public async Task<ApiResult> RoleDelete(RoleDeleteDto input)
        {
            var role = await repo.GetAsync(input.RoleId);
            if (role == null)
                throw new ApplicationServiceException("所选角色不存在!");
             await new RoleDeleteCheckSpecification(repo).IsSatisfiedBy(role);
             repo.Delete(role);
             return ApiResult.Ok("角色删除成功");
        }
    }
}
