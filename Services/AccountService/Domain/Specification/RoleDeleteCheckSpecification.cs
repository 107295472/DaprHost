using Domain.Entities;
using Domain.Repository;
using DomainBase;
using FreeSql;
using InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specification
{
    public class RoleDeleteCheckSpecification : ISpecification<Role>
    {
        private readonly IBaseRepository<Role,long> repo;
        public RoleDeleteCheckSpecification(IBaseRepository<Role,long> rolerepository)
        {
            this.repo = rolerepository;
        }

        public async Task<bool> IsSatisfiedBy(Role entity)
        {
           
            if (await repo.Orm.GetRepository<UserRole>().Where(x=>x.RoleId==entity.Id).AnyAsync())
                throw new DomainException("当前角色包含用户关联，不能删除");
            return true;
        }
    }
}
