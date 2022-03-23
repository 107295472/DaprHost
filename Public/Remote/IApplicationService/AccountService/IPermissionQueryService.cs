using IApplicationService.AccountService.Dtos.Input;
using IApplicationService.Base;
using IApplicationService.Base.AppQuery;
using Client.ServerSymbol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.AccountService
{
    [RemoteService("accountservice", "permissionquery", "权限服务")]
    public interface IPermissionQueryService
    {
        [RemoteFunc(funcDescription: "获取初始化权限接口")]
        Task<ApiResult> GetInitPermissionApilist();

        [RemoteFunc(funcDescription: "获取权限列表")]
        Task<ApiResult> GetPermissionList(PageQueryInputBase input);

        [RemoteFunc(funcDescription: "获取所有权限")]
        Task<ApiResult> GetAllPermissions();

        [RemoteFunc(funcDescription: "获取用户权限码")]
        Task<ApiResult> GetPermCode();
        [RemoteFunc(funcDescription: "权限列表")]
        Task<ApiResult> GetList(PermissionApiDto input);
    }
}
