using BlueLotus360.Core.Domain.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Core.Domain.Entity
{
    public class PartnerOrder : BaseEntity
    {
        public PartnerOrder()
        {
            OrderItemDetails = new List<PartnerOrderDetails>();
            // AuthorizedCompany = new Company();
            OrderStatus = new CodeBaseResponse();
            Location = new CodeBaseResponse();
            BU = new CodeBaseResponse();
            Platforms = new AccountResponse();
            Customer = new Person();
            // CreatedBy = new User();
            // UpdatedBy = new User();
            pagination = new Pagination();
        }
        public AccountResponse Platforms { get; set; }
        private decimal _quantity;
        private decimal _amount;
        public Pagination pagination { get; set; }
        // public User CreatedBy { get; set; }
        // public User UpdatedBy { get; set; }
        public CodeBaseResponse OrderStatus { get; set; }
        // public Company AuthorizedCompany { get; set; }
        public CodeBaseResponse Location { get; set; }
        public CodeBaseResponse BU { get; set; }
        public int OrderLastSyncMinutes { get; set; }
        public long PartnerOrderId  { get; set; } = 1; //auto-genarated
        public string OrderId { get; set; } = "";// partner order id
        public string OrderReference { get; set; } = "";//partner display id
        public int PaymentKey { get; set; } //uber wallet
        //public string PaymentName { get; set; }= ""; //uber wallet
        public long DeliveryBrandId { get; set; } = 1;//  Delivery brand
        public string DeliveryAccessCode { get; set; } = "";
        //Total Quantity
        public decimal Quantity
        {
            get
            {
                decimal totalQuantity = 0;

                if (_quantity == 0)
                {
                    {
                        foreach (PartnerOrderDetails partnerOrderItem in OrderItemDetails)
                        {
                            totalQuantity += partnerOrderItem.ItemQuantity;
                        }
                    }

                    _quantity = totalQuantity;
                }

                return _quantity;
            }

            set
            {
                _quantity = value;
            }
        }
        //Total Amount
        public decimal Amount
        {
            get
            {
                decimal totalAmount = 0;
                if (_amount == 0)
                {
                    {
                        foreach (PartnerOrderDetails partnerOrderItem in OrderItemDetails)
                        {
                                totalAmount += partnerOrderItem.BaseTotalPrice;
                            
                        }
                    }

                    _amount = totalAmount;
                }

                return _amount;
            }

            set
            {
                _amount = value;
            }
        }
        public decimal DiscountAmount { get; set; } //Total dis Amount
        //uber created date
        public string OrderDate { get; set; } = "";
        public string PickupTime { get; set; } = "";
        public string OrderNote { get; set; } = ""; //Order Comment
        public string DeliveryNote { get; set; } = ""; //Delivery Comment
        public string DeliveryBrand { get; set; } = "";
        public long WorkStationKey { get; set; }

        //public string OrderType { get; set; }
        //public string OrderStatus { get; set; }
        public string PaymentType { get; set; } = "";
        public string PaymentCode { get; set; } = "";
        // public decimal Discount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal ChargesAmount { get; set; }
        public List<PartnerOrderDetails> OrderItemDetails { get; set; }
        public Person Customer { get; set; }
        public bool IsBarPrint { get; set; } = false;
        public bool IsAllKOTMode { get; set; } = false;
        public string KOTReportSource { get; set; } = "";
        public string BillReportSource { get; set; } = "";
        public string SalesReportSource { get; set; } = "";
        public bool IsKitchenPrint { get; set; }
        public decimal DeliveryCharges { get; set; }


    }

    public class PartnerOrderDetails : BaseEntity
    {

        public PartnerOrderDetails()
        {
            OrderItem = new ItemResponse();
        }

        public long PartnerOrderDetailsId { get; set; } //auto
        public long ParnterOrderId { get; set; } = 1; //from header, auto system insert

        public ItemResponse OrderItem { get; set; }
        public decimal ItemQuantity { get; set; }
        public decimal TransactionPrice { get; set; }
        public decimal BaseTotalPrice { get; set; }
        public decimal ItemDiscount { get; set; } = 0;
        public string SpecialInstructions { get; set; } = ""; //item comment
        public bool IsComposite { get; set; }
        public bool IsModifier { get; set; }
        public long ItemOfferKey { get; set; } //if offers have pass this key
        public int AvailableStockCount { get; set; } = 0;
        public string PartnerOrderItemInstanceId { get; set; } = "";  //Unique identifying string for the shopping cart item, provided by DeliveryPartners (Uber)
        public string Remarks { get; set; } = "";


    }


    public class Person
    {
        private string name = "";
        private int key;
        private string address = "";
        private string city = "";
        private string postalCode = "";
        private int isAct;
        private string phone = "";
        private string loyaltyCardNo = "";
        private string email = "";
        private string doorNo = "";
        private string adrId = "";
        private int adrKy = 1;

        public string Name { get => name; set => name = value; }
        public int Key { get => key; set => key = value; }
        public string Address { get => address; set => address = value; }
        public string City { get => city; set => city = value; }
        public string PostalCode { get => postalCode; set => postalCode = value; }
        public int IsAct { get => isAct; set => isAct = value; }
        public string Phone { get => phone; set => phone = value; }
        public string LoyaltyCardNo { get => loyaltyCardNo; set => loyaltyCardNo = value; }
        public string Email { get => email; set => email = value; }
        public string DoorNo { get => doorNo; set => doorNo = value; }
        public string AdrId { get => adrId; set => adrId = value; }
        public int AdrKy { get => adrKy; set => adrKy = value; }
    }

    public class Pagination
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 15;
    }

    public class RequestParameters
    {
        public RequestParameters()
        {
            pagination = new Pagination();
        }
        public string FromDate { get; set; } = "";
        public string ToDate { get; set; } = "";
        public int LocationKey { get; set; }
        public int BUKy { get; set; }
        public int StatusKey { get; set; }
        public int OrderKey { get; set; }
        public int OrderTypeKey { get; set; }
        public string PlatformName { get; set; } = "";
        public string OrderID { get; set; } = "";
        public int ItemKey { get; set; }

        public Pagination pagination { get; set; }
    }

    public class StockInjection
    {
        public int OrderKey { get; set; }
        public string? IntegrationId { get; set; }
        public string? RequestId { get; set; }
    }

    public class ResponseDetails
    {
        public int TriggerKey { get; set; }
        public int SubscriberKey { get; set; }
        public string? ResponseCode { get; set; }
        public string? Response { get; set; }
        public string? Reference { get; set; }
        public string? TrnTyp { get; set; }
        public string? ContenetPayload { get; set; }
    }

    public class UberDiscontinueItem
    {
        public string ItmCd { get; set; }
        public bool isDiscontinue { get; set; }

    }
}
