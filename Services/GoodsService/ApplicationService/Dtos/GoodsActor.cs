﻿using InfrastructureBase;
using Mesh.Dapr.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.Dtos
{
    public record GoodsActor : ActorStateModel
    {
        public GoodsActor()
        {
            AutoSave = true;
        }
        public long Id { get; set; }
        public int Stock { get; set; }
        public void ChangeStock(int stock)
        {
            if (stock < 0)
                throw new ApplicationServiceException("库存不能为0!");
            else
                Stock = stock;
        }
        public void DeductionStock(int stock)
        {
            if (stock < 0)
                throw new ApplicationServiceException("库存不能为0!");
            if (Stock < stock)
                throw new ApplicationServiceException("库存不足!");
            else
                Stock -= stock;
        }
        public void UnDeductionStock(int stock)
        {
            Stock += stock;
        }
        public override bool AutoSave { get; set; }
        /// <summary>
        /// 实时同步
        /// </summary>
        public override int ReminderSeconds => 0;
    }
}
