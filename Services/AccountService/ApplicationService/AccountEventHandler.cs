using ApplicationService.Dtos;
using Client.ServerProxyFactory.Interface;
using Client.ServerSymbol.Events;
using Domain.Entities;
using Domain.Enums;
using Domain.Repository;
using Domain.Specification;
using FreeSql;
using IApplicationService.AccountService.Dtos;
using IApplicationService.AccountService.Dtos.Event;
using IApplicationService.AccountService.Dtos.Input;
using IApplicationService.Base.AccessToken;
using IApplicationService.Base.AppEvent;
using InfrastructureBase;
using InfrastructureBase.Object;
using Server.Kestrel.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService
{
    public class AccountEventHandler : IEventHandler
    {
        private readonly IEventBus eventBus;
        private readonly ILocalEventBus localEventBus;
        private readonly IStateManager stateManager;
        private readonly IUserRepository repo;
        public AccountEventHandler(IUserRepository re,IEventBus eventBus, ILocalEventBus localEventBus, IStateManager stateManager)
        {
            this.repo = re;
            this.eventBus = eventBus;
            this.localEventBus = localEventBus;
            this.stateManager = stateManager;
        }
        [EventHandlerFunc(EventTopicDictionary.Account.LoginExpire)]
        public async Task<DefaultEventHandlerResponse> LoginCacheExpire(EventHandleRequest<LoginSuccessDto> input)
        {
            //Console.WriteLine("************退出***************");
            return await new DefaultEventHandlerResponse().RunAsync(nameof(LoginCacheExpire), input.GetDataJson(), async () =>
            {
                await stateManager.DelState(new AccountLoginAccessToken(input.GetData().Token));
            });
        }

    }
}
