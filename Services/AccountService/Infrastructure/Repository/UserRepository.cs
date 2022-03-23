using Domain.Entities;
using Domain.Repository;
using FreeSql;
using IApplicationService.Base;
using InfrastructureBase.Data;
using InfrastructureBase.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class UserRepository : RepositoryBase<UserEntity>, IUserRepository
    {
        public UserRepository(UnitOfWorkManager uowm) : base(uowm) { }
        public async Task<ApiResult> Add(UserEntity user)
        {

            await InsertAsync(user);
            if (user.Roles?.Count > 0)
                Orm.GetRepository<UserRole>().Insert(user.Roles.Select(x => new UserRole { UserId = user.Id, RoleId = x.Id }));
            return ApiResult.Ok();
        }
        public async Task<List<string>> GetPermissionsAsync(long uid)
        {
            return await Orm.GetRepository<ApiEntity>()
                .Where(a => Orm.Select<UserRoleEntity, RolePermissionEntity, PermissionApiEntity>()
                .InnerJoin((b, c, d) => b.RoleId == c.RoleId && b.UserId ==uid)
                .InnerJoin((b, c, d) => c.PermissionId == d.PermissionId)
                .Where((b, c, d) => d.ApiId == a.Id).Any())
                .ToListAsync(a=>a.Path);
        }
    }
}
