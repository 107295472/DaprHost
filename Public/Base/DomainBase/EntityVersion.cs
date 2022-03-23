using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace DomainBase
{
    /// <summary>
    /// 实体版本
    /// </summary>
    public class EntityVersion : Entity, IEntityVersion
    {
        /// <summary>
        /// 版本
        /// </summary>
        [Description("版本")]
        [Column(Position = -1, IsVersion = true)]
        public long Version { get; set; }
    }
}