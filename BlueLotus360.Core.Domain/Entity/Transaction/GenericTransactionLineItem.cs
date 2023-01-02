using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.MastrerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Core.Domain.Entity.Transaction
{
    public class GenericTransactionLineItem : BaseEntity
    {
        public long TransactionKey { get; set; } = 1;
        public long ElementKey { get; set; } = 1;

        public long ItemTransactionKey { get; set; } = 1;
        public DateTime EffectiveDate { get; set; } = DateTime.Now;
        public int LineNumber { get; set; }
        public long ItemTransferLinkKey = 1;
        public ItemResponse TransactionItem { get; set; } = new ItemResponse();
        public CodeBaseResponse TransactionLocation { get; set; } = new CodeBaseResponse();
        public decimal Quantity { get; set; }
        public decimal TransactionQuantity { get; set; }
        public decimal TransactionRate { get; set; }
        public decimal TransactionPrice { get; set; }
        public decimal Rate { get; set; }
        public UnitResponse TransactionUnit { get; set; } = new UnitResponse();
        public CodeBaseResponse BussinessUnit { get; set; } = new CodeBaseResponse();
        public decimal DiscountAmount { get; set; }
        public decimal TransactionDiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public ProjectResponse TransactionProject { get; set; } = new ProjectResponse();
        public AddressResponse Address { get; set; } = new AddressResponse();
        public CodeBaseResponse ItemProperty { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse ConditionsState { get; set; } = new CodeBaseResponse();
        public int IsInventory { get; set; } = 1;
        public int IsCosting { get; set; } = 1;
        public int IsSetOff { get; set; } = 0;
        public int OrderDetailKey { get; set; } = 1;
        public long ReferenceItemTransactionKey { get; set; } = 1;
        public CodeBaseResponse Code1 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse Code2 { get; set; } = new CodeBaseResponse();
        public string? Description { get; set; }
        public string? Remarks { get; set; }
        public long OrderKey { get; set; } = 1;
        public long Skey { get; set; } = 1;
        public decimal QuantityPercentage { get; set; }
        public decimal HeaderDiscountAmount { get; set; }
        public ProjectResponse Project2 { get; set; } = new ProjectResponse();
        public decimal Quantity2 { get; set; }
        public decimal TaskQuantity { get; set; }
        public UnitSimple? TaskUnit { get; set; }
        public decimal FromNo { get; set; }
        public decimal ToNo { get; set; }
        public decimal NextActionNo { get; set; }
        public DateTime NextActionDate { get; set; }= DateTime.Now;
        public CodeBaseResponse NextActionType { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse ItemPack { get; set; } = new CodeBaseResponse();
        public decimal CommisionPercentage { get; set; }
        public decimal ItemTaxType1 { get; set; }
        public decimal ItemTaxType2 { get; set; }
        public decimal ItemTaxType3 { get; set; }
        public decimal ItemTaxType4 { get; set; }
        public decimal ItemTaxType5 { get; set; }
        public decimal ItemTaxType1Per { get; set; }
        public decimal ItemTaxType2Per { get; set; }
        public decimal ItemTaxType3Per { get; set; }
        public decimal ItemTaxType4Per { get; set; }
        public decimal ItemTaxType5Per { get; set; }
        public long LCKey { get; set; } = 1;
        public long LoanKey { get; set; } = 1;
        public long ProcessDetailKey { get; set; } = 1;
        public long LCDetailKey { get; set; } = 1;
        public long LoanDetailKey { get; set; } = 1;
        public CodeBaseResponse Analysis1 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse Analysis2 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse Analysis3 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse Analysis4 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse Analysis5 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse Analysis6 { get; set; } = new CodeBaseResponse();
        public decimal SalesPrice2 { get; set; }
        public AddressResponse ReservationAddress { get; set; } = new AddressResponse();
        public long ItemBudgetKey { get; } = 1;
        public bool IsQuantiy { get; set; } = false;
        public decimal BalanceQuantity { get; set; }

        public decimal Amount1 { get; set; }
        public decimal Amount2 { get; set; }
        public decimal Amount3 { get; set; }
        public decimal Amount4 { get; set; }
        public decimal Amount5 { get; set; }
        public decimal Amount6 { get; set; }
        public decimal Amount7 { get; set; }
        public decimal Amount8 { get; set; }
        public decimal Amount9 { get; set; }
        public decimal Amount10 { get; set; }
        public decimal LooseQuantity { get; set; }
        public DateTime DateTime1 { get; set; }= DateTime.Now;
        public DateTime DateTime2 { get; set; } = DateTime.Now;
        public DateTime DateTime3 { get; set; } = DateTime.Now;
        public CodeBaseResponse TransactionType { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse ProjectTaskLocation { get; set; } = new CodeBaseResponse();
        public long ObjectKey = 1;
        public long FromItemTransactionKey = 1;
        public long OfferItemTransactionKey = 1;
        public decimal LineAmount { get; set; }
        public  ItemResponse ProcessItem { get; set; } = new ItemResponse();

        public CodeBaseResponse ItemCategory1 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse ItemCategory2 { get; set; } = new CodeBaseResponse();

        public decimal MarkupPercentage { get; set; }
        public decimal MarkupAmount { get; set; }
        public decimal TotalMarkupAmount { get; set; }
        public DateTime DeliveryDate { get; set; } = DateTime.Now;
        public IList<ItemSerialNumber> SerialNumbers { get; set; }= new List<ItemSerialNumber>();

        // CarMart
        public AccountResponse BaringPrinciple { get; set; }=new AccountResponse();
        public decimal PrinciplePrecentage { get; set; }
        public decimal PrincipleAmount { get; set; }
        public AccountResponse BaringCompany { get; set; } = new AccountResponse();
        public decimal CompanyPrecentage { get; set; }
        public decimal CompanyAmount { get; set; }
        public decimal CustomerAmount { get; set; }
        public int IsSelected { get; set; }
        public decimal Time { get; set; }
        public int IsMaterialItem { get; set; }
        public int IsServiceItem { get; set; }
        public decimal SubTotal { get; set; }
        public int TransactionDetailsAccountKey { get; set; } = 1;

        public GenericTransactionLineItem()
        {
            //TransactionItem = new();
            // TransactionUnit = new UnitResponse();
            SerialNumbers = new List<ItemSerialNumber>();
            BaringPrinciple = new AccountResponse();
            BaringCompany= new AccountResponse();   

        }

        public decimal GetLineDiscount()
        {
            return (this.TransactionRate * DiscountPercentage / 100) * TransactionQuantity;

        }

        public decimal GetLineTotalWithDiscount()
        {
            return GetLineTotalWithoutDiscount() - GetLineDiscount();
        }





        public decimal GetLineTotalWithoutDiscount()
        {
            return TransactionQuantity * TransactionRate;
        }

    }
}
