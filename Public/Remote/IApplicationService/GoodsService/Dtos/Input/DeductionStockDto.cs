using Client.ServerSymbol.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.GoodsService.Dtos.Input
{
    public class DeductionStockDto : ActorSendDto
    {
        public long GoodsId { get; set; }
        public int DeductionStock { get; set; }
        public override string ActorId { get; set; }
    }
}
