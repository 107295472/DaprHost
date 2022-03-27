using DomainBase.Input;
using IApplicationService.AccountService.Dtos.Input;
using IApplicationService.Base;
using IApplicationService.Base.AppQuery;
using Client.ServerSymbol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DomainBase.Entities;

namespace IApplicationService.AccountService
{
    [RemoteService("accountservice", "accountquery", "用户服务")]
    public interface IAccountQueryService
    {
        [RemoteFunc(funcDescription: "获取用户信息")]
        Task<ApiResult> GetUserInfo();

        [RemoteFunc(funcDescription: "检查是否初始化RBAC")]
        Task<ApiResult> CheckRoleBasedAccessControler();

        [RemoteFunc(funcDescription: "获取用户信息")]
        Task<ApiResult> GetAccountList(PageQueryInputBase input);

        [RemoteFunc(funcDescription: "根据用户编号获取用户姓名")]
        Task<ApiResult> GetAccountUserNameByIds(GetAccountUserNameByIdsDto input);

        [RemoteFunc(funcDescription: "用户列表")]
        Task<ApiResult> GetPage(PageInput<UserEntity> input);
        [RemoteFunc(funcDescription: "负载测试")]
        Task<ApiResult> SlbTest();
        [RemoteFunc(funcDescription: "获取一个模拟用户数据")]
        Task<ApiResult> GetMockAccount();

    }
}
