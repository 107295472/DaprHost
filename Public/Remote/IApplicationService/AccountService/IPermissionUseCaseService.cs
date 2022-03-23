using IApplicationService.AccountService.Dtos.Input;
using IApplicationService.Base;
using Client.ServerSymbol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.AccountService
{
    [RemoteService("accountservice", "permissionusecase", "权限服务")]
    public interface IPermissionUseCaseService
    {
        [RemoteFunc(funcDescription: "批量保存权限")]
        Task<ApiResult> SavePermissions(List<CreatePermissionDto> input);
        [RemoteFunc(funcDescription: "添加权限")]
        Task<ApiResult> AddPermissions(AddPermissionDto input);
        [RemoteFunc(funcDescription: "角色分配权限")]
        Task<ApiResult> AssignPsermission(AssignPsermissionDto input);

    }
}
