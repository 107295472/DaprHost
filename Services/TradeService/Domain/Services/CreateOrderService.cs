﻿using Domain.Dtos;
using Domain.Entities;
using Domain.ValueObject;
using DomainBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public partial class CreateOrderService
    {
        Func<IEnumerable<long>, Task<List<OrderGoodsSnapshot>>> getGoodsList;
        Func<CreateOrderDeductionGoodsStockDto, Task<bool>> deductionGoodsStock;
        Func<CreateOrderDeductionGoodsStockDto, Task<bool>> undeductionGoodsStock;
        List<CreateOrderDeductionGoodsStockDto> succGoodsIds = new List<CreateOrderDeductionGoodsStockDto>();//成功扣库存容器
        public CreateOrderService(
            Func<IEnumerable<long>, Task<List<OrderGoodsSnapshot>>> getGoodsList,
            Func<CreateOrderDeductionGoodsStockDto, Task<bool>> deductionGoodsStock,
            Func<CreateOrderDeductionGoodsStockDto, Task<bool>> undeductionGoodsStock)
        {
            this.getGoodsList = getGoodsList;
            this.deductionGoodsStock = deductionGoodsStock;
            this.undeductionGoodsStock = undeductionGoodsStock;
        }
        public async Task<Order> CreateOrder(long userId, string consigneeName,List<OrderItem> orderItems)
        {
            var order = new Order();
            if (orderItems == null || !orderItems.Any())
                throw new DomainException("订单明细不能为空!");
            orderItems = orderItems.GroupBy(x => x.GoodsId).Select(x => new OrderItem() { GoodsId = x.Key, Count = x.Sum(y => y.Count) }).ToList();
            //rpc获取商品基本信息
            var goodslist = await getGoodsList(orderItems.Select(x => x.GoodsId));
            //填充订单明细
            foreach (var item in orderItems)
            {
                var goods = goodslist.FirstOrDefault(x => x.GoodsId == item.GoodsId);
                if (goods == null)
                    throw new DomainException($"订单创建失败,商品不存在或已下架!");
                var deductstock = new CreateOrderDeductionGoodsStockDto(item.GoodsId, item.Count);
                if (!await deductionGoodsStock(deductstock))
                    throw new DomainException($"订单创建失败,商品{goods.GoodsName}库存不足!");
                else
                    succGoodsIds.Add(deductstock);
                item.Create(order.Id, goods);
            }
            //创建订单实体
            order.CreateOrder(userId, consigneeName,"地址","电话", orderItems);
            return order;
        }
        public async Task UnCreateOrder()
        {
            foreach (var deductstock in succGoodsIds)
            {
                await undeductionGoodsStock(deductstock);
            }
        }
    }
    public partial class CreateOrderService
    {
        public CreateOrderService(
           Func<IEnumerable<long>, Task<List<OrderGoodsSnapshot>>> getGoodsList)
        {
            this.getGoodsList = getGoodsList;
        }
        public async Task<Order> FinalCreateOrder(long userId, string consigneeName, string consigneeAddress, string consigneeTel, List<OrderItem> orderItems)
        {
            var order = new Order();
            if (orderItems == null || !orderItems.Any())
                throw new DomainException("订单明细不能为空!");
            orderItems = orderItems.GroupBy(x => x.GoodsId).Select(x => new OrderItem() { GoodsId = x.Key, Count = x.Sum(y => y.Count) }).ToList();
            //rpc获取商品基本信息
            var goodslist = await getGoodsList(orderItems.Select(x => x.GoodsId));
            //填充订单明细
            foreach (var item in orderItems)
            {
                var goods = goodslist.FirstOrDefault(x => x.GoodsId == item.GoodsId);
                //由于这里不使用actor，所以校验工作在saga的上一个流程已经处理完毕了，此处仅需要进行实体明细填充
                item.Create(order.Id, goods);
            }
            //创建订单实体
            order.CreateOrder(userId, consigneeName, consigneeAddress, consigneeTel, orderItems);
            return order;
        }
    }
}
