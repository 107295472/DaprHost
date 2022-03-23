
using InfrastructureBase.AuthBase;
using System.Threading.Tasks;
using IApplicationService.Base.AppQuery;
using IApplicationService.Base;
using IApplicationService.GoodsService;
using Client.ServerProxyFactory.Interface;

namespace ApplicationService
{
    public class LimitedTimeActivitieQueryService : ILimitedTimeActivitieQueryService
    {
        private readonly IStateManager stateManager;
        public LimitedTimeActivitieQueryService(IStateManager stateManager)
        {
            this.stateManager = stateManager;
        }
		
        [AuthenticationFilter]
        public async Task<ApiResult> GetLimitedTimeActivitieList(PageQueryInputBase input)
        {
           return ApiResult.Ok();
        }
		
    }
}
