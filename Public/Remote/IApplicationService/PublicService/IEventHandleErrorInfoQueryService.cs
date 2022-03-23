using Client.ServerSymbol;
using IApplicationService.Base.AppQuery;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IApplicationService.Base;

namespace IApplicationService.PublicService
{
    [RemoteService("publicservice", "eventhandleerrorinfoquery", "公共服务")]
    public interface IEventHandleErrorInfoQueryService
    {
        [RemoteFunc(funcDescription: "获取列表")]
        Task<ApiResult> GetEventHandleErrorInfoList(PageQueryInputBase input);
        [RemoteFunc(funcDescription: "测试")]
        Task<ApiResult> PubTest();
    }
}
