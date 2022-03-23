using System;
using System.Linq;
using System.Threading.Tasks;
using IApplicationService.PublicService;
using IApplicationService.Base.AppQuery;

using InfrastructureBase.AuthBase;
using InfrastructureBase.Data;

using IApplicationService.PublicService.Dtos.Input;
using InfrastructureBase;
using Domain.Entities;
using MallSetting = Domain.Entities.MallSetting;
using IApplicationService.Base;
using Client.ServerProxyFactory.Interface;

namespace ApplicationService
{
    public class MallSettingUseCaseService : IMallSettingUseCaseService
    {
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        public MallSettingUseCaseService(IEventBus eventBus, IStateManager stateManager)
        {
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }

        [AuthenticationFilter]
        public async Task<ApiResult> CreateOrUpdateMallSetting(CreateOrUpdateMallSettingDto input)
        {
            return ApiResult.Ok();
        }
    }
}
