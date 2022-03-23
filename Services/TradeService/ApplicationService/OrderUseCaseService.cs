using System;
using System.Linq;
using System.Threading.Tasks;
using IApplicationService.Base.AppQuery;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Data;
using IApplicationService.TradeService.Dtos.Output;
using IApplicationService.TradeService.Dtos.Input;
using Domain.Repository;
using InfrastructureBase;
using IApplicationService.TradeService;
using IApplicationService.GoodsService;
using Domain.Services;
using DomainBase;
using System.Collections.Generic;
using Domain.Entities;
using InfrastructureBase.Object;
using Domain.Specification;
using Domain.ValueObject;
using Domain.Dtos;
using IApplicationService.GoodsService.Dtos.Input;
using InfrastructureBase.Http;
using Domain.Events;
using IApplicationService.AccountService;
using IApplicationService.AccountService.Dtos.Output;
using IApplicationService.Base;
using IApplicationService.Base.AppEvent;
using Client.ServerProxyFactory.Interface;

namespace ApplicationService
{
    public class OrderUseCaseService : IOrderUseCaseService
    {
        private readonly IOrderRepository repository;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        private readonly IGoodsQueryService goodsQueryService;
        private readonly IGoodsActorService goodsActorService;
        private readonly IAccountQueryService accountQueryService;
        public OrderUseCaseService(IOrderRepository repository, IAccountQueryService accountQueryService, IEventBus eventBus, IStateManager stateManager, IGoodsQueryService goodsQueryService, IGoodsActorService goodsActorService)
        {
            this.repository = repository;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
            this.goodsQueryService = goodsQueryService;
            this.goodsActorService = goodsActorService;
            this.accountQueryService = accountQueryService;
        }

        public async Task<ApiResult> CreateOrder(OrderCreateDto input)
        {
            return await ApiResult.Ok().Async();
        }
        public async Task<ApiResult> OrderPay(OrderPayDto input)
        {

            return await ApiResult.Ok().Async();
        }

        #region 私有远程服务包装器方法
        async Task<List<OrderGoodsSnapshot>> GetGoodsListByIds(IEnumerable<long> input)
        {
            return (await goodsQueryService.GetGoodsListByIds(new GetGoodsListByIdsDto(input))).GetData<List<OrderGoodsSnapshot>>();
        }
        async Task<bool> DeductionGoodsStock(CreateOrderDeductionGoodsStockDto input)
        {
            var data = input.CopyTo<CreateOrderDeductionGoodsStockDto, DeductionStockDto>();
            data.ActorId = input.GoodsId.ToString();
            return (await goodsActorService.DeductionGoodsStock(data)).GetData<bool>();
        }
        async Task<bool> UnDeductionGoodsStock(CreateOrderDeductionGoodsStockDto input)
        {
            var data = input.CopyTo<CreateOrderDeductionGoodsStockDto, DeductionStockDto>();
            data.ActorId = input.GoodsId.ToString();
            return (await goodsActorService.UnDeductionGoodsStock(data)).GetData<bool>();
        }
        #endregion
    }
}
