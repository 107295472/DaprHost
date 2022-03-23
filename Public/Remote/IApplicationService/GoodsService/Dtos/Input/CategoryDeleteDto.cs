﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.GoodsService.Dtos.Input
{
    public class CategoryDeleteDto
    {
        [Required(ErrorMessage ="请选择一个商品类型")]
        public long Id { get; set; }
    }
}
