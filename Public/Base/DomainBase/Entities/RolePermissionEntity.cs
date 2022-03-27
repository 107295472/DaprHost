using DomainBase;
using FreeSql.DataAnnotations;

namespace DomainBase.Entities
{
    /// <summary>
    /// 角色权限
    /// </summary>
	[Table(Name = "ad_role_permission")]
    [Index("idx_{tablename}_01", nameof(RoleId) + "," + nameof(PermissionId), true)]
    public class RolePermissionEntity : Entity, IAggregateRoot
    {
        /// <summary>
        /// 角色Id
        /// </summary>
		public long RoleId { get; set; }

        /// <summary>
        /// 权限Id
        /// </summary>
		public long PermissionId { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public RoleEntity Role { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public PermissionEntity Permission { get; set; }
    }
}