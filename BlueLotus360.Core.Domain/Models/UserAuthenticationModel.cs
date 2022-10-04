﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Core.Domain.Models
{
    public class UserAuthenticationRequest
    {
        public string UserName  { get; set; }
        public string Password  { get; set; }
    }

    public class UserCompanyUpdateRequest
    {
        public int CompanyKey { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }

    }
}
