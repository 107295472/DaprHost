using Domain.Dtos;
using Domain.Entities;
using Domain.Repository;
using DomainBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;
namespace Domain.Services
{
    public class PermissionMultiCreateService
    {
        private readonly IBaseRepository<PermissionEntity> repo;
        private readonly IEnumerable<CreatePermissionTmpDto> input;
        public PermissionMultiCreateService(IBaseRepository<PermissionEntity> repository, IEnumerable<CreatePermissionTmpDto> input)
        {
            this.repo = repository;
            this.input = input;
        }
        public void Create()
        {
            if (input.Any())
            {
                var domain = new List<Permission>();
                var allFatherPermissions = input.GroupBy(x => x.ServerName).Select(x => x.Key).ToList();
                if (!allFatherPermissions.Any())
                    throw new DomainException("服务名无效!");
                repo.Delete(x =>true);//直接删除所有
                allFatherPermissions.ForEach(x =>
                {
                    var permission = new PermissionEntity();
                    permission.Label = x;
                    repo.Insert(permission);
                    var child = input.Where(y => y.ServerName == x).ToList();
                    if (!child.Any())
                    {
                        throw new DomainException($"服务{x}下没有有效接口!");
                    }
                    else
                    {
                        child.ForEach(y =>
                        {
                            if(string.IsNullOrEmpty(y.Path))
                                throw new DomainException($"服务{x}下没有有效接口地址!");
                            var childpermission = new PermissionEntity();
                            childpermission.Label = y.PermissionName;
                            childpermission.Path = y.Path;
                            //childpermission.CreatePermission(permission.Id, y.PermissionName, y.Path);
                            repo.Insert(childpermission);
                        });
                    }
                });
            }
            else
            {
                throw new DomainException("请至少输入一个权限");
            }
        }
    }
}
