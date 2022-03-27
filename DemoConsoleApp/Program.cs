using System;
using Yitter.IdGenerator;
using System.Threading.Tasks;
using FreeSql;
using System.Text;
using System.Linq;
using DomainBase.Entities.Personnel;

namespace DemoConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //RegGlobalID(0);
            //Console.WriteLine(YitIdHelper.NextId());
            var str="test32342wedfwsdfdsdf";
            Console.WriteLine(BytesTohexString(Encoding.UTF8.GetBytes(str)));
            Console.ReadLine();
        }
        public static void RegGlobalID(ushort workid)
        {
            // 创建 IdGeneratorOptions 对象，请在构造函数中输入 WorkerId：
            var options = new IdGeneratorOptions(workid);
            // options.WorkerIdBitLength = 10; // WorkerIdBitLength 默认值6，支持的 WorkerId 最大值为2^6-1，若 WorkerId 超过64，可设置更大的 WorkerIdBitLength
            // ...... 其它参数设置参考 IdGeneratorOptions 定义，一般来说，只要再设置 WorkerIdBitLength （决定 WorkerId 的最大值）。

            // 保存参数（必须的操作，否则以上设置都不能生效）：
            YitIdHelper.SetIdGenerator(options);
            // 以上初始化过程只需全局一次，且必须在第2步之前设置。
        }
        public static string BytesTohexString(byte[] bytes)
        {
            if (bytes == null || bytes.Count() < 1)
            {
                return string.Empty;
            }

            var count = bytes.Count();

            var cache = new StringBuilder();
            cache.Append("0x");
            for (int ii = 0; ii < count; ++ii)
            {
                var tempHex = Convert.ToString(bytes[ii], 16).ToUpper();
                cache.Append(tempHex.Length == 1 ? "0" + tempHex : tempHex);
            }

            return cache.ToString();
        }
        public static  void SyncTable()
        {
            IFreeSql fsql = new FreeSqlBuilder()
                   .UseConnectionString(DataType.SqlServer, "User ID=sa;Password=sql2008Ss;Host=8.142.117.37;Port=1433;Database=master;Pooling=true;")
                   .UseAutoSyncStructure(false) //自动同步实体结构到数据库，FreeSql不会扫描程序集，只有CRUD时才会生成表。
                   .Build(); //请务必定义成 Singleton 单例模式
            fsql.CodeFirst.SyncStructure(typeof(EmployeeEntity),nameof(EmployeeEntity));
        }
    }
}
