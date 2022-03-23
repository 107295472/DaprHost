using Domain.Entities;
using DomainBase;
using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IRoleRepository : IBaseRepository<Role,long>
    {
        Task<bool> RoleRelationUser(long roleId);
        //void Add(Role t);
    }
}
