using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
    public class OperateOrderSuccessEvent
    {
        public OperateOrderSuccessEvent(Order order, string name)
        {
            OrderId = order.Id;
            UserName = name;
            UserId = order.UserId;
        }
        public long OrderId { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
    }
}
