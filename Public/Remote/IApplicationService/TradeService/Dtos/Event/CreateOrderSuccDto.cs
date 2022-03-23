using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.TradeService.Dtos.Event
{
    public class OperateOrderSuccDto
    {
        public long OrderId { get; set; }
        public string UserName { get; set; }
        public long UserId { get; set; }
    }
}
