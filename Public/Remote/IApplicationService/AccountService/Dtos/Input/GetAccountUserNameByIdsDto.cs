using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.AccountService.Dtos.Input
{
    public class GetAccountUserNameByIdsDto
    {
        public List<long> Ids { get; set; }
    }
    public class AssignRoleDto
    {
        [Required(ErrorMessage = "用户ID不能为空")]
        public long AccountID { get; set; }
        [Required(ErrorMessage = "用户名不能为空")]
        public string LoginName { get; set; }
        [Required(ErrorMessage = "角色ID不能为空")]
        public List<long> RoleID { get; set; }
    }
}
