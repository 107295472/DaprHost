using Client.ServerProxyFactory.Interface;
using Client.ServerSymbol.Events;
using Domain.Repository;

namespace ApplicationService
{
    public class RoleEventHandler : IEventHandler
    {
        private readonly IRoleRepository repository;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        public RoleEventHandler(IRoleRepository repository, IEventBus eventBus, IStateManager stateManager)
        {
            this.repository = repository;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }
    }
}
