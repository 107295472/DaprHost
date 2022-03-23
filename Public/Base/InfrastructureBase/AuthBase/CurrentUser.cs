using DomainBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureBase.AuthBase
{
    public class CurrentUser
    {
        public CurrentUser()
        {

        }
        public CurrentUser(long id, string loginName, string nickName, List<string> permissions,long? tid)
        {
            Id = id;
            LoginName = loginName;
            NickName = nickName;
            TenantId = tid;
            if (permissions == null)
                IgnorePermission = true;
            else
                Permissions = permissions;
            Permissions = permissions;
        }
        public long Id { get; set; }
        public string LoginName { get; set; }
        public string UserImage { get; set; }
        public string NickName { get; set; }
        //public List<string> Permissions { get; set; }
        /// <summary>
        /// 租户Id
        /// </summary>
        public long? TenantId { get; }
        public List<string> Permissions { get; set; }
        public bool IgnorePermission { get; set; }
    }
}
