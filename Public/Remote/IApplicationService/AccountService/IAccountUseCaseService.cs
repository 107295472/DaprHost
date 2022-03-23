﻿using IApplicationService.AccountService.Dtos.Input;
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
    [RemoteService("accountservice", "accountusecase", "用户服务")]
    public interface IAccountUseCaseService
    {
        [RemoteFunc(funcDescription: "初始化RBAC")]
        Task<ApiResult> InitRoleBasedAccessControler(InitUserOauthDto input);

        [RemoteFunc(funcDescription:"用户注册")]
        Task<ApiResult> AccountRegister(CreateAccountDto input);

        [RemoteFunc(funcDescription:"创建用户")]
        Task<ApiResult> AccountCreate(UserAddInput input);

        [RemoteFunc(funcDescription:"用户信息更新")]
        Task<ApiResult> AccountUpdate(UpdateAccountDto input);

        [RemoteFunc(funcDescription:"锁定/解锁用户")]
        Task<ApiResult> LockOrUnLockAccount(LockOrUnLockAccountDto input);

        [RemoteFunc(funcDescription:"删除用户")]
        Task<ApiResult> AccountDelete(AccountDeleteDto input);

        [RemoteFunc(funcDescription:"用户登录")]
        Task<ApiResult> AccountLogin(AuthLoginInput input);

        [RemoteFunc(funcDescription:"用户登出")]
        Task<ApiResult> AccountLoginOut();

        [RemoteFunc(funcDescription:"完善用户信息")]
        Task<ApiResult> SupplementaryAccountInfo(SupplementaryUserDto input);
        [RemoteFunc(funcDescription: "用户分配角色")]
        Task<ApiResult> AssignRole(AssignRoleDto input);
    }
}
