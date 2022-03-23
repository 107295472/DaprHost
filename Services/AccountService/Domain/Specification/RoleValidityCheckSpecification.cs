using Domain.Entities;
using Domain.Repository;
using DomainBase;
using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specification
{
    public class RoleValidityCheckSpecification : ISpecification<Account>
    {
        private readonly IBaseRepository<Role> roleRepository;

        public RoleValidityCheckSpecification(IBaseRepository<Role> roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        public async Task<bool> IsSatisfiedBy(Account entity)
        {
            return true;
        }
    }
}
