using Domain.Entities;
using Domain.Repository;
using IApplicationService;
using IApplicationService.%ctxplaceholder%Service;
using IApplicationService.%ctxplaceholder%Service.Dtos.Output;
using IApplicationService.%ctxplaceholder%Service.Dtos.Input;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Object;
using Client.ServerProxyFactory.Interface;
using System.Threading.Tasks;
using IApplicationService.Base.AppQuery;
using System.Linq;
using InfrastructureBase.Data;
using Microsoft.EntityFrameworkCore;

namespace ApplicationService
{
    public class %placeholder%QueryService : I%placeholder%QueryService
    {
        private readonly I%placeholder%Repo repo;
        private readonly IStateManager stateManager;
        public %placeholder%QueryService(I%placeholder%Repo dbContext, IStateManager stateManager)
        {
            this.repo = dbContext;
            this.stateManager = stateManager;
        }
		
        [AuthenticationFilter]
        public async Task<ApiResult> Get%placeholder%List(PageQueryInputBase input)
        {
             var Data = await repo.Select<%placeholder%>().Count(out var Total).Page(input.Page,input.Limit).ToListAsync<Get%placeholder%ListResponse>();
            return ApiResult.Ok(new PageQueryResonseBase<Get%placeholder%ListResponse>(Data, Total));
        }
		
    }
}
