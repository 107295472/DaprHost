using Domain.Repository;
using IApplicationService.Base.AppQuery;
using IApplicationService.GoodsService.Dtos.Output;

using InfrastructureBase.Data;
using System.Threading.Tasks;
using System.Linq;
using IApplicationService.GoodsService.Dtos.Input;
using InfrastructureBase.Object;
using InfrastructureBase.AuthBase;
using IApplicationService.GoodsService;
using System;
using Autofac;
using System.Collections.Generic;
using FreeSql;
using Domain.Entities;
using IApplicationService.Base;
using InfrastructureBase;
using Client.ServerProxyFactory.Interface;

namespace ApplicationService
{
    public class GoodsQueryService : IGoodsQueryService
    {
        private readonly IEventBus eventBus;
        private readonly ILocalEventBus localEventBus;
        private readonly IStateManager stateManager;
        private readonly IBaseRepository<Goods, long> repo;
        public GoodsQueryService(IBaseRepository<Goods, long> re, IFreeSql sql, IEventBus eventBus, ILocalEventBus localEventBus, IStateManager stateManager)
        {
            this.repo = re;
            this.eventBus = eventBus;
            this.localEventBus = localEventBus;
            this.stateManager = stateManager;
        }
        [AuthenticationFilter]
        public async Task<ApiResult> GetGoodsList(PageQueryInputBase input)
        {
            return ApiResult.Ok();
        }

        public async Task<ApiResult> GetGoodslistByGoodsName(GetGoodslistByGoodsNameDto input)
        {
            return await ApiResult.Ok().Async();
        }

        public async Task<ApiResult> GetGoodsListByIds(GetGoodsListByIdsDto input)
        {
           return await ApiResult.Ok().Async();
        }

        public async Task<ApiResult> GetEsGoods(PageQueryInputBase input)
        {
           return await ApiResult.Ok().Async();
        }
    }
}
