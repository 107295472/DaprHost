using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.Base.AccessToken
{
    public class AccessTokenItem
    {
        public AccessTokenItem() { }
        public AccessTokenItem(long id) { Id = id; }
        public long Id { get; set; }
    }
}
