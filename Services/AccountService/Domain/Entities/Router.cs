using DomainBase;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// 路由表
    /// </summary>
    [Table(Name = "Router")]
    public class Router : Entity, IAggregateRoot
    {
        [Column(StringLength =20)]
        public string Path { get; set; }
        [Column(StringLength = 20)]
        public string Name { get; set; }
        [Column(StringLength = 50)]
        public string Redirect { get; set; }
        [Column(StringLength = 30)]
        public string Title { get; set; }
        [Column(StringLength = 100)]
        public string Icon { get; set; }
        [Column(StringLength = 100)]
        public string Component { get; set; }
        public long Parent { get; set; }
        public int OrderNo { get; set; }
        public bool Status { get; set; }



        [Column(IsIgnore =true)]
        public List<Router> Children { get; set; }
        [Column(IsIgnore = true)]
        public Meta Meta { get; set; }
    }
    public class Meta
    {
        public string Title { get; set; }
        public string Icon { get; set; }
    }
}
