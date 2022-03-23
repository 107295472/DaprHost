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
    /// 用户角色
    /// </summary>
    [Index("idx_{tablename}_01", nameof(UserId) + "," + nameof(RoleId), true)]
    public class UserRole: Entity
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        public User User { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public long RoleId { get; set; }

        public Role Role { get; set; }
    }
}
