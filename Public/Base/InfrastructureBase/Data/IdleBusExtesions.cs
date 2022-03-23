using FreeSql;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace InfrastructureBase.Data
{
    public static class IdleBusExtesions
    {

        /// <summary>
        /// 获得FreeSql实例
        /// </summary>
        /// <param name="ib"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static IFreeSql GetFreeSql(this IdleBus<IFreeSql> ib, IServiceProvider serviceProvider)
        {
            var freeSql = serviceProvider.GetRequiredService<IFreeSql>();
            return freeSql;
        }
    }
}