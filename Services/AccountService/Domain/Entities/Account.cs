﻿using Domain.Enums;
using DomainBase;
using System;
using System.Collections.Generic;
using System.Linq;
using FreeSql.DataAnnotations;
namespace Domain.Entities
{
    /// <summary>
    /// 账号领域实体
    /// </summary>
    [Table(Name ="Account")]
    public class Account : Entity, IAggregateRoot
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 账户状态
        /// </summary>
        public AccountState State { get; set; }
        public bool SuperAdmin { get; set; }
        /// <summary>
        /// 用户信息
        /// </summary>
        [Column(IsIgnore = true)]
        public User User { get; set; }
        /// <summary>
        /// 用户权限
        /// </summary>
        [Column(IsIgnore = true)]
        public List<long> Roles { get; set; }

        /// <summary>
        /// 创建账号
        /// </summary>
        /// <returns></returns>
        public void CreateAccount(string loginName, string nickName, string sourcePassword, Func<string, object[], string> md5password)
        {
            LoginName = loginName;
            NickName = nickName;
            Password = md5password(sourcePassword, new object[] { Id });
            State = AccountState.Normal;
            //初始化时不收集用户信息
            User = new User();
            //创建账号时角色为空
            Roles = new List<long>();
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        public void UpdateNicknameOrPassword(string nickName, string password)
        {
            if (!string.IsNullOrEmpty(password))
            {
                if (string.IsNullOrEmpty(Password))
                    Password = password;
                else if (Password == password)
                    throw new DomainException("新旧密码不能相同!");
                else
                    Password = password;
            }
            if (!string.IsNullOrEmpty(nickName))
            {
                this.NickName = nickName;
            }
        }
        /// <summary>
        /// 锁定用户
        /// </summary>
        public void ChangeAccountLockState(long currentId)
        {
            if (Id == currentId)
                throw new DomainException("登录用户不能锁定自身!");
            State = State == AccountState.Normal ? AccountState.Locking : AccountState.Normal;
        }
        public void SetRoles(List<long> roleids)
        {
            if (roleids == null || !roleids.Any())
                throw new DomainException("请至少选择一个角色!");
            Roles = roleids;
        }
        /// <summary>
        /// 检查用户状态
        /// </summary>
        /// <returns></returns>
        public void CheckAccountCanLogin(string password)
        {
            if (Password != password)
                throw new DomainException("用户密码错误!");
            if (State == AccountState.Locking)
                throw new DomainException("用户被锁定!");
        }
    }
}
