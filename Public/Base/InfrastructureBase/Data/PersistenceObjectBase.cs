using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yitter.IdGenerator;

namespace InfrastructureBase.Data
{
    public abstract class PersistenceObjectBase
    {
        public PersistenceObjectBase()
        {
            Id = YitIdHelper.NextId();
        }
        /// <summary>
        /// key
        /// </summary>
        [Key]
        public long Id { get; set; }
    }
}
