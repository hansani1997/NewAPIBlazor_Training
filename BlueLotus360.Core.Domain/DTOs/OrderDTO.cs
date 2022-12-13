using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.MastrerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Core.Domain.DTOs
{
    public class GenericOrder : BaseEntity
    {
        public int OrderKey { get; set; } = 1;
        public string OrderNumber { get; set; } = "";
        public string OrderDocumentNumber { get; set; } = "";
        public DateTime OrderDate { get; set; }
        public DateTime OrderFinishDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public CodeBaseResponse OrderLocation { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse OrderPaymentTerm { get; set; } = new CodeBaseResponse();
        public AddressResponse OrderCustomer { get; set; } = new AddressResponse();
        public AddressResponse OrderRepAddress { get; set; } = new AddressResponse();
        public AccountResponse OrderAccount { get; set; }
        public CodeBaseResponse OrderType { get; set; } = new CodeBaseResponse();
        public IList<GenericOrderItem> OrderItems { get; set; }
       // public OrderItem SelectedOrderItem { get; set; }
        public int FormObjectKey { get; set; } = 1;
        public decimal HeaderLevelDisountPrecentage { get; set; }
        public CodeBaseResponse BussinessUnit { get; set; } = new CodeBaseResponse();
        public int OrderDetKy { get; set; } = 1;
        public int FromOrderKey { get; set; } = 1;
        public bool IsFromQuotation { get; set; }
        public string HeaderDescription { get; set; } = "";
        public CodeBaseResponse OrderApproveState { get; set; }
        public CodeBaseResponse OrderPrefix { get; set; }
        public CodeBaseResponse OrderCategory1 { get; set; }
        public CodeBaseResponse OrderCategory2 { get; set; }
        public CodeBaseResponse OrderStatus { get; set; }
        public CodeBaseResponse OrderControlCondition { get; set; }
        public ProjectResponse OrderProject { get; set; }
        public int Cd1Ky { get; set; } = 1;
        public decimal MeterReading { get; set; }
        public User EnteredUser { get; set; }
        public AccountResponse Insurance { get; set; }
      
        public GenericOrder()
        {
            OrderLocation = new CodeBaseResponse();
            OrderPaymentTerm = new CodeBaseResponse();
            OrderCustomer = new AddressResponse();
            OrderRepAddress= new AddressResponse();
            OrderAccount=new AccountResponse();
            OrderType = new CodeBaseResponse();
            OrderDocumentNumber = "";
            OrderItems = new List<GenericOrderItem>();
            //SelectedOrderItem = new OrderItem();
            BussinessUnit = new CodeBaseResponse();
            OrderApproveState = new CodeBaseResponse();
            OrderPrefix = new CodeBaseResponse();
            OrderCategory1=new CodeBaseResponse();
            OrderCategory2=new CodeBaseResponse();
            OrderProject=new ProjectResponse();
            OrderStatus = new CodeBaseResponse();
            OrderControlCondition=new CodeBaseResponse();
            EnteredUser = new User();
            Insurance = new AccountResponse();
            EnteredUser = new User();
        }

    }


    public class GenericOrderItem:BaseDTO
    {
        public int FromOrderDetailKey { get; set; }
        public decimal TransactionRate { get; set; }
        public decimal Rate { get; set; }
        public CodeBaseResponse OrderLineLocation { get; set; }
        public int IsTransfer { get; set; }
        public int IsTransferConfirmed { get; set; }
        public decimal TransactionQuantity { get; set; }
        public decimal TransferQuantity { get; set; }
        public UnitResponse TransactionUnit { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public ItemResponse TransactionItem { get; set; }
        public CodeBaseResponse OrderType { get; set; }
        public decimal AvailableStock { get; set; }
        public decimal LineNumber { get; set; }
        public decimal RequestedQuantity { get; set; }
        public decimal LineTotal { get; set; }
        public decimal LineTotalWithoutDiscount { get; set; }

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
        public int IsApproved { get; set; }
        public int IsActive { get; set; }
        public CodeBaseResponse BussinessUnit { get; set; } = new CodeBaseResponse();
        public decimal AvailableQuantity { get; set; } = 0;
        public UnitResponse Unit { get; set; } = new UnitResponse();
        public decimal VAT { get; set; } = 0;
        public decimal SVAT { get; set; } = 0;
        public decimal AvlStk { get; set; } = 0;
        public string? Description { get; set; } = "";
        public string Remark { get; set; } = "";
        public int OrderDetailsAccountKey { get; set; }
        public AccountResponse BaringPrinciple { get; set; }
        public decimal PrinciplePrecentage { get; set; }
        public decimal PrincipleAmount { get; set; }
        public AccountResponse BaringCompany { get; set; }
        public decimal CompanyPrecentage { get; set; }
        public decimal CompanyAmount { get; set; }
        public decimal CustomerAmount { get; set; }
        public decimal Time { get; set; }
        public AccountResponse Supplier { get; set; }//where to map
        public AddressResponse ResourceAddress { get; set; } //technician ,where to map
        public DateTime InsertDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; } = DateTime.Now;
        public CodeBaseResponse AnalysisType1 { get; set; }
		public CodeBaseResponse AnalysisType2 { get; set; }
		public GenericOrderItem()
        {
            OrderLineLocation = new CodeBaseResponse();
            TransactionItem = new ItemResponse();
            OrderType = new CodeBaseResponse();
            BaringPrinciple = new AccountResponse();
            BaringCompany = new AccountResponse();
            Supplier=new AccountResponse();
            ResourceAddress = new AddressResponse();
            AnalysisType1 = new CodeBaseResponse();
            AnalysisType2= new CodeBaseResponse();  
        }

    }

    public class OrderOpenRequest
    {
        public long OrderKey { get; set; } = 1;
        public long BaseTypKy { get; set; } = 1;
        public long PrjKy { get; set; } = 1;
        public long ObjKy { get; set; } = 1;
    }

    public class OrderHeaderCreateDTO : BaseDTO
    {
        public DateTime OrderDate { get; set; } = new DateTime(1900, 1, 1);
        public CodeBaseResponse PayementTerm { get; set; } = new CodeBaseResponse();
        public string DocumentNumber { get; set; } = "";
        public string YourReference { get; set; } = "";
        public DateTime YourReferenceDate { get; set; } = new DateTime(1900, 1, 1);
        public decimal Amount { get; set; }
        public decimal TransactionAmount { get; set; }
        public int TransactionCurrencyKey { get; set; } = 1;
        public decimal TransactionExchangeRate { get; set; } = 1;
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TransactionDisountAmount { get; set; }
        public decimal MarkupPercentage { get; set; }
        public decimal RetensionPercentage { get; set; }
        public decimal RetensionPerdiod { get; set; }
        public decimal Gurantee { get; set; }
        public DateTime ExpiaryDate { get; set; } = new DateTime(1900, 1, 1);
        public int RetensionPerdiodKey { get; set; } = 1;
        public int BillFerquencyKey { get; set; } = 1;
        public decimal BillToPayement { get; set; }
        public int LocationAddressKey { get; set; } = 1;
        public bool IsOrderSetOff { get; set; }
        public string Description { get; set; } = "";
        public string Remarks { get; set; } = "";
        public int OrderFerquencyKey { get; set; } = 1;
        public int OrderStatusKey { get; set; } = 1;
        public long AddressKey { get; set; } = 1;
        public long RepAddessKey { get; set; } = 1;
        public int DistributerAddressKey { get; set; } = 1;
        public DateTime DeliveryDate { get; set; } = new DateTime(1900, 1, 1);
        public DateTime OrderFinishDate { get; set; } = new DateTime(1900, 1, 1);
        public int CustomItemKey { get; set; } = 1;

        public int IsActive { get; set; }
        public int IsApproved { get; set; }
        public int IsPrinted { get; set; }
        public int IsLocked { get; set; }
        public int AccessLevelKey { get; set; } = 1;
        public int ConfidentialLevelKey { get; set; } = 1;
        public int RecurenceDeliveryNo { get; set; } = 1;
        public int RecurenceOrderDay { get; set; } = 1;
        public int RecurenceOrderTime { get; set; } = 1;
        public int OrderCategory1Key { get; set; } = 1;
        public int OrderCategory2Key { get; set; } = 1;
        public int OrderCategory3Key { get; set; } = 1;
        public long Location2Key { get; set; } = 1;
        public int AprrovePriorityKey { get; set; } = 1;
        public int AprroceStatusKey { get; set; } = 1;
        public long AccountKey { get; set; } = 1;
        public int FromOrderKey { get; set; } = 1;
        public CodeBaseResponse OrderLocation { get; set; } = new CodeBaseResponse();

        public int PurchaseOrderKey { get; set; } = 1;
        public int Code1Key { get; set; } = 1;
        public int Code2Key { get; set; } = 1;
        public int ProjectKey { get; set; } = 1;
        public int OrderModeKey { get; set; } = 1;
        public int DelliveryModeKey { get; set; } = 1;
        public int OrderKey { get; set; } = 1;

        public int PrefixKey { get; set; } = 1;
        public int OrderNumber { get; set; }

        public string PrefixedOrderNumber { get; set; } = "";

        public CodeBaseResponse OrderType { get; set; } = new CodeBaseResponse();
        public decimal MeterReading { get; set; }
        public AccountResponse Insurance { get; set; } = new AccountResponse();


    }

    public class OrderHeaderEditDTO : OrderHeaderCreateDTO
    {
        public CodeBaseResponse OrderPrefix { get; set; } = new CodeBaseResponse();
        public string PrefixOrderNumber { get; set; }
        public CodeBaseResponse PayementTerm { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse TransactionCurrency { get; set; } = new CodeBaseResponse();
        public decimal TransactionDiscountAmount { get; set; }
        public CodeBaseResponse OrderFrequency { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse OrderStatus { get; set; } = new CodeBaseResponse();
        public AddressResponse RepAdress { get; set; } = new AddressResponse();
        public AddressResponse OrderAdress { get; set; } = new AddressResponse();
        public AddressResponse DistributerAddress { get; set; } = new AddressResponse();
        public int CustomeItemKey { get; set; } = 1;
        public int IsPrinter { get; set; }
        public CodeBaseResponse AccessLevel { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse ConfidentialLevel { get; set; } = new CodeBaseResponse();
        public int RecurenceDeliveryKey { get; set; } = 1;
        public CodeBaseResponse OrderCategory1 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse OrderCategory2 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse OrderCategory3 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse Location2 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse ApprovePriorty { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse ApproveStatus { get; set; } = new CodeBaseResponse();
        public AddressResponse LocationAddress { get; set; } = new AddressResponse();
        public CodeBaseResponse Code1 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse Code2 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse OrderMode { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse DeliveryMode { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse BillFrequency { get; set; } = new CodeBaseResponse();
        public decimal BillToPayment { get; set; }
        public decimal Retension { get; set; }
        public bool IsRecordLocked { get; set; }
        public CodeBaseResponse BussinessUnit { get; set; }
    }


    public class OrderLineCreateDTO : BaseDTO
    {
        public long AccountKey { get; set; } = 1;
        public decimal AddAlwPercentage { get; set; }
        public long AddressKey { get; set; } = 1;
        public decimal Amount1 { get; set; }
        public int Analysis1Key { get; set; } = 1;
        public int Analysis2Key { get; set; } = 1;
        public int Analysis3Key { get; set; } = 1;
        public int Analysis4Key { get; set; } = 1;
        public int Analysis5Key { get; set; } = 1;
        public int Analysis6Key { get; set; } = 1;
        public int AprovePriorityKey { get; set; } = 1;
        public int AprroveStatusKey { get; set; } = 1;
        public int BussinessUnitKey { get; set; } = 1;
        public decimal ConversionRate { get; set; }
        public decimal CustTime { get; set; }
        public int DeliveryAddressKey { get; set; } = 1;

        public string Description { get; set; } = "";
        public decimal DiscountPercentage { get; set; }
        public decimal DisocuntAmount { get; set; }
        public decimal DohPercentage { get; set; }
        public int FromOrderDetailCmpnKey { get; set; } = 1;
        public int FromOrderDetailKey { get; set; } = 1;
        public decimal GohPercentage { get; set; }
        public string GroupUuId { get; set; }
        public bool IsLineItemDirty { get; set; }
        public bool IsLineItemPersisted { get; set; }
        public bool IsNotProduction { get; set; }
        public bool IsProduction { get; set; }
        public bool IsRateInclTt1 { get; set; }
        public bool IsSetOff { get; set; }
        public bool IsVirtualItem { get; set; }
        public long ItemKey { get; set; } = 1;
        public string ItemNo { get; set; } = "";
        public int ItemPropertyKey { get; set; } = 1;
        public decimal ItemTaxType1 { get; set; }
        public decimal ItemTaxType1Per { get; set; }
        public decimal ItemTaxType2 { get; set; }
        public decimal ItemTaxType2Per { get; set; }
        public decimal ItemTaxType3 { get; set; }
        public decimal ItemTaxType3Per { get; set; }
        public decimal ItemTaxType4 { get; set; }
        public decimal ItemTaxType4Per { get; set; }
        public decimal ItemTaxType5 { get; set; }
        public decimal ItemTaxType5Per { get; set; }
        public decimal LcDetailKey { get; set; }
        public decimal LcKey { get; set; }
        public decimal LineNumber { get; set; }
        public int LiNoCurrencyKey { get; set; } = 1;
        public decimal LiNoDiscountAmount { get; set; }
        public decimal LiNoExchangeRate { get; set; }
        public decimal LiNoRate { get; set; }
        public int LoanDetailKey { get; set; } = 1;
        public int LoanKey { get; set; } = 1;
        public CodeBaseResponse OrderLineLocation { get; set; } = new CodeBaseResponse();
        public decimal Maint { get; set; }

        public DateTime OrderDate { get; set; }
        public string OrderItemName { get; set; }
        public int OrderKey { get; set; } = 1;
        public CodeBaseResponse OrderType { get; set; } = new CodeBaseResponse();
        public decimal OrgRate { get; set; }
        public int ProcessDetail2Key { get; set; } = 1;
        public int ProcessDetail3Key { get; set; } = 1;
        public int ProcessDetail4Key { get; set; } = 1;
        public int ProcessDetail5Key { get; set; } = 1;
        public int ProcessDetailKey { get; set; } = 1;
        public decimal ProductionTime { get; set; }
        public decimal ProfitProcentage { get; set; }
        public int Project2Key { get; set; } = 1;
        public int Project3Key { get; set; } = 1;
        public int Project4Key { get; set; } = 1;
        public int Project5Key { get; set; } = 1;
        public int ProjectKey { get; set; } = 1;
        public decimal Rate { get; set; }
        public string Remarks { get; set; }
        public DateTime RequiredDate { get; set; }
        public int ReserveAddressKey { get; set; } = 1;
        public string ReserveAddressID { get; set; } ="";
        public decimal TransactionDiscountAmount { get; set; }
        public decimal TransactionPrice { get; set; }
        public decimal TransactionQuantity { get; set; }
        public decimal TransactionRate { get; set; }
        public int TransactionUnitKey { get; set; } = 1;
        public string TransactionUnitName { get; set; } = "";
        public string Uuid { get; set; }

        public int OrderLineItemKey { get; set; } = 1;

        public int IsActive { get; set; }
        public int IsApproved { get; set; }
        public int UnitKey { get; set; } = 1;

        public int PrefixKey { get; set; } = 1;

        public int IsTransfer { get; set; } = 0;

        public int IsConfirmed { get; set; } = 0;

        public decimal OriginalQuantity { get; set; }
        public int ItemTypeKey { get; set; }
        public string? ItemTypeName { get; set; }
        public string? ItemTypeOurCode { get; set; }
        public DateTime InsertDate { get; set; }= DateTime.Now;
        public DateTime UpdateDate { get; set; } = DateTime.Now;

        public CodeBaseResponse AnalysisType1 = new CodeBaseResponse();
        public CodeBaseResponse AnalysisType2 = new CodeBaseResponse();
        public CodeBaseResponse AnalysisType3 = new CodeBaseResponse();
    }

    public class OrderFindDto
    {
        public int OrderKey { get; set; } = 1;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int OrderTypeKey { get; set; }
        public string OrderNo { get; set; } = "";
        public int FromOrderNumber { get; set; } = 0;
        public int ToOrderNumber { get; set; } = int.MaxValue;

        public string DocumentNumber { get; set; } = "";
        public string YourReference { get; set; } = ""; 

        public int LocationKey { get; set; } = 1;

        public int ProjectKey { get; set; } = 1;
        public int AccountKey { get; set; } = 1;

        public int AddressKey { get; set; } = 1;

        public int ApproveStatusKey { get; set; } = 1;
        public bool IsPrinterd { get; set; }

        public bool IsRecurence { get; set; }

        public int PrefixKey { get; set; } = 1;

        public int ObjectKey { get; set; } = 1;
        public CodeBaseResponse Location { get; set; }
        public CodeBaseResponse Prefix { get; set; }

        public OrderFindDto()
        {
            Location = new CodeBaseResponse();
            Prefix = new CodeBaseResponse();
        }


    }

    public class OrderFindResults
    {
        public int OrderKey { get; set; } = 1;

        public DateTime OrderDate { get; set; }

        public string Prefix { get; set; }

        public string OrderNumber { get; set; }

        public string DocumentNumber { get; set; }

        public string YourReference { get; set; }

        public string Description { get; set; } = "";
        public string CusSupId { get; set; } = "";
        public string CusSupName { get; set; } = "";
        public int ProjectKey { get; set; }
        public string ProjectName { get; set; }
        public CodeBaseResponse ApproveState { get; set; }
        public AccountResponse Account { get; set; }
        public int RequestingObjectKey { get; set; }
        public string PreviewURL { get; set; }
        public int EntUsrKy { get; set; }
        public int IsActive { get; set; }
        public CodeBaseResponse ApproveReason { get; set; }
        public OrderFindResults()
        {
            ApproveState = new CodeBaseResponse();
        }

    }

    public class GetFromQuoatationDTO
    {
        public int OrdTypKy { get; set; }
        public int PreOrdTypKy { get; set; }
        public AddressResponse Supplier { get; set; }
        public AddressResponse AdvAnalysis { get; set; }
        public CodeBaseResponse Location { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public CodeBaseResponse Project { get; set; }
        public string SoNo { get; set; } = "";
        public int OrdNo { get; set; } = 1;
        public int PreOrdPreFixKy { get; set; } = 1;
        public long ObjKy { get; set; }

        public GetFromQuoatationDTO()
        {
            Supplier = new();
            AdvAnalysis = new();
            Location = new CodeBaseResponse();
            Project = new CodeBaseResponse();
        }

    }

    public class GetFromQuotResults
    {
        public int OrdKy { get; set; }
        public string OrdNo { get; set; } = "";
        public DateTime OrdDt { get; set; }
        public string DocNo { get; set; } = "";
        public int PrjId { get; set; }
        public string PrjNm { get; set; } = "";
        public string SupAccCd { get; set; } = "";
        public string SupAccNm { get; set; } = "";
        public string Prefix { get; set; } = "";
        public string LocCd { get; set; } = "";
    }

    public class QuotationDetails : OrderLineCreateDTO
    {
        public string OrdNo { get; set; } = "";
        public string DocNo { get; set; } = "";
        public string YourReference { get; set; } = "";
        public decimal AvailableQuantity { get; set; }
        public UnitResponse Unit { get; set; }
        public CodeBaseResponse HdrLocation { get; set; }
        public AddressResponse OrderAdress { get; set; }
        public AddressResponse RepAdress { get; set; }
        public AddressResponse DistributeAddress { get; set; }
        public decimal VATAmt { get; set; } = 0;
        public decimal VAT { get; set; } = 0;
        public decimal WHTAmt { get; set; } = 0;
        public decimal WHT { get; set; } = 0;
        public decimal NBTAmt { get; set; } = 0;
        public decimal NBT { get; set; } = 0;
        public decimal SvatAmt { get; set; } = 0;
        public decimal SVAT { get; set; } = 0;
        public decimal AvlStk { get; set; } = 0;
        public decimal DiscountAmount { get; set; } = 0;
        public CodeBaseResponse PaymentTerm { get; set; } = new CodeBaseResponse();
    }


}
