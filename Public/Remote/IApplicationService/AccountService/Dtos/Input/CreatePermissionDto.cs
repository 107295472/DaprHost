using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.AccountService.Dtos.Input
{
    public class CreatePermissionDto
    {
        //[Required(ErrorMessage = "服务名不能为空")]
        //public string ServerName { get; set; }

        //[Required(ErrorMessage = "权限名不能为空")]
        //public string PermissionName { get; set; }

        //[Required(ErrorMessage = "接口地址不能为空")]
        //public string Path { get; set; }
        [Required(ErrorMessage = "服务名不能为空")]
        public string SrvName { get; set; }
        [Required(ErrorMessage = "权限名不能为空")]
        public string FuncName { get; set; }
        public bool CheckPermission { get; set; }
        [Required(ErrorMessage = "接口地址不能为空")]
        public string Path { get; set; }
    }
    public class AddPermissionDto
    {
        [Required(ErrorMessage = "权限名不能为空")]
        public string PermissionName { get; set; }
        [Required(ErrorMessage = "接口地址不能为空")]
        public string Path { get; set; }
        public long PermissionID { get; set; } = 0;
        public string Code { get; set; }
        public string Type { get; set; }
        public long ViewID { get; set; }
        public string Desc { get; set; }
    }
    public class AssignPsermissionDto
    {
        [Required(ErrorMessage = "角色ID不能为空")]
        public long RoleID { get; set; }
        [Required(ErrorMessage = "权限ID不能为空")]
        public List<long> PermissionIds { get; set; }
    }
}
