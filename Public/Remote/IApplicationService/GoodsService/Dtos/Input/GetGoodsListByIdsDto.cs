using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.GoodsService.Dtos.Input
{
    public class GetGoodsListByIdsDto
    {
        public GetGoodsListByIdsDto() { }
        public GetGoodsListByIdsDto(IEnumerable<long> ids)
        {
            this.Ids = ids.ToList();
        }
        public List<long> Ids { get; set; }
    }
}
