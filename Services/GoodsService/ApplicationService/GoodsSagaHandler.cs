using Domain.Dtos;
using Domain.Repository;
using Domain.Services;
using IApplicationService.GoodsService;
using IApplicationService.GoodsService.Dtos.Input;
using IApplicationService.Sagas.CreateOrder.Dtos;
using IApplicationService.Sagas.CreateOrder.Handles;
using InfrastructureBase;
using InfrastructureBase.AopFilter;
using InfrastructureBase.Object;
using Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeductionStockDto = IApplicationService.Sagas.CreateOrder.Dtos.DeductionStockDto;

namespace ApplicationService
{
    public class GoodsSagaHandler : IGoodsHandler
    {
        private readonly IGoodsRepository repository;
        public GoodsSagaHandler(IGoodsRepository repository)
        {
            this.repository = repository;
        }
        [Tran]
        public async Task<DeductionStockDto> DeductInventory(DeductionStockDto dto)
        {
            try
            {
                var goods = new BatchDeductInventoryService(
                    await repository.Select.Where(it=>dto.Items.Select(x => x.GoodsId).ToArray().Contains(it.Id)
                    
                    ).ToListAsync())
                        .BatchDeductInventory(dto.CopyTo<DeductionStockDto, Domain.Dtos.DeductionStockDto>());
                //goods.ForEach(x => repository.Update(x));
                repository.Update(goods);
                return dto;
            }
            catch (Exception e)
            {
                throw new SagaException<DeductionStockDto>(dto, e.Message);
            }
        }
        [Tran]
        public async Task InventoryRollback(DeductionStockDto dto)
        {
            try
            {
                var goods = new BatchRollbackDeductInventoryService(await repository.Select.Where(it => dto.Items.Select(x => x.GoodsId).ToArray().Contains(it.Id)
                ).ToListAsync())
                        .BatchRollbackDeductInventory(dto.CopyTo<DeductionStockDto, Domain.Dtos.DeductionStockDto>());
                repository.Update(goods);
            }
            catch (Exception e)
            {
                throw new SagaException<DeductionStockDto>(dto, e.Message);
            }
        }
    }
}
