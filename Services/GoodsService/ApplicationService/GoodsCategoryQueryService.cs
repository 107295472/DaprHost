using Client.ServerProxyFactory.Interface;
using Domain.Entities;
using FreeSql;
using IApplicationService.Base;
using IApplicationService.Base.AppQuery;
using IApplicationService.GoodsService;
using InfrastructureBase;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Data;
using InfrastructureBase.Object;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationService
{
    public class GoodsCategoryQueryService : IGoodsCategoryQueryService
    {
        private readonly IEventBus eventBus;
        private readonly ILocalEventBus localEventBus;
        private readonly IStateManager stateManager;
        private readonly IBaseRepository<GoodsCategory, long> repo;
        public GoodsCategoryQueryService(IBaseRepository<GoodsCategory, long> re, IFreeSql sql, IEventBus eventBus, ILocalEventBus localEventBus, IStateManager stateManager)
        {
            this.repo = re;
            this.eventBus = eventBus;
            this.localEventBus = localEventBus;
            this.stateManager = stateManager;
        }

        public async Task<ApiResult> GetAllCategoryList()
        {
            var query =repo.Select.OrderBy(x=>x.Sort).ToList(x=> new { x.Id, x.CategoryName });
            return await ApiResult.Ok(query).Async();
        }

        [AuthenticationFilter]
        public async Task<ApiResult> GetCategoryList(PageQueryInputBase input)
        {
            var Data = await repo.Select.OrderBy(x => x.Sort).Count(out var Total).Page(input.Page, input.Limit).ToListAsync();
            return ApiResult.Ok(new PageQueryResonseBase<GoodsCategory>(Data, Total));
        }
    }
}
