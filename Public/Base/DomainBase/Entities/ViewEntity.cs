using DomainBase;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;

namespace DomainBase.Entities
{
    /// <summary>
    /// 视图管理
    /// </summary>
	[Table(Name = "ad_view")]
    [Index("idx_{tablename}_01", nameof(ParentId) + "," + nameof(Label), true)]
    public class ViewEntity : Entity, IAggregateRoot
    {
        /// <summary>
        /// 所属节点
        /// </summary>
		public long ParentId { get; set; }

        [Navigate(nameof(ParentId))]
        public List<ViewEntity> Childs { get; set; }

        /// <summary>
        /// 视图命名
        /// </summary>
        [Column(StringLength = 50)]
        public string Name { get; set; }

        /// <summary>
        /// 视图名称
        /// </summary>
        [Column(StringLength = 500)]
        public string Label { get; set; }

        /// <summary>
        /// 视图路径
        /// </summary>
        [Column(StringLength = 500)]
        public string Path { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [Column(StringLength = 500)]
        public string Description { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 缓存
        /// </summary>
        public bool Cache { get; set; } = true;

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}