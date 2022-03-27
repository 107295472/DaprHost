using DomainBase;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FreeSql;
using DomainBase.Entities;

namespace Domain.Specification
{
    /// <summary>
    /// 用户ID唯一规约
    /// </summary>
    public class UniqueAccountIdSpecification : ISpecification<UserEntity>
    {
        private readonly IUserRepository fsql;

        public UniqueAccountIdSpecification(IUserRepository sql)
        {
            this.fsql = sql;
        }

        public async Task<bool> IsSatisfiedBy(UserEntity entity)
        {
            if (await fsql.Where(x=>x.UserName==entity.UserName).FirstAsync() == null)
                return true;
            else
                throw new DomainException("账号必须唯一");
        }
    }
}
