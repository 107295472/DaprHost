using Client.ServerSymbol;
using IApplicationService.Base.AppQuery;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IApplicationService.Base;

namespace IApplicationService.GoodsService
{
    [RemoteService("goodsservice", "activitiquery", "限时活动服务")]
    public interface ILimitedTimeActivitieQueryService
    {
        [RemoteFunc(funcDescription: "获取限时活动列表")]
        Task<ApiResult> GetLimitedTimeActivitieList(PageQueryInputBase input);
    }
}
