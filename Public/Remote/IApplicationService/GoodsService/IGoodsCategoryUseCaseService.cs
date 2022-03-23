﻿using IApplicationService.Base;
using IApplicationService.GoodsService.Dtos.Input;
using Client.ServerSymbol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.GoodsService
{
    [RemoteService("goodsservice", "categoryusecase", "商品分类服务")]
    public interface IGoodsCategoryUseCaseService
    {
        [RemoteFunc(funcDescription: "创建商品分类")]
        Task<ApiResult> CreateCategory(CategoryCreateDto input);
        [RemoteFunc(funcDescription: "更新商品分类")]
        Task<ApiResult> UpdateCategory(CategoryUpdateDto input);
        [RemoteFunc(funcDescription: "删除商品分类")]
        Task<ApiResult> DeleteCategory(CategoryDeleteDto input);
    }
}
