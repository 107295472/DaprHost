using Domain;
using Domain.Repository;
using IApplicationService.TradeService.Dtos.Output;
using IApplicationService.TradeService.Dtos.Input;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Object;
using System.Threading.Tasks;
using IApplicationService.Base.AppQuery;
using System.Linq;
using InfrastructureBase.Data;
using IApplicationService.TradeService;
using System.Collections.Generic;
using Domain.Enums;
using IApplicationService.AccountService;
using IApplicationService.AccountService.Dtos.Input;
using IApplicationService.AccountService.Dtos.Output;
using System;
using IApplicationService.Base;
using Client.ServerProxyFactory.Interface;

namespace ApplicationService
{
    public class OrderQueryService : IOrderQueryService
    {
        private readonly IAccountQueryService accountQuery;
        private readonly IStateManager stateManager;
        public OrderQueryService(IStateManager stateManager, IAccountQueryService accountQuery)
        {
            this.stateManager = stateManager;
            this.accountQuery = accountQuery;
        }

        [AuthenticationFilter]
        public async Task<ApiResult> GetOrderList(PageQueryInputBase input)
        {
            return ApiResult.Ok();
        }

        [AuthenticationFilter(false)]
        public async Task<ApiResult> GetOrderSellCountByGoodsId(GetOrderSellCountByGoodsIdDto input)
        {
            return ApiResult.Ok();
        }
    }
}
