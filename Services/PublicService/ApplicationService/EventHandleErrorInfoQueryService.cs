using Domain.Entities;
using IApplicationService.PublicService;

using InfrastructureBase.AuthBase;
using InfrastructureBase.Object;
using System.Threading.Tasks;
using IApplicationService.Base.AppQuery;
using System.Linq;
using InfrastructureBase.Data;
using IApplicationService.Base;
using Client.ServerProxyFactory.Interface;

namespace ApplicationService
{
    public class EventHandleErrorInfoQueryService : IEventHandleErrorInfoQueryService
    {
        private readonly IStateManager stateManager;
        public EventHandleErrorInfoQueryService(IStateManager stateManager)
        {
            this.stateManager = stateManager;
        }
		
        [AuthenticationFilter]
        public async Task<ApiResult> GetEventHandleErrorInfoList(PageQueryInputBase input)
        {
            return ApiResult.Ok(new PageQueryResonseBase<EventHandleErrorInfo>(null, 11));
        }

        public async Task<ApiResult> PubTest()
        {
            var ip = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces().Select(p => p.GetIPProperties()).SelectMany(p => p.UnicastAddresses)
                  .Where(p => p.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !System.Net.IPAddress.IsLoopback(p.Address))
                  .FirstOrDefault()?.Address.ToString();
            return await ApiResult.Ok($"主机---{ip}").Async();
        }
    }
}
