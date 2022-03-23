using System.Threading.Tasks;
using IApplicationService.TradeService;
using InfrastructureBase.AuthBase;
using IApplicationService.TradeService.Dtos.Input;
using Domain.Repository;
using IApplicationService.PublicService;
using IApplicationService.Base;
using Client.ServerProxyFactory.Interface;

namespace ApplicationService
{
    public class LogisticsUseCaseService : ILogisticsUseCaseService
    {
        private readonly ILogisticsRepository repository;
        private readonly IOrderRepository orderRepository;
        private readonly IMallSettingQueryService mallSettingQuery;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        public LogisticsUseCaseService(ILogisticsRepository repository, IOrderRepository orderRepository, IMallSettingQueryService mallSettingQuery, IEventBus eventBus, IStateManager stateManager)
        {
            this.repository = repository;
            this.orderRepository = orderRepository;
            this.mallSettingQuery = mallSettingQuery;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }
        [AuthenticationFilter(false)]
        public async Task<ApiResult> Deliver(LogisticsDeliverDto input)
        {
            return ApiResult.Ok();
        }

        [AuthenticationFilter(false)]
        public async Task<ApiResult> Receive(LogisticsReceiveDto input)
        {
           return ApiResult.Ok();
        }
    }
}
