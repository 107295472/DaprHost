﻿using Client.ServerSymbol.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.Base.AccessToken
{
    public class AccountLoginAccessToken : StateStore
    {
        public AccountLoginAccessToken(string key, AccessTokenItem data)
        {
            Key = $"UserAccountAccessToken_{key}";
            Data = data;
            TtlInSeconds = 604800;
        }
        public AccountLoginAccessToken(string key)
        {
            Key = $"UserAccountAccessToken_{key}";
        }
        public override string Key { get; set; }
        public override object Data { get; set; }
    }
}
