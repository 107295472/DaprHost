using Domain.Enums;
using DomainBase;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table(Name = "Permission")]
    [Index("idx_{tablename}_01", nameof(ParentId) + "," + nameof(Label), true)]
    public class Permission : Entity, IAggregateRoot
    {
        public long ParentId { get; set; }
        [Navigate(nameof(ParentId))]
        public List<Permission> Childs { get; set; }
        /// <summary>
        /// 权限名
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// 接口地址
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 权限编码
        /// </summary>
        [Column(StringLength = 550)]
        public string Code { get; set; }

        /// <summary>
        /// 权限类型
        /// </summary>
        [Column(MapType = typeof(int), CanUpdate = false)]
        public PermissionType Type { get; set; } = PermissionType.Dot;
        /// <summary>
        /// 是否检查
        /// </summary>
        public bool CheckPermission { get; set; } = true;
        /// <summary>
        /// 视图
        /// </summary>
        public long? ViewId { get; set; }

        public ViewEntity View { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        [Column(StringLength = 100)]
        public string Icon { get; set; }

        /// <summary>
        /// 隐藏
        /// </summary>
		public bool Hidden { get; set; } = false;

        /// <summary>
        /// 启用
        /// </summary>
		public bool Enabled { get; set; } = true;

        /// <summary>
        /// 可关闭
        /// </summary>
        public bool? Closable { get; set; }

        /// <summary>
        /// 打开组
        /// </summary>
        public bool? Opened { get; set; }

        /// <summary>
        /// 打开新窗口
        /// </summary>
        public bool? NewWindow { get; set; }

        /// <summary>
        /// 链接外显
        /// </summary>
        public bool? External { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; set; } = 0;
        /// <summary>
        /// 描述
        /// </summary>
        [Column(StringLength = 100)]
        public string Description { get; set; }
    }
}
