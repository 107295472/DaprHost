﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.Base
{
    public class ApiResult
    {
        public ApiResult()
        {

        }
        public ApiResult(string message = null, int code = 0, object data = null)
        {
            if (message != null)
                Message = message;
            Code = code;
            if (code != 0)
            {
                Success = false;
            }
            else
            {
                Success = true;
            }
            if (data != null)
                Result = data;
        }
        public int Code { get; set; }
        [JsonIgnore]
        public bool Success { get; private set; } = true;
        public string Message { get; set; }
        public object Result { get; set; }
        internal object resultType { get; set; }

        public static ApiResult Ok(string message = null, int code = 0)
        {
            return new ApiResult(message ?? "操作成功", code);
        }

        public static ApiResult Ok(object Data, string message = null, int code = 0)
        {
            return new ApiResult(message ?? "操作成功", code, Data);
        }
        public static ApiResult<T> Ok<T>(Task<T> Data, string message = null, int code = 0)
        {
            return new ApiResult<T>(message ?? "操作成功", code, Data);
        }
        public static ApiResult Err(string message = null, int code = -1)
        {
            return new ApiResult(message ?? "出错了,请稍后再试", code);
        }
    }
    public class ApiResult<T> : ApiResult
    {
        public ApiResult(string message = null, int code = 0, Task<T> data = null)
        {
            if (message != null)
                Message = message;
            if (code != 0)
                Code = code;
            if (data != null)
                TaskData = data;
        }
        [NotMapped]
        public Task<T> TaskData { get; set; }
    }
}
