using Client.ServerSymbol;
using IApplicationService.PublicService.Dtos.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IApplicationService.Base;

namespace IApplicationService.PublicService
{
    [RemoteService("publicservice", "mallsettingusecase", "公共服务")]
    public interface IMallSettingUseCaseService
    {
        [RemoteFunc(funcDescription: "创建或更新商城配置")]
        Task<ApiResult> CreateOrUpdateMallSetting(CreateOrUpdateMallSettingDto input);
    }
}
