using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.MastrerData;

namespace BlueLotus360.Core.Domain.Entity.Transaction
{
    public class BLTransaction : BaseEntity
    {
        public long TransactionKey { get; set; } = 1;
        public string? TransactionNumber { get; set; }
        public long ElementKey { get; set; } = 1;
        public CodeBaseResponse TransactionType { get; set; } = new CodeBaseResponse();
        public decimal Amount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal HeaderDiscountAmount { get; set; }
        public CodeBaseResponse TransactionCurrency { get; set; } = new CodeBaseResponse();
        public decimal TransactionExchangeRate { get; set; }
        public string? DocumentNumber { get; set; }
        public string? YourReference { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public DateTime YourReferenceDate { get; set; } = DateTime.Now;
        public CodeBaseResponse PaymentTerm { get; set; } = new CodeBaseResponse();
        public int IsPrinted { get; set; }
        public int IsRecurrence { get; set; }
        public DateTime ReminderDate { get; set; } = DateTime.Now;
        public int IsLocked { get; set; }
        public CodeBaseResponse AccessLevel { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse ConfidentialLevel { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse ApproveState { get; set; } = new CodeBaseResponse();
        public long ObjectKey { get; set; } = 1;
        public ProjectResponse TransactionProject { get; set; }= new ProjectResponse(); 
        public CodeBaseResponse BussinessUnit { get; set; } = new CodeBaseResponse();
        public long HeaderTransferLinkKey = 1;
        public long ContraAccountObjectKey { get; set; } = 1;
        public AccountResponse ContraAccount { get; set; } = new AccountResponse();
        public CodeBaseResponse Location { get; set; } = new CodeBaseResponse();
        public string Description { get; set; } = "";
        public string Remarks { get; set; } = "";
        public ItemResponse CustomItem { get; set; } = new ItemResponse();
        public CodeBaseResponse Shift { get; set; } = new CodeBaseResponse();
        public decimal CommisionPercentage { get; set; }

        public int IsQuantityPosted { get; set; }   
        public int IsValuePosted { get; set; }
        public int IsItemTrnValue { get; set; }
        public decimal Amount1 { get; set; }
        public decimal Amount2 { get; set; }
        public decimal Amount3 { get; set; }
        public decimal Amount4 { get; set; }
        public decimal Amount5 { get; set; }
        public decimal Amount6 { get; set; }
        public string OtherNumber { get; set; } = "";
        public CodeBaseResponse Code1 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse Code2 { get; set; } = new CodeBaseResponse();
        public AddressResponse Rep { get; set; }= new AddressResponse();
        public decimal RepCommissionPercentage { get; set; }
        public long OrderDetailKey { get; set; } = 1;
        public long AccountObjectKey { get; set; } = 1;
        public AccountResponse Account { get; set; } = new AccountResponse();
        public AddressResponse Address { get; set; } = new AddressResponse();
        public long OrderNumberKey { get; set; } = 1;
        public AddressResponse Address1 { get; set; } = new AddressResponse();
        public AddressResponse Address2 { get; set; } = new AddressResponse();
        public long FromOrderKey { get; set; } = 1;
        public long FromTransactionKey { get; set; } = 1;
        public long RecurenceDeliveryKey { get; set; } = 1;
        public string? TransactionImageFilePath { get; set; } = "";
        public int IsMultiCredit { get; set; }
        public decimal ItemTaxType1 { get; set; }
        public decimal ItemTaxType2 { get; set; }
        public decimal ItemTaxType3 { get; set; }
        public decimal ItemTaxType4 { get; set; }
        public decimal ItemTaxType5 { get; set; }
        public bool IsFromImport { get; set; }
        public bool IsHold { get; set; }
        public bool IsBlock { get; set; }
        public CodeBaseResponse ApproveReason { get; set; } = new CodeBaseResponse();

        public ItemSerialNumber SerialNumber { get; set; } = new ItemSerialNumber();


        public DateTime DeliveryDate { get; set; } = DateTime.Now;
        //
        public DateTime DueDate { get; set; } = DateTime.Now;
        public string? Prefix { get; set; } = "";
        public string? PreviewURL { get; set; } = "";
        public int EntUsrKy { get; set; }
        public IList<GenericTransactionLineItem> InvoiceLineItems { get; set; }= new List<GenericTransactionLineItem>();

        public decimal TotalMarkupValue { get; set; }
        public decimal MarkupPercentage { get; set; }

        public bool IsVarcar1On { get; set; }
        public decimal Quantity1 { get; set; }
        public CodeBaseResponse TransactionControlCondition { get; set; }=new CodeBaseResponse();

        public BLTransaction()
        {
            Location = new CodeBaseResponse();
            PaymentTerm = new CodeBaseResponse();
            Address = new AddressResponse();
            InvoiceLineItems = new List<GenericTransactionLineItem>();
            YourReferenceDate = DateTime.Now;
            SerialNumber = new ItemSerialNumber();
            TransactionControlCondition = new CodeBaseResponse();
        }


        public decimal GetOrderTotalWithDiscounts()
        {
            return InvoiceLineItems.Sum(x => x.GetLineTotalWithDiscount());
        }

        public decimal GetOrderTotalWithoutDiscounts()
        {
            return InvoiceLineItems.Sum(x => x.GetLineTotalWithoutDiscount());
        }

        public decimal GetOrderRateTotal()
        {
            return InvoiceLineItems.Sum(x => x.TransactionRate);
        }

        public decimal GetOrderDiscountTotal()
        {
            return InvoiceLineItems.Sum(d => d.GetLineDiscount());
        }

        public decimal GetQuantityTotal()
        {
            return InvoiceLineItems.Sum(q => q.TransactionQuantity);
        }


        public void AddGridItems(GenericTransactionLineItem item)
        {
            InvoiceLineItems.Add(item);

        }


    }

    public class TransactionFindRequest
    {
        public long TransactionKey { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public CodeBaseResponse? Prefix { get; set; }
        public CodeBaseResponse? ApproveStatus { get; set; }
        public string? TransactionNumber { get; set; }
        public string DocumentNumber { get; set; } = "";
        public string? YourReference { get; set; } = "";
        public CodeBaseResponse? Location { get; set; }
        public AddressResponse? Address { get; set; }
        public ProjectResponse? Project { get; set; }
        public AccountResponse? Suuplier { get; set; }
        public ItemResponse? Item { get; set; }
        public int IsRecurrence { get; set; } = 0;
        public int IsPrinted { get; set; } = 0;
        public long ElementKey { get; set; } = 1;

        public CodeBaseResponse? PaymentTerm { get; set; }

        public decimal Amount { get; set; }
        public CodeBaseResponse? TransactionType { get; set; }

    }

    public class TransactionOpenRequest
    {
        public long TransactionKey { get; set; } = 1;

        public long TrasctionTypeKey { get; set; } = 1;

        public long ElementKey { get; set; } = 1;
    }
    public class GenericTransactionFindResponse
    {
        public long TransactionKey { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Prefix { get; set; }
        public string? TransactionNumber { get; set; }
        public string? DocumentNumber { get; set; }
        public string? YourReference { get; set; }
        public CodeBaseResponse? Location { get; set; }
        public AddressResponse? Address { get; set; }
        public decimal Amount { get; set; }

        public int IsApprove { get; set; } = 1;


    }

    public class StockAsAtResponse
    {
        public long ItemKey { get; set; }
        public decimal StockAsAt { get; set; }

    }



    public class StockAsAtRequest
    {
        public long ItemKey { get; set; }
        public long LocationKey { get; set; }
        public long ElementKey { get; set; } = 1;
        public long ProjectKey { get; set; } = 1;
        public int TrnTypKy { get; set; } = 1;
        public int IsuPrjKy { get; set; } = 1;
        public int IsuLocKy { get; set; } = 1;
        public int TrnUnitKy { get; set; } = 1;
    }
}
