using DomainBase;
using FreeSql.DataAnnotations;

namespace DomainBase.Entities
{
    /// <summary>
    /// 权限接口
    /// </summary>
	[Table(Name = "ad_permission_api")]
    [Index("idx_{tablename}_01", nameof(PermissionId) + "," + nameof(ApiId), true)]
    public class PermissionApiEntity : EntityAdd, IAggregateRoot
    {
        /// <summary>
        /// 权限Id
        /// </summary>
		public long PermissionId { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public PermissionEntity Permission { get; set; }

        /// <summary>
        /// 接口Id
        /// </summary>
		public long ApiId { get; set; }

        /// <summary>
        /// 接口
        /// </summary>
        public ApiEntity Api { get; set; }
    }
}