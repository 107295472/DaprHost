using Domain.Entities;
using Domain.Repository;
using IApplicationService.TradeService;
using IApplicationService.TradeService.Dtos.Output;
using IApplicationService.TradeService.Dtos.Input;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Object;
using System.Threading.Tasks;
using IApplicationService.Base.AppQuery;
using System.Linq;
using InfrastructureBase.Data;
using IApplicationService.Base;
using Client.ServerProxyFactory.Interface;

namespace ApplicationService
{
    public class TradeLogQueryService : ITradeLogQueryService
    {
        private readonly IStateManager stateManager;
        public TradeLogQueryService(IStateManager stateManager)
        {
            this.stateManager = stateManager;
        }
		
        [AuthenticationFilter(false)]
        public async Task<ApiResult> GetTradeLogListByOrderId(GetTradeLogListByOrderIdDto input)
        {
            return await ApiResult.Ok().Async();
        }
    }
}
