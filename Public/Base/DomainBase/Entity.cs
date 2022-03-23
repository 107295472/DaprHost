using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace DomainBase
{

    public class Entity 
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Description("主键Id")]
        [Column(Position = 1, IsIdentity = false, IsPrimary = true)]
        public virtual long Id { get; set; } = Yitter.IdGenerator.YitIdHelper.NextId();
    }
}