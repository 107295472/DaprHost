using ApplicationService.Dtos;
using Autofac;
using Client.ServerProxyFactory.Interface;
using Domain.Entities;
using FreeSql;
using IApplicationService.Base;
using IApplicationService.GoodsService;
using IApplicationService.GoodsService.Dtos.Input;
using InfrastructureBase;
using InfrastructureBase.AopFilter;
using InfrastructureBase.Object;
using Mesh.Dapr;
using System;
using System.Threading.Tasks;

namespace ApplicationService
{
    public class GoodsActorService: BaseActorService<GoodsActor>, IGoodsActorService
    {
        private readonly IEventBus eventBus;
        private readonly ILocalEventBus localEventBus;
        private readonly IStateManager stateManager;
        private readonly IBaseRepository<Goods,long> repo;
        public GoodsActorService(IBaseRepository<Goods,long> re, IFreeSql sql, IEventBus eventBus, ILocalEventBus localEventBus, IStateManager stateManager)
        {
            this.repo = re;
            this.eventBus = eventBus;
            this.localEventBus = localEventBus;
            this.stateManager = stateManager;
        }
        public async Task<ApiResult> UpdateGoodsStock(DeductionStockDto input)
        {
            return await ApiResult.Ok(true, "商品库存更新成功!").RunAsync(async () =>
            {
                await SetActorDataIfNotExists(input.GoodsId);
                ActorData.ChangeStock(input.DeductionStock);
            });
        }

        public async Task<ApiResult> DeductionGoodsStock(DeductionStockDto input)
        {
            return await ApiResult.Ok(true, "商品库存减扣成功").RunAsync(async () =>
            {
                await SetActorDataIfNotExists(input.GoodsId);
                ActorData.DeductionStock(input.DeductionStock);
            });
        }
        public async Task<ApiResult> UnDeductionGoodsStock(DeductionStockDto input)
        {
            return await ApiResult.Ok(true, "商品库存回滚成功").RunAsync(async () =>
            {
                await SetActorDataIfNotExists(input.GoodsId);
                ActorData.UnDeductionStock(input.DeductionStock);
            });
        }
        async Task SetActorDataIfNotExists(long id)
        {
            if (ActorData == null)
            {
                Goods goods = default;
                try
                {
                    goods = await  repo.GetAsync(id);
                }
                catch(Exception e)
                {
                    Console.WriteLine($"商品对象获取失败,失败原因：{e.GetBaseException()?.Message ?? e.Message}");
                }
                finally
                {
                    if (goods == null)
                        throw new ApplicationServiceException("没有找到该商品!");
                }
                ActorData = goods.CopyTo<Goods, GoodsActor>();
            }
        }
        [Tran]
        public override async Task SaveData(GoodsActor model, ILifetimeScope scope)
        {
            var goods = await repo.GetAsync(model.Id);
            if (goods != null)
                goods.ChangeStock(model.Stock);
            repo.Update(goods);
            //await localEventBus.SendEvent(EventTopicDictionary.Goods.Loc_WriteToElasticsearch, goods);
            await Task.CompletedTask;
        }
    }
}
