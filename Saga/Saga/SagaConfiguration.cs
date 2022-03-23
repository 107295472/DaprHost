﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga
{
    /// <summary>
    /// 流程配置
    /// </summary>
    public class SagaConfiguration
    {
        public SagaConfiguration() { }
        public SagaConfiguration(string clusterName, string serviceName, string messageQueueConnectionString, string storeConnectionString, params TopicConfiguration[] dictionary)
        {
            this.ClusterName = clusterName;
            this.ServiceName = serviceName;
            this.MessageQueueConnectionString = messageQueueConnectionString;
            this.StoreConnectionString = storeConnectionString;
            this.AllTopicLinkedDictionary = dictionary.ToList();

        }
        /// <summary>
        /// 集群名称,用于注册集群交换机
        /// </summary>
        public string ClusterName { get; set; } = "Default";
        /// <summary>
        /// 服务名，同于订阅频道
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 消息队列连接字符串
        /// </summary>
        public string MessageQueueConnectionString { get; set; }
        /// <summary>
        /// 持久化设备连接字符串
        /// </summary>
        public string StoreConnectionString { get; set; }

        public List<TopicConfiguration> AllTopicLinkedDictionary { get; set; }

        public string GetflowNameByTopic(string topic) => AllTopicLinkedDictionary.FirstOrDefault(x => x.ExistsByTopic(topic))?.FlowName;
        public TopicLinked GetNextByTopic(string flowName, string topic) => AllTopicLinkedDictionary.FirstOrDefault(x => x.FlowName == flowName).GetNext(topic);
        public TopicLinked GetPrevTopicByTopic(string flowName, string topic) => AllTopicLinkedDictionary.FirstOrDefault(x => x.FlowName == flowName).GetPrev(topic);
    }
}
