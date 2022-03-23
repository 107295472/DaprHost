﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.AccountService.Dtos.Input
{
    public class RoleCreateDto
    {
        [Required(ErrorMessage = "请填写角色名")]
        [MaxLength(8, ErrorMessage = "角色名太长了")]
        public string Name { get; set; }
        //public List<long> Permissions { get; set; }
    }
}
