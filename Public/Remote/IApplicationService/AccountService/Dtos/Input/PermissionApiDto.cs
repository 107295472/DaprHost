using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.AccountService.Dtos.Input
{
    public class PermissionApiDto
    {
        public string key { get; set; }
        public DateTime? start { get; set; }
        public DateTime? end { get; set; }
    }
}
