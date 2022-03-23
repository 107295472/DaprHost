﻿using Domain.Enums;
using DomainBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// 物流
    /// </summary>
    public class Logistics : Entity, IAggregateRoot
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public long OrderId { get; set; }
        /// <summary>
        /// 物流渠道
        /// </summary>
        public LogisticsType LogisticsType { get; set; }
        /// <summary>
        /// 物流回执号
        /// </summary>
        public string LogisticsNo { get; set; }
        /// <summary>
        /// 寄件人姓名
        /// </summary>
        public string DeliverName { get; set; }
        /// <summary>
        /// 寄件人地址
        /// </summary>
        public string DeliverAddress { get; set; }
        /// <summary>
        /// 寄件操作人
        /// </summary>
        public long DeliverUserId { get; set; }
        /// <summary>
        /// 收件人姓名
        /// </summary>
        public string ReceiverName { get; set; }
        /// <summary>
        /// 收件人地址
        /// </summary>
        public string ReceiverAddress { get; set; }
        /// <summary>
        /// 收件操作人
        /// </summary>
        public long ReceiverUserId { get; set; }
        /// <summary>
        /// 寄件时间
        /// </summary>
        public DateTime DeliveTime { get; set; }
        /// <summary>
        /// 收件时间
        /// </summary>
        public DateTime? ReceiveTime { get; set; }
        /// <summary>
        /// 物流状态
        /// </summary>
        public LogisticsState LogisticsState { get; set; }
        /// <summary>
        /// 物流发货
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="logisticsType"></param>
        /// <param name="logisticsNo"></param>
        /// <param name="deliverName"></param>
        /// <param name="deliverAddress"></param>
        /// <param name="receiverName"></param>
        /// <param name="receiverAddress"></param>
        /// <param name="deliveTime"></param>
        public void Deliver(long orderId, LogisticsType logisticsType, string logisticsNo, string deliverName, string deliverAddress, long deliverUserId, string receiverName, string receiverAddress, DateTime? deliveTime)
        {
            OrderId = orderId;
            LogisticsType = logisticsType;
            LogisticsNo = logisticsNo;
            if (string.IsNullOrEmpty(deliverName))
                throw new DomainException("发货人无效!");
            if (string.IsNullOrEmpty(deliverAddress))
                throw new DomainException("发货地址未填写!");
            DeliverUserId = deliverUserId;
            DeliverName = deliverName;
            DeliverAddress = deliverAddress;
            ReceiverName = receiverName;
            ReceiverAddress = receiverAddress;
            if (deliveTime != null && deliveTime.Value >= DateTime.Now)
                throw new DomainException("发货时间不能晚于现在!");
            DeliveTime = deliveTime ?? DateTime.Now;
            LogisticsState = LogisticsState.DeliverGoods;
        }
        /// <summary>
        /// 物流收货
        /// </summary>
        /// <param name="receiveTime"></param>
        public void Receive(long receiverUserId, DateTime? receiveTime)
        {
            if (LogisticsState == LogisticsState.DeliverGoods)
            {
                LogisticsState = LogisticsState.ReceivingGoods;
                ReceiverUserId = receiverUserId;
                if (receiveTime != null && receiveTime.Value >= DateTime.Now)
                    throw new DomainException("确认收货时间不能晚于现在!");
                ReceiveTime = receiveTime ?? DateTime.Now;
            }
            else
                throw new DomainException("当前状态无法进行收货操作");
        }
    }
}
