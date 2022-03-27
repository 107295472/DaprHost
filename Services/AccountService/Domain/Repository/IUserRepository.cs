using DomainBase;
using DomainBase.Entities;
using FreeSql;
using IApplicationService.Base;
using InfrastructureBase.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IUserRepository : IRepositoryBase<UserEntity>
    {
        Task<ApiResult> Add(UserEntity t);
        Task<List<string>> GetPermissionsAsync(long uid);
    }
}
