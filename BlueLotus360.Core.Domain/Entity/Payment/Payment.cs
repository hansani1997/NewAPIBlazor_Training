using BlueLotus360.Core.Domain.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Core.Domain.Entity.Payment
{
    public class Payment
    {
    }

    public class AccPaymentMappingRequest
    {
        public CodeBaseResponse Location { get; set; }=new CodeBaseResponse();
        public CodeBaseResponse PayementTerm { get; set; }=new CodeBaseResponse();
        public bool LoadAll { get; set; }
        public long ELementKey { get; set; } = 1;

    }

    public class AccPaymentMappingResponse
    {
        public AccountResponse Account { get; set; }= new AccountResponse();    
        public CodeBaseResponse PayementMode { get; set; } = new CodeBaseResponse();    

        public AccPaymentMappingResponse()
        {
            Account = new AccountResponse();
            PayementMode = new CodeBaseResponse();
        }

    }
}
