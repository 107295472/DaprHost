using Client.ServerSymbol;
using Client.ServerSymbol.Events;
using IApplicationService.Base;
using IApplicationService.GoodsService.Dtos.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.GoodsService
{
    [RemoteService("goodsservice", "goodsactor")]
    public interface IGoodsActorService : IActorService
    {
        [RemoteFunc(FuncType.Actor)]
        Task<ApiResult> UpdateGoodsStock(DeductionStockDto input);
        [RemoteFunc(FuncType.Actor)]
        Task<ApiResult> UnDeductionGoodsStock(DeductionStockDto input);
        [RemoteFunc(FuncType.Actor)]
        Task<ApiResult> DeductionGoodsStock(DeductionStockDto input);
    }
}
