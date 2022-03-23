using Client.ServerSymbol.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.Base.AccessToken
{
    public class AccountLoginCache : StateStore
    {
        public AccountLoginCache(long key, object data)
        {
            Key = $"UserAccountInfo_{key}";
            this.Data = data;
            TtlInSeconds = 604800;
        }
        public AccountLoginCache(long key)
        {
            Key = $"UserAccountInfo_{key}";
        }
        public override string Key { get; set; }
        public override object Data { get; set; }
    }
}
