using IApplicationService.Base;
using IApplicationService.Base.AppQuery;
using Client.ServerSymbol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.AccountService
{
    [RemoteService("accountservice", "rolequery", "角色服务")]
    public interface IRoleQueryService
    {
        [RemoteFunc(funcDescription: "获取角色列表")]
        Task<ApiResult> GetRoleList(PageQueryInputBase input);

        [RemoteFunc(funcDescription: "获取所有角色")]
        Task<ApiResult> GetAllRoles();
    }
}
