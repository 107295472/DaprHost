using Client.ServerProxyFactory.Interface;
using Domain;
using Domain.Entities;
using Domain.Repository;
using Domain.Specification;
using FreeSql;
using IApplicationService.Base;
using IApplicationService.GoodsService;
using IApplicationService.GoodsService.Dtos.Input;
using InfrastructureBase;
using InfrastructureBase.AopFilter;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Object;
using System.Threading.Tasks;

namespace ApplicationService
{
    public class GoodsCategoryUseCaseService : IGoodsCategoryUseCaseService
    {
        private readonly IEventBus eventBus;
        private readonly ILocalEventBus localEventBus;
        private readonly IStateManager stateManager;
        private readonly IBaseRepository<GoodsCategory, long> repo;
        public GoodsCategoryUseCaseService(IBaseRepository<GoodsCategory, long> re, IFreeSql sql, IEventBus eventBus, ILocalEventBus localEventBus, IStateManager stateManager)
        {
            this.repo = re;
            this.eventBus = eventBus;
            this.localEventBus = localEventBus;
            this.stateManager = stateManager;
        }
        [AuthenticationFilter]
        [Tran]
        public async Task<ApiResult> CreateCategory(CategoryCreateDto input)
        {
            var goodsCategory = new GoodsCategory();
            goodsCategory.CreateOrUpdate(input.CategoryName, input.Sort);
            repo.Insert(goodsCategory);
            await new UniqueGoodsCategoryNameSpecification(repo).IsSatisfiedBy(goodsCategory);
            return ApiResult.Ok("商品分类创建成功");
        }
        [AuthenticationFilter]
        public async Task<ApiResult> DeleteCategory(CategoryDeleteDto input)
        {
            var goodsCategory = await repo.GetAsync(input.Id);
            if (goodsCategory == null)
                throw new ApplicationServiceException("没有找到该商品分类!");
            repo.Delete(goodsCategory);
            await new CheckGoodsCategoryCanRemoveSpecification(repo).IsSatisfiedBy(goodsCategory);
            return ApiResult.Ok("商品分类删除成功");
        }
        [AuthenticationFilter]
        [Tran]
        public async Task<ApiResult> UpdateCategory(CategoryUpdateDto input)
        {
            var goodsCategory = await repo.GetAsync(input.Id);
            if (goodsCategory == null)
                throw new ApplicationServiceException("没有找到该商品分类!");
            goodsCategory.CreateOrUpdate(input.CategoryName, input.Sort);
            repo.Update(goodsCategory);
            await new UniqueGoodsCategoryNameSpecification(repo).IsSatisfiedBy(goodsCategory);
            return ApiResult.Ok("商品分类更新成功");
        }
    }
}
