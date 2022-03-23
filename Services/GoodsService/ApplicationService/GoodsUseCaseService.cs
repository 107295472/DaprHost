using Domain.Repository;
using IApplicationService.GoodsService.Dtos.Input;
using System.Threading.Tasks;
using InfrastructureBase.AuthBase;
using ApplicationService.Dtos;
using Autofac;
using IApplicationService.GoodsService;
using IApplicationService.Base;
using InfrastructureBase;
using Client.ServerProxyFactory.Interface;
using Mesh.Dapr;

namespace ApplicationService
{
    public class GoodsUseCaseService : BaseActorService<GoodsActor>, IGoodsUseCaseService
    {
        private readonly IGoodsRepository repository;
        private readonly IGoodsCategoryRepository goodsCategoryRepository;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        private readonly IGoodsActorService actorService;
        private readonly IServiceProxyFactory serviceProxyFactory;
        private readonly ILocalEventBus localEventBus;
        public GoodsUseCaseService(IGoodsRepository repository, IGoodsCategoryRepository goodsCategoryRepository,ILocalEventBus localEventBus, IServiceProxyFactory serviceProxyFactory, IEventBus eventBus, IStateManager stateManager)
        {
            this.repository = repository;
            this.goodsCategoryRepository = goodsCategoryRepository;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
            this.serviceProxyFactory = serviceProxyFactory;
            this.actorService = serviceProxyFactory.CreateActorProxy<IGoodsActorService>();
            this.localEventBus = localEventBus;
        }
        [AuthenticationFilter]
        public async Task<ApiResult> CreateGoods(GoodsCreateDto input)
        {
           return ApiResult.Ok("商品创建失败");
        }
        [AuthenticationFilter]
        public async Task<ApiResult> UpdateGoods(GoodsUpdateDto input)
        {
          return ApiResult.Ok("商品更新失败");
        }
        [AuthenticationFilter]
        public async Task<ApiResult> UpOrDownShelfGoods(UpOrDownShelfGoodsDto input)
        {
          return ApiResult.Ok("商品上架/下架失败");
        }
        [AuthenticationFilter]
        public async Task<ApiResult> DeleteGoods(GoodsDeleteDto input)
        {
            //var goods = await repository.GetAsync(input.Id);
            //if (goods == null)
            //    throw new ApplicationServiceException("没有查询到该商品!");
            //repository.Delete(goods);
            //if (await unitofWork.CommitAsync())
            //{
            //    await localEventBus.SendEvent(EventTopicDictionary.Goods.Loc_RemoveToElasticsearch, goods);
            //    return ApiResult.Ok("商品删除成功");
            //}
            return ApiResult.Ok("商品删除失败");
        }

        [AuthenticationFilter]
        public async Task<ApiResult> UpdateGoodsStock(DeductionStockDto input)
        {
            input.ActorId = input.GoodsId.ToString();
            var result =  await actorService.UpdateGoodsStock(input);
            return result;
        }

        [AuthenticationFilter]
        public async Task<ApiResult> DeductionGoodsStock(DeductionStockDto input)
        {
            input.ActorId = input.GoodsId.ToString();
            return await actorService.DeductionGoodsStock(input);
        }

        public override async Task SaveData(GoodsActor model, ILifetimeScope scope)
        {
            var goods = await repository.GetAsync(model.Id);
            if (goods != null)
                goods.ChangeStock(model.Stock);
            await Task.CompletedTask;
        }
    }
}
