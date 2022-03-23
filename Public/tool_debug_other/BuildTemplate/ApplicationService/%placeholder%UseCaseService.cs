using System;
using System.Linq;
using System.Threading.Tasks;
using IApplicationService;
using IApplicationService.%ctxplaceholder%Service;
using IApplicationService.Base.AppQuery;
using InfrastructureBase.AuthBase;
using Client.ServerProxyFactory.Interface;
using InfrastructureBase.Data;
using Infrastructure.PersistenceObject;
using IApplicationService.%ctxplaceholder%Service.Dtos.Output;
using IApplicationService.%ctxplaceholder%Service.Dtos.Input;
using Domain.Repository;
using InfrastructureBase;
using Domain.Entities;

namespace ApplicationService
{
    public class %placeholder%UseCaseService : I%placeholder%UseCaseService
    {
        private readonly I%placeholder%Repo repo;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        public %placeholder%UseCaseService(I%placeholder%Repo repository, IEventBus eventBus, IStateManager stateManager)
        {
            this.repo = repository;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }
		
        [AuthenticationFilter]
        public async Task<ApiResult> Create%placeholder%(%placeholder%CreateDto input)
        {
            var entity = new %placeholder%();
            entity.CreateOrUpdate();
            repository.Add(entity);
            return ApiResult.Ok();
        }
		
        [AuthenticationFilter]
        public async Task<ApiResult> Update%placeholder%(%placeholder%UpdateDto input)
        {
            var entity = await repository.GetAsync(input.Id);
            if (entity == null)
                throw new ApplicationServiceException();
            entity.CreateOrUpdate();
            repository.Update(entity);
            return ApiResult.Ok();
        }
		
        [AuthenticationFilter]
        public async Task<ApiResult> Delete%placeholder%(%placeholder%DeleteDto input)
        {
            var entity = await repository.GetAsync(input.Id);
            if (entity == null)
                throw new ApplicationServiceException();
            repository.Delete(entity);
            return ApiResult.Ok();
        }
    }
}