using DomainBase;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mongo.Entity
{
    public class EventLog_1 : MongoEntity
    {

        public long UserID { get; set; }
        public long AccountID { get; set; }
        public string Remark { get; set; }
    }
}
