﻿using BlueLotus360.Core.Domain.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Core.Domain.Entity.Transaction
{
    public class GenericTransactionLineItem
    {
        public long TransactionKey { get; set; } = 1;
        public long ElementKey { get; set; } = 1;

        public long ItemTransactionKey { get; set; } = 1;
        public DateTime EffectiveDate { get; set; } = DateTime.Now;
        public int LineNumber { get; set; }
        public long ItemTransferLinkKey = 1;
        public Item TransactionItem { get; set; } = new Item();
        public CodeBaseSimple TransactionLocation { get; set; } = new CodeBaseSimple();
        public decimal Quantity { get; set; }
        public decimal TransactionQuantity { get; set; }
        public decimal TransactionRate { get; set; }
        public decimal TransactionPrice { get; set; }
        public decimal Rate { get; set; }
        public UnitSimple TransactionUnit { get; set; } = new UnitSimple();
        public CodeBaseSimple BussinessUnit { get; set; } = new CodeBaseSimple();
        public decimal DiscountAmount { get; set; }
        public decimal TransactionDiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public CodeBaseSimple TransactionProject { get; set; }
        public CodeBaseSimple Address { get; set; }
        public CodeBaseSimple ItemProperty { get; set; } = new CodeBaseSimple();
        public CodeBaseSimple ConditionsState { get; set; } = new CodeBaseSimple();
        public int IsInventory { get; set; } = 1;
        public int IsCosting { get; set; } = 1;
        public int IsSetOff { get; set; } = 0;
        public int OrderDetailKey { get; set; } = 1;
        public long ReferenceItemTransactionKey { get; set; } = 1;
        public CodeBaseSimple Code1 { get; set; } = new CodeBaseSimple();
        public CodeBaseSimple Code2 { get; set; } = new CodeBaseSimple();
        public string Description { get; set; }
        public string Remarks { get; set; }
        public long OrderKey { get; set; } = 1;
        public long Skey { get; set; } = 1;
        public decimal QuantityPercentage { get; set; }
        public decimal HeaderDiscountAmount { get; set; }
        public Project Project2 { get; set; }
        public decimal Quantity2 { get; set; }
        public decimal TaskQuantity { get; set; }
        public UnitSimple TaskUnit { get; set; }
        public decimal FromNo { get; set; }
        public decimal ToNo { get; set; }
        public decimal NextActionNo { get; set; }
        public DateTime NextActionDate { get; set; }
        public CodeBaseSimple NextActionType { get; set; } = new CodeBaseSimple();
        public CodeBaseSimple ItemPack { get; set; } = new CodeBaseSimple();
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
        public CodeBaseSimple Analysis1 { get; set; } = new CodeBaseSimple();
        public CodeBaseSimple Analysis2 { get; set; } = new CodeBaseSimple();
        public CodeBaseSimple Analysis3 { get; set; } = new CodeBaseSimple();
        public CodeBaseSimple Analysis4 { get; set; } = new CodeBaseSimple();
        public CodeBaseSimple Analysis5 { get; set; } = new CodeBaseSimple();
        public CodeBaseSimple Analysis6 { get; set; } = new CodeBaseSimple();
        public decimal SalesPrice2 { get; set; }
        public Address ReservationAddress { get; set; } = new Address();
        public long ItemBudgetKey { get; } = 1;
        public bool IsQuantiy { get; set; } = false;


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
        public DateTime DateTime1 { get; set; }
        public DateTime DateTime2 { get; set; }
        public DateTime DateTime3 { get; set; }
        public CodeBaseSimple TransactionType { get; set; } = new CodeBaseSimple();
        public CodeBaseSimple ProjectTaskLocation { get; set; } = new CodeBaseSimple();
        public long ObjectKey = 1;
        public long FromItemTransactionKey = 1;
        public long OfferItemTransactionKey = 1;
        public decimal LineAmount { get; set; }
        public  Item ProcessItem { get; set; } = new Item();

        public decimal BalanceQuantity { get; set; }


        public CodeBaseSimple ItemCategory1;
        public CodeBaseSimple ItemCategory2;

        public decimal MarkupPercentage { get; set; }
        public decimal MarkupAmount { get; set; }
        public decimal TotalMarkupAmount { get; set; }
        public DateTime DeliveryDate { get; set; }
        public IList<ItemSerialNumber> SerialNumbers { get; set; }


        public GenericTransactionLineItem()
        {
            //TransactionItem = new();
            // TransactionUnit = new UnitResponse();
            SerialNumbers = new List<ItemSerialNumber>();

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