using Domain.Entities;
using DomainBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FreeSql;

namespace Domain.Repository
{
    public class %placeholder%Repo : DefaultRepository<%placeholder%,long>,IUserRepo
    {
        public %placeholder%(UnitOfWorkManager uowm) : base(uowm?.Orm, uowm) { }
    }
}
