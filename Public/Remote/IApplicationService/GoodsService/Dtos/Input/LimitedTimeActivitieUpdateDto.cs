﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.GoodsService.Dtos.Input
{
    public class LimitedTimeActivitieUpdateDto : LimitedTimeActivitieCreateDto
    {

        [Required(ErrorMessage = "请选择一个活动")]
        public Guid Id { get; set; }
    }
}
