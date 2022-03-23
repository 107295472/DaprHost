using DomainBase;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using FreeSql.DataAnnotations;

namespace Domain.Entities
{
    /// <summary>
    /// 用户领域
    /// </summary>
    [Table(Name = "User")]
    public class User : Entity
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Column(StringLength = 60)]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Column(StringLength = 60)]
        public string Password { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Column(StringLength = 60)]
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [Column(StringLength = 100)]
        public string Avatar { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Column(MapType = typeof(int))]
        public AccountState Status { get; set; }
        /// <summary>
        /// 超级管理
        /// </summary>
        public bool SuperAdmin { get; set; } = false;
        /// <summary>
        /// 备注
        /// </summary>
        [Column(StringLength = 500)]
        public string Remark { get; set; }

        [Navigate(ManyToMany = typeof(UserRole))]
        public ICollection<Role> Roles { get; set; }
        /// <summary>
        /// 检查用户状态
        /// </summary>
        /// <returns></returns>
        public void CheckAccountCanLogin(string password)
        {
            if (Password != password)
                throw new DomainException("用户密码错误!");
            if (Status == AccountState.Locking)
                throw new DomainException("用户被锁定!");
        }
    }
}
