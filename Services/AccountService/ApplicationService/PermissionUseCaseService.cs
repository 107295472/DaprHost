using Domain.Dtos;
using Domain.Repository;
using Domain.Services;
using IApplicationService.AccountService.Dtos.Input;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfrastructureBase.Object;
using InfrastructureBase.AuthBase;
using FreeSql;
using InfrastructureBase.AopFilter;
using Domain.Entities;
using InfrastructureBase;
using System;
using Domain.Enums;
using Mapster;
using IApplicationService.AccountService;
using IApplicationService.Base;
using InfrastructureBase.Data;
using InfrastructureBase.Extensions;
using Client.ServerProxyFactory.Interface;
using DomainBase.Entities;

namespace ApplicationService
{
    public class PermissionUseCaseService : IPermissionUseCaseService
    {
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        private readonly IRepositoryBase<PermissionEntity> repo;
        public PermissionUseCaseService(IRepositoryBase<PermissionEntity> sql)
        {
            this.repo = sql;
        }

        public async Task<ApiResult> AddPermissions(AddPermissionDto input)
        {
            PermissionEntity p = new PermissionEntity();
            //p.CreatePermission(input.PermissionID,input.PermissionName,input.Path,input.Code,Enum.Parse<PermissionType>(input.Type),input.ViewID,input.Desc);
            await repo.InsertAsync(p);

            return ApiResult.Ok();
        }
        /// <summary>
        /// 给角色分配权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Tran]
        [AuthenticationFilter]
        public async Task<ApiResult> AssignPsermission(AssignPsermissionDto input)
        {
            var rolePer = new List<RolePermission>();

            for (int i = 0; i < input.PermissionIds.Count; i++)
            {
                rolePer.Add(new RolePermission()
                {
                    RoleId = input.RoleID,
                    PermissionId = input.PermissionIds[i]
                }); ;
            }
            repo.Orm.Delete<RolePermission>().Where(x => x.RoleId == input.RoleID).ExecuteAffrows();
            await repo.Orm.GetRepository<RolePermission>().InsertAsync(rolePer);

            return ApiResult.Ok();
        }


        [AuthenticationFilter]
        public async Task<ApiResult> SavePermissions(List<CreatePermissionDto> input)
        {
            var config = new TypeAdapterConfig();
            config.NewConfig<CreatePermissionDto, CreatePermissionTmpDto>()
    .Map(dest => dest.ServerName, src => src.SrvName)
    .Map(dest => dest.PermissionName, src => src.FuncName);

            var result = input.Adapt<List<CreatePermissionTmpDto>>(config);
            new PermissionMultiCreateService(repo, result).Create();
            return await ApiResult.Ok("权限批量导入成功").Async();
        }
    }
}
