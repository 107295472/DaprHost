using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoRepository.DAL;

namespace InfrastructureBase.Data
{
    public class Mongo
    {
        public static MongoRepository<T> GetRepository<T>(string tableName) where T : IEntity<string>
        {
           return new MongoRepository<T>("mongodb://192.168.10.22:27017/logdb", tableName);
        }
    }
}
