using Domain.Repository;
using Client.ServerProxyFactory.Interface;
using Client.ServerSymbol.Events;

namespace ApplicationService
{
    public class %placeholder%EventHandler : IEventHandler
    {
        private readonly I%placeholder%Repo repo;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        public %placeholder%EventHandler(I%placeholder%Repo repository, IEventBus eventBus, IStateManager stateManager)
        {
            this.repo = repository;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }
    }
}
