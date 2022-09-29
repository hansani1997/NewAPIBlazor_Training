﻿using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.Transaction;
using BlueLotus360.Core.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Web.APIApplication.Definitions.ServiceDefinitions
{
    public interface ITransactionService
    {
       BaseServerResponse<BLTransaction> SaveTransaction(BLTransaction transaction,Company company,User user,UIObject uIObject);

    }
}
