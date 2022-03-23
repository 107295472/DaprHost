using Client.ServerProxyFactory.Interface;
using Hangfire;
using IApplicationService.Base.AppEvent;
using IApplicationService.GoodsService.Dtos.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobService.CronJob.Goods
{
    public class WriteGoodsInElasticsearch : CronJobBase
    {
        public override void RunCornJob()
        {
            RecurringJob.AddOrUpdate<IEventBus>((bus) => bus.SendEvent(EventTopicDictionary.Goods.UpdateGoodsToEs, new UpdateGoodsToEsDto()), "*/5 * * * *");
        }
    }
}
