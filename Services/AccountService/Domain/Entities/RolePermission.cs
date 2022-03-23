using DomainBase;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// 角色权限
    /// </summary>
    [Table(Name = "ad_role_permission")]
    [Index("idx_{tablename}_01", nameof(RoleId) + "," + nameof(PermissionId), true)]
    public class RolePermission : Entity
    {
        public long RoleId { get; set; }
        public long PermissionId { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public Permission Permission { get; set; }
    }
}
