using Client.ServerProxyFactory.Interface;
using Domain.Repository;
using Domain.Specification;
using IApplicationService.Base;
using IApplicationService.GoodsService;
using IApplicationService.GoodsService.Dtos.Input;


using InfrastructureBase;
using InfrastructureBase.AuthBase;
using System.Threading.Tasks;

namespace ApplicationService
{
    public class LimitedTimeActivitieUseCaseService : ILimitedTimeActivitieUseCaseService
    {
        private readonly ILimitedTimeActivitieRepository repository;
        private readonly IGoodsRepository goodsRepository;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        public LimitedTimeActivitieUseCaseService(ILimitedTimeActivitieRepository repository, IGoodsRepository goodsRepository, IEventBus eventBus, IStateManager stateManager)
        {
            this.repository = repository;
            this.goodsRepository = goodsRepository;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }

        public Task<ApiResult> CreateLimitedTimeActivitie(LimitedTimeActivitieCreateDto input)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApiResult> DeleteLimitedTimeActivitie(LimitedTimeActivitieDeleteDto input)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApiResult> UpdateLimitedTimeActivitie(LimitedTimeActivitieUpdateDto input)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApiResult> UpOrDownShelfActivitie(UpOrDownShelfActivitieDto input)
        {
            throw new System.NotImplementedException();
        }
    }
}
