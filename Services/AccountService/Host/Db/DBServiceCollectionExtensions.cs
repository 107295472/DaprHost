using DomainBase;
using FreeSql;
using InfrastructureBase;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Data;
using InfrastructureBase.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Host.Db
{
    public static class DBServiceCollectionExtensions
    {
        /// <summary>
        /// 添加数据库
        /// </summary>
        /// <param name="services"></param>
        /// <param name="env"></param>
        public  static void AddDbAsync(this IServiceCollection services)
        {
            services.AddScoped<UnitOfWorkManager>();
            //if(dbConfig== null) throw new ArgumentNullException(nameof(dbConfig));

            #region FreeSql

            var freeSqlBuilder = new FreeSqlBuilder()
                    .UseConnectionString(DbConfig.DataType,DbConfig.ConnectionString)
                    .UseAutoSyncStructure(false)
                    .UseLazyLoading(false)
                    .UseNoneCommandParameter(true);

            #region 监听所有命令

            //if (DbConfig.MonitorCommand)
            //{
            //    freeSqlBuilder.UseMonitorCommand(cmd => { }, (cmd, traceLog) =>
            //    {
            //        //Console.WriteLine($"{cmd.CommandText}\n{traceLog}\r\n");
            //        Console.WriteLine($"{cmd.CommandText}\r\n");
            //    });
            //}

            #endregion 监听所有命令

            var fsql = freeSqlBuilder.Build();
            fsql.GlobalFilter.Apply<IEntitySoftDelete>("SoftDelete", a => a.IsDeleted == false);

            //DbHelper.ConfigEntity(fsql);

            #region 初始化数据库

            //同步结构
            if (DbConfig.SyncStructure)
            {
                DbHelper.SyncStructure(fsql);
            }

            CurrentUser? user = null;
            if (HttpContextExt.Current != null)
                user = HttpContextExt.Current.User;


            #region 审计数据

            //计算服务器时间
            var serverTime = fsql.Select<DualEntity>().Limit(1).First(a => DateTime.UtcNow);
            var timeOffset = DateTime.UtcNow.Subtract(serverTime);
            DbHelper.TimeOffset = timeOffset;
            fsql.Aop.AuditValue += (s, e) =>
            {
                DbHelper.AuditValue(e, timeOffset, user);
            };

            #endregion 审计数据
            #endregion 初始化数据库
            #region 监听Curd操作

            if (DbConfig.Curd)
            {
                fsql.Aop.CurdBefore += (s, e) =>
                {
                    Console.WriteLine($"{e.Sql}\r\n");
                };
            }

            #endregion 监听Curd操作

            if (AppConfig.Tenant)
            {
                fsql.GlobalFilter.Apply<ITenant>("Tenant", a => a.TenantId == user.TenantId);
            }

            #endregion FreeSql

            services.AddSingleton(fsql);
        }
    }
}