using DomainBase;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table(Name = "Role")]
    [Index("idx_{tablename}_01", nameof(Name), true)]
    public class Role : Entity, IAggregateRoot
    {
        public string Name { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        [Column(StringLength = 200)]
        public string Description { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
		public bool Enabled { get; set; } = true;

        /// <summary>
        /// 排序
        /// </summary>
		public int Sort { get; set; }

        [Navigate(ManyToMany = typeof(UserRole))]
        public ICollection<User> Users { get; set; }

        [Navigate(ManyToMany = typeof(RolePermission))]
        public ICollection<Permission> Permissions { get; set; }
    }
}
