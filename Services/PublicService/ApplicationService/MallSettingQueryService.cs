using Domain.Entities;
using IApplicationService.PublicService;

using InfrastructureBase.AuthBase;
using InfrastructureBase.Object;
using System.Threading.Tasks;
using IApplicationService.Base.AppQuery;
using System.Linq;
using InfrastructureBase.Data;
using IApplicationService.PublicService.Dtos.Output;
using IApplicationService.Base;
using Client.ServerProxyFactory.Interface;

namespace ApplicationService
{
    public class MallSettingQueryService : IMallSettingQueryService
    {
        private readonly IStateManager stateManager;
        public MallSettingQueryService(IStateManager stateManager)
        {
            this.stateManager = stateManager;
        }

        public async Task<ApiResult> GetMallSetting()
        {
                return ApiResult.Ok();
        }
    }
}
