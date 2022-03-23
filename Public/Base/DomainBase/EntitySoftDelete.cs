using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace DomainBase
{
    /// <summary>
    /// 实体软删除
    /// </summary>
    public class EntitySoftDelete : Entity, IEntitySoftDelete
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        [Description("是否删除")]
        [Column(Position = -1)]
        public bool IsDeleted { get; set; } = false;
    }
}