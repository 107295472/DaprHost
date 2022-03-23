using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.AccountService.Dtos.Output
{
    public class AllPermissionResponse
    {
        public long Id { get; set; }
        public string Label { get; set; }
        public IEnumerable<AllPermissionResponse> Child { get; set; }
    }
}
