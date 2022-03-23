using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yitter.IdGenerator;

namespace DomainBase
{
    [Table(Name = "CasbinRule")]
    public class CasbinRule
    {
        public CasbinRule()
        {
            Id = YitIdHelper.NextId();
        }
        [Column(Position = -1, IsIdentity = false, IsPrimary = true)]
        public long Id { get; set; }
        [Column(StringLength =10)]
        public string Ptype { get; set; }
        [Column(StringLength = 256)]
        public string V0 { get; set; }
        [Column(StringLength = 256)]
        public string V1 { get; set; }
        [Column(StringLength = 256)]
        public string V2 { get; set; }
        [Column(StringLength = 256)]
        public string V3 { get; set; }
        [Column(StringLength = 256)]
        public string V4 { get; set; }
        [Column(StringLength = 256)]
        public string V5 { get; set; }
    }
}
