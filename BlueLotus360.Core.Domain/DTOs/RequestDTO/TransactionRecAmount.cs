using BlueLotus360.Core.Domain.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Core.Domain.DTOs.RequestDTO
{
    public class TransactionRecAmount
    {

    }

    public class RecviedAmountResponse
    {
        public decimal TotalPayedAmount { get; set; }
    }

    public class PaymentModeWiseAmount
    {
        public CodeBaseResponse PaymentMode { get; set; }= new CodeBaseResponse();  
        public decimal Amount { get; set; }
        public string? PayementDocumentNumber { get; set; }
        public DateTime PayementDocumentDate { get; set; }

    }

    public class PayementModeReciept
    {
        public IList<PaymentModeWiseAmount> Payements { get; set; }=new List<PaymentModeWiseAmount>();  
        public DateTime PayementDate { get; set; }
        public long TransactionKey { get; set; } = 1;
        public string? OurCode { get; set; } = "RECP";
        public long ElementKey { get; set; } = 1;
    }
}
