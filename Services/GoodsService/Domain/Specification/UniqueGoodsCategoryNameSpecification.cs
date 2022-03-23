using Domain.Entities;
using Domain.Repository;
using DomainBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;
namespace Domain.Specification
{
    public class UniqueGoodsCategoryNameSpecification : ISpecification<GoodsCategory>
    {
        private readonly IBaseRepository<GoodsCategory,long> categoryRepository;
        public UniqueGoodsCategoryNameSpecification(IBaseRepository<GoodsCategory,long> categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        public async Task<bool> IsSatisfiedBy(GoodsCategory entity)
        {
            if (await categoryRepository.Select.AnyAsync(x => x.CategoryName == entity.CategoryName && x.Id != entity.Id))
                throw new DomainException("商品分类名重复!");
            else
                return true;
        }
    }
}
