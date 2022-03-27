using DomainBase;
using FreeSql.DataAnnotations;

namespace DomainBase.Entities.Personnel
{
    /// <summary>
    /// 员工附属部门
    /// </summary>
	[Table(Name = "ad_employee_organization")]
    [Index("idx_{tablename}_01", nameof(EmployeeId) + "," + nameof(OrganizationId), true)]
    public class EmployeeOrganizationEntity : EntityAdd
    {
        /// <summary>
        /// 员工Id
        /// </summary>
		public long EmployeeId { get; set; }

        /// <summary>
        /// 员工
        /// </summary>
        public EmployeeEntity Employee { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
		public long OrganizationId { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public OrganizationEntity Organization { get; set; }
    }
}