using Domain.Entities;
using DomainBase;
using FreeSql;
using FreeSql.Aop;
using FreeSql.DataAnnotations;
using InfrastructureBase.Attributes;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Extensions;
using InfrastructureBase.Helpers;
using InfrastructureBase.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Yitter.IdGenerator;

namespace InfrastructureBase.Data
{
    public class DbHelper
    {
        /// <summary>
        /// 偏移时间
        /// </summary>
        public static TimeSpan TimeOffset;


        /// <summary>
        /// 获得指定程序集表实体
        /// </summary>
        /// <returns></returns>
        public static Type[] GetEntityTypes()
        {
            List<string> assemblyNames = new List<string>()
            {
                "Domain.Entities"
            };

            List<Type> entityTypes = new List<Type>();

            foreach (var assemblyName in assemblyNames)
            {
                foreach (Type type in Assembly.Load(assemblyName).GetExportedTypes())
                {
                    foreach (Attribute attribute in type.GetCustomAttributes())
                    {
                        if (attribute is TableAttribute tableAttribute)
                        {
                            if (tableAttribute.DisableSyncStructure == false)
                            {
                                entityTypes.Add(type);
                            }
                        }
                    }
                }
            }

            return entityTypes.ToArray();
        }

        /// <summary>
        /// 配置实体
        /// </summary>
        public static void ConfigEntity(IFreeSql db)
        {
            //租户生成和操作租户Id
            if (true)
            {
                var iTenant = nameof(ITenant);
                var tenantId = nameof(ITenant.TenantId);

                //获得指定程序集表实体
                var entityTypes = GetEntityTypes();

                foreach (var entityType in entityTypes)
                {
                    if (entityType.GetInterfaces().Any(a => a.Name == iTenant))
                    {
                        db.CodeFirst.Entity(entityType, a =>
                        {
                            a.Ignore(tenantId);
                        });
                    }
                }
            }
        }

        /// <summary>
        /// 审计数据
        /// </summary>
        /// <param name="e"></param>
        /// <param name="user"></param>
        /// <param name="timeOffset"></param>
        public static void AuditValue(AuditValueEventArgs e, TimeSpan timeOffset, CurrentUser user)
        {
            if (e.Property.GetCustomAttribute<ServerTimeAttribute>(false) != null
                   && (e.Column.CsType == typeof(DateTime) || e.Column.CsType == typeof(DateTime?))
                   && (e.Value == null || (DateTime)e.Value == default || (DateTime?)e.Value == default))
            {
                e.Value = DateTime.Now.Subtract(timeOffset);
            }

            //if (e.Column.CsType == typeof(long)
            //&& e.Property.GetCustomAttribute<SnowflakeAttribute>(false) is SnowflakeAttribute snowflakeAttribute
            //&& snowflakeAttribute.Enable && (e.Value == null || (long)e.Value == default || (long?)e.Value == default))
            //{
            //    e.Value = YitIdHelper.NextId();
            //}

            if (user == null || user.Id <= 0)
            {
                return;
            }

            if (e.AuditValueType == AuditValueType.Insert)
            {
                switch (e.Property.Name)
                {
                    case "CreatedUserId":
                        if (e.Value == null || (long)e.Value == default || (long?)e.Value == default)
                        {
                            e.Value = user.Id;
                        }
                        break;

                    case "CreatedUserName":
                        if (e.Value == null || ((string)e.Value).IsNull())
                        {
                            e.Value = user.LoginName;
                        }
                        break;

                    case "TenantId":
                        if (e.Value == null || (long)e.Value == default || (long?)e.Value == default)
                        {
                            e.Value = user.TenantId;
                        }
                        break;
                }
            }
            else if (e.AuditValueType == AuditValueType.Update)
            {
                switch (e.Property.Name)
                {
                    case "ModifiedUserId":
                        e.Value = user.Id;
                        break;

                    case "ModifiedUserName":
                        e.Value = user.LoginName;
                        break;
                }
            }
        }

        /// <summary>
        /// 同步结构
        /// </summary>
        public static void SyncStructure(IFreeSql db, string msg = null)
        {
            //打印结构比对脚本
            //var dDL = db.CodeFirst.GetComparisonDDLStatements<PermissionEntity>();
            //Console.WriteLine("\r\n " + dDL);

            //打印结构同步脚本
            //db.Aop.SyncStructureAfter += (s, e) =>
            //{
            //    if (e.Sql.NotNull())
            //    {
            //        Console.WriteLine(" sync structure sql:\n" + e.Sql);
            //    }
            //};

            //获得指定程序集表实体
            var entityTypes = GetEntityTypes();

            db.CodeFirst.SyncStructure(entityTypes);
            Console.WriteLine($" {(msg.NotNull() ? msg : $"sync structure")} succeed");
        }

        /// <summary>
        /// 检查实体属性是否为自增长
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static bool CheckIdentity<T>() where T : class
        {
            var isIdentity = false;
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                if (property.GetCustomAttributes(typeof(ColumnAttribute), false).FirstOrDefault() is ColumnAttribute columnAttribute && columnAttribute.IsIdentity)
                {
                    isIdentity = true;
                    break;
                }
            }

            return isIdentity;
        }

        /// <summary>
        /// 初始化数据表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="tran"></param>
        /// <param name="data"></param>
        /// <param name="dbConfig"></param>
        /// <returns></returns>
        private static async Task InitDtDataAsync<T>(
            IFreeSql db,
            IUnitOfWork unitOfWork,
            System.Data.Common.DbTransaction tran,
            T[] data
        ) where T : class, new()
        {
            var table = typeof(T).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault() as TableAttribute;
            var tableName = table.Name;

            try
            {
                if (await db.Queryable<T>().AnyAsync())
                {
                    Console.WriteLine($" table: {tableName} record already exists");
                    return;
                }

                if (!(data?.Length > 0))
                {
                    Console.WriteLine($" table: {tableName} import data []");
                    return;
                }

                var repo = db.GetRepositoryBase<T>();
                var insert = db.Insert<T>();
                if (unitOfWork != null)
                {
                    repo.UnitOfWork = unitOfWork;
                    insert = insert.WithTransaction(tran);
                }

                //var isIdentity = CheckIdentity<T>();
                //if (isIdentity)
                //{
                //    if (dbConfig.Type == DataType.SqlServer)
                //    {
                //        var insrtSql = insert.AppendData(data).InsertIdentity().ToSql();
                //        await repo.Orm.Ado.ExecuteNonQueryAsync($"SET IDENTITY_INSERT {tableName} ON\n {insrtSql} \nSET IDENTITY_INSERT {tableName} OFF");
                //    }
                //    else
                //    {
                //        await insert.AppendData(data).InsertIdentity().ExecuteAffrowsAsync();
                //    }
                //}
                //else
                //{
                //    repo.DbContextOptions.EnableAddOrUpdateNavigateList = true;
                //    await repo.InsertAsync(data);
                //}
                repo.DbContextOptions.EnableAddOrUpdateNavigateList = true;
                await repo.InsertAsync(data);
                Console.WriteLine($" table: {tableName} sync data succeed");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($" table: {tableName} sync data failed.\n{ex.Message}");
            }
        }

        /// <summary>
        /// 同步数据审计方法
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private static void SyncDataAuditValue(object s, AuditValueEventArgs e)
        {
            var user = new { Id = 161223411986501, Name = "admin", TenantId = 161223412138053 };

            if (e.Property.GetCustomAttribute<ServerTimeAttribute>(false) != null
                   && (e.Column.CsType == typeof(DateTime) || e.Column.CsType == typeof(DateTime?))
                   && (e.Value == null || (DateTime)e.Value == default || (DateTime?)e.Value == default))
            {
                e.Value = DateTime.Now.Subtract(TimeOffset);
            }

            //if (e.Column.CsType == typeof(long)
            //&& e.Property.GetCustomAttribute<SnowflakeAttribute>(false) != null
            //&& (e.Value == null || (long)e.Value == default || (long?)e.Value == default))
            //{
            //    e.Value = YitIdHelper.NextId();
            //}

            if (user == null || user.Id <= 0)
            {
                return;
            }

            if (e.AuditValueType == AuditValueType.Insert)
            {
                switch (e.Property.Name)
                {
                    case "CreatedUserId":
                        if (e.Value == null || (long)e.Value == default || (long?)e.Value == default)
                        {
                            e.Value = user.Id;
                        }
                        break;

                    case "CreatedUserName":
                        if (e.Value == null || ((string)e.Value).IsNull())
                        {
                            e.Value = user.Name;
                        }
                        break;

                    case "TenantId":
                        if (e.Value == null || (long)e.Value == default || (long?)e.Value == default)
                        {
                            e.Value = user.TenantId;
                        }
                        break;
                }
            }
            else if (e.AuditValueType == AuditValueType.Update)
            {
                switch (e.Property.Name)
                {
                    case "ModifiedUserId":
                        e.Value = user.Id;
                        break;

                    case "ModifiedUserName":
                        e.Value = user.Name;
                        break;
                }
            }
        }

    }
}