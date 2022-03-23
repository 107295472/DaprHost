using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.GoodsService.Dtos.Output
{
    public class GetLimitedTimeActivitieListResponse
    {
        public long Id { get; set; }
        public long GoodsId { get; set; }
        public string ActivitieName { get; set; }
        public string GoodsName { get; set; }
        public decimal Price { get; set; }
        public decimal ActivitiePrice { get; set; }
        public int ActivitieState { get; set; }
        public string ActivitieStateInfo { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool ShelfState { get; set; }
    }
}
