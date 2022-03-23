using AgileConfig.Client;
using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using InfrastructureBase.Helpers;
using InfrastructureBase.Cache;

namespace InfrastructureBase
{
   public  class App
    {
        public static ConfigClient Client;
    }
    public class AppConfig
    {
        public static bool Tenant { get { return App.Client[$"{nameof(DbConfig)}:{ nameof(Tenant)}"].ToBool(); } }
        public static CacheType CacheType { get { return (CacheType)Enum.Parse(typeof(CacheType), App.Client[$"{nameof(AppConfig)}:{ nameof(CacheType)}"]); } }
        public static string RedisConnStr { get { return App.Client[$"{nameof(DbConfig)}:{ nameof(RedisConnStr)}"]; } }
    }
    /// <summary>
    /// 数据库配置
    /// </summary>
    public static class DbConfig
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public static DataType DataType { get {

                return (DataType)Enum.Parse(typeof(DataType),App.Client[$"{nameof(DbConfig)}:{ nameof(DataType)}"]);
            
            } }

        /// <summary>
        /// 数据库字符串
        /// </summary>
        public static string ConnectionString { get {
                return App.Client[$"{nameof(DbConfig)}:{ nameof(ConnectionString)}"];
            }}

        /// <summary>
        /// 空闲时间(分)
        /// </summary>
        public static int IdleTime { get; set; } = 10;

        /// <summary>
        /// 生成数据
        /// </summary>
        public static bool GenerateData { get; set; } = false;

        /// <summary>
        /// 同步结构
        /// </summary>
        public static bool SyncStructure { get; set; } = false;

        /// <summary>
        /// 同步数据
        /// </summary>
        public static bool SyncData { get; set; } = false;

        /// <summary>
        /// 建库
        /// </summary>
        public static bool CreateDb { get; set; } = false;

        /// <summary>
        /// 建库连接字符串
        /// </summary>
        public static string CreateDbConnectionString { get; set; }

        /// <summary>
        /// 建库脚本
        /// </summary>
        public static string CreateDbSql { get; set; }

        /// <summary>
        /// 监听所有操作
        /// </summary>
        public static bool MonitorCommand { get { return App.Client[$"{nameof(DbConfig)}:{ nameof(MonitorCommand)}"].ToBool(); } }

        /// <summary>
        /// 监听Curd操作
        /// </summary>
        public static bool Curd { get { return App.Client[$"{nameof(DbConfig)}:{ nameof(Curd)}"].ToBool(); } }
    }
}
