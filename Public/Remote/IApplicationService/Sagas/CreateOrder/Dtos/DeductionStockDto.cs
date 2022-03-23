using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.Sagas.CreateOrder.Dtos
{
    public class DeductionStockDto
    {
        public List<GoodsInfo> Items { get; set; }
        public class GoodsInfo
        {
            public long GoodsId { get; set; }
            public int Count { get; set; }
        }
    }
}
