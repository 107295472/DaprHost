
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;
using Yitter.IdGenerator;

namespace DomainBase
{
    /// <summary>
    /// 领域实体标记
    /// </summary>
    public abstract class MongoEntity : MongoRepository.DAL.Entity
    {
        public DateTime CreateTime { get; set; } = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
    }
}
