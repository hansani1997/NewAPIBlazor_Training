using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlueLotus360.Core.Domain.Entity.UberEats
{
    public class Uber
    {
        public string Id { get; set; }
        public string Display_id { get; set; }
        public string External_reference_id { get; set; }
        public string External_data { get; set; }
        public string Current_state { get; set; }
        public string First_name { get; set; }
        public string Type { get; set; }
        public string Freeform_text { get; set; }
        public string Title { get; set; }

    }

    public class UberOrder : Uber
    {
        public string Brand { get; set; }
        public Store Store { get; set; }
        public Eater Eater { get; set; }
        public List<Eater> Eaters { get; set; }
        public Cart Cart { get; set; }
        public Payment Payment { get; set; }
        public Packaging Packaging { get; set; }
        public DateTime Placed_at { get; set; }
        public DateTime Estimated_ready_for_pickup_at { get; set; }
        public List<UberEatsDelivery> Deliveries { get; set; }
    }

    public class Store : Uber
    {
        public string Name { get; set; }
    }

    public class Eater : Uber
    {

        public string Last_name { get; set; }
        public string Phone { get; set; }
        public string Phone_code { get; set; }
        public MerchantDelivery Delivery { get; set; }
    }

    public class MerchantDelivery : Uber
    {
        public UberLocation Location { get; set; }
        public string Notes { get; set; }

    }

    public class UberEatsDelivery : Uber
    {

        public Vehicle Vehicle { get; set; }
        public string Estimated_pickup_time { get; set; }
        public string Picture_url { get; set; }
    }

    public class UberLocation : Uber
    {

        public string Street_address { get; set; }
        public int Latitude { get; set; }
        public int Longitude { get; set; }
        public string Google_place_id { get; set; }
        public string Unit_number { get; set; }

    }

    public class Cart
    {
        public List<UberItem> Items { get; set; }
        public string Special_instructions { get; set; }
        public List<FulfillmentIssue> Fulfillment_issues { get; set; }
    }

    public class UberItem : Uber
    {
        public string Instance_id { get; set; } //Unique identifying string for the shopping cart item, provided by Uber.
        public int Quantity { get; set; }
        public int Default_quantity { get; set; }
        public Allergy Special_requests { get; set; }
        public UberPrice Price { get; set; }
        public List<SelectedModifierGroup> Selected_modifier_groups { get; set; }
        public string Eater_id { get; set; }
        public string Special_instructions { get; set; }
        public FulfillmentAction Fulfillment_action { get; set; }
    }

    public class SelectedModifierGroup : Uber
    {
        public List<UberItem> Selected_items { get; set; }
        public List<UberItem> Removed_items { get; set; }
    }

    public class Vehicle
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string License_plate { get; set; }
    }


    public class UberPrice
    {
        public Money Unit_price { get; set; }
        public Money Total_price { get; set; }
        public Money Base_unit_price { get; set; }
        public Money Base_total_price { get; set; }
    }

    public class Money
    {
        public int Amount { get; set; }
        public string Currency_code { get; set; }
        public string Formatted_amount { get; set; }
    }


    public class FulfillmentAction
    {
        public string Fulfillment_action_type { get; set; }
    }

    public class FulfillmentIssue
    {

        public FulfillmentIssue()
        {
            Root_item = new UberEatsFulFillmentRootItem();
            Item_substitute = new UberEatsFulFillmentSubstituteItem();
            Item_availability_info = new ItemAvailabilityInfo();
        }
        public string Fulfillment_issue_type { get; set; }
        public string Fulfillment_action_type { get; set; }
        public UberEatsFulFillmentRootItem Root_item { get; set; }
        public UberEatsFulFillmentSubstituteItem Item_substitute { get; set; }
        public ItemAvailabilityInfo Item_availability_info { get; set; }
    }

    public class UberEatsFulFillmentRootItem
    {
        public string Instance_id { get; set; } //Unique identifying string for the shopping cart item, provided by Uber
    }

    public class UberEatsFulFillmentSubstituteItem
    {
        public string Id { get; set; } //Unique identifying string for the item substitute, provided by the store
        public int Quantity { get; set; }
    }

    public class ItemAvailabilityInfo
    {
        //public int Items_requested { get; set; }
        public int Items_available { get; set; }
    }

    public class Allergy
    {
        public List<Allergen> Allergens_to_exclude { get; set; }
        public string Allergy_instructions { get; set; }
    }

    public class Allergen : Uber
    {

    }

    public class Packaging
    {
        public DisposableItems Disposable_items { get; set; }
    }

    public class DisposableItems
    {
        public bool Should_include { get; set; }
    }

    public class Payment
    {
        public Charges Charges { get; set; }
        public Accounting Accounting { get; set; }
   
      
    }

    public class Charges
    {
        public Money Total { get; set; }
        public Money Sub_total { get; set; }
        public Money Tax { get; set; }
        public Money Total_fee { get; set; }
        public Money Total_fee_tax { get; set; }
        public Money Bag_fee { get; set; }
        public Money Total_promo_applied { get; set; }
        public Money Sub_total_promo_applied { get; set; }
        public Money Tax_promo_applied { get; set; }
        public Money Pick_and_pack_fee { get; set; }
        public Money Delivery_fee { get; set; }
        public Money Delivery_fee_tax { get; set; }
        public Money Small_order_fee { get; set; }
        public Money Small_order_fee_tax { get; set; }
        public Money Tip { get; set; }

    }

    public class Accounting
    {
        public TaxRemittance Tax_remittance { get; set; }
    }

    public class TaxRemittance
    {
        public RemittanceInfo Tax { get; set; }
        public RemittanceInfo Total_fee_tax { get; set; }
        public RemittanceInfo Delivery_fee_tax { get; set; }
        public RemittanceInfo Small_order_fee_tax { get; set; }
    }

    public class RemittanceInfo
    {
        public List<PayeeDetail> Uber { get; set; }
        public List<PayeeDetail> Restaurant { get; set; }
        public List<PayeeDetail> Courier { get; set; }
        public List<PayeeDetail> Eater { get; set; }

    }

    public class PayeeDetail : Money
    {

    }

    public class Promotion
    {
        public string External_promotion_id { get; set; }
        public string Promo_type { get; set; }
        public int Promo_discount_value { get; set; }
        public int Promo_discount_percentage { get; set; }
        public int Promo_delivery_fee_value { get; set; }
        public List<DiscountItem> Discount_items { get; set; }

    }

    public class DiscountItem
    {
        public string External_id { get; set; }
        public int Discounted_quantity { get; set; }
    }

    public class ReasonForDeny
    {
        public long ReasonKey { get; set; }
        public string ReasonName { get; set; }
        public string Explanation { get; set; }
        public string Code { get; set; }
        public List<string> Out_of_stock_items { get; set; }
        public List<string> Invalid_items { get; set; }
        public long OrderTypeKey { get; set; }
        public long OrderStatusKey { get; set; }
    }

    public class ReasonForCancel
    {
        public string Reason { get; set; }
        public string Details { get; set; }
    }

    public class ReasonDeny
    {
        public ReasonDeny()
        {
            Reason = new ReasonForDeny();
        }
        public ReasonForDeny Reason { get; set; }
    }

    public class UberEatsPatchCart
    {

        public UberEatsPatchCart()
        {
            Fulfillment_issues = new List<FulfillmentIssue>();
        }
        public List<FulfillmentIssue> Fulfillment_issues { get; set; }
    }

    public enum UberTokenEndpoints
    {
        [Description("eats.store")]
        Eats_Store_Read_Scope,
        [Description("eats.order")]
        Eats_Order_Write_Scope,
        [Description("eats.pos_provisioning")]
        Eats_Provisioning_Scope,
        [Description("eats.store.status.write")]
        Eats_Store_Write_Scope,
        [Description("eats.store.orders.read")]
        Eats_Order_Read_Scope,
        [Description("eats.report")]
        Eats_Report_Scope
    }

    public enum UberEndpointURLS
    {
        UploadMenu,
        SetHolidayHours,
        UpdateItem,
        GetOrder,
        AcceptOrder,
        DenyOrder,
        CancelOrder,
        GetStoreList,
        GetStoreStatus,
        UpdateStoreStatus,
        ProvisionSetup,
        AuthCode,
        PatchCart,
    }

    public enum UberRequestIDs
    {
        [Description("{store_id}")]
        store_id,
        [Description("{order_id}")]
        order_id,
        [Description("{item_id}")]
        item_id,
    }

    public enum UberEatsFullfillmentIssueType
    {
        OUT_OF_ITEM,
        PARTIAL_AVAILABILITY,

    }

    public enum UberEatsFullfillmentActionType
    {
        REMOVE_ITEM,
        REPLACE_FOR_ME,
        SUBSTITUTE_ME,

    }
    public class UberTokenResponse
    {
        public string Access_token { get; set; }
        public long Expires_in { get; set; }
        public string Token_type { get; set; }
        public string Scope { get; set; }
        public string Refresh_token { get; set; }
        public int Last_authenticated { get; set; }
    }

    public class PartnerMenuItem
    {

        public string? ItemImage { get; set; } //need
        public string? ItemImageUrl { get; set; } //need
        public string? ItemName { get; set; } //need
        public long ItemKey { get; set; }
        public string? ItemCode { get; set; } //need
        public string? ItemShortName { get; set; }
        public string? CategoryName { get; set; } //need
        public string? CategoryCode { get; set; } //need
        public string? EAN { get; set; }
        public string? Description { get; set; } //need
        public string? Remarks { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalesPrice { get; set; } //need
        public decimal OptionalSalesPrice { get; set; }
        public bool IsModifierItem { get; set; }
        public bool IsCompositeItem { get; set; }
        public bool IsPayemntType { get; set; }
        public decimal MaximumDiscount { get; set; }
        public decimal VatPercentage { get; set; }
        public bool IsDiscontinued { get; set; } //need
        public bool IsAgeVerification { get; set; } //need
        public bool IsPartnerItem { get; set; } //need
        public bool IsAvailableInPartnerSide { get; set; } //need
        public PartnerMenuItem ParentItem { get; set; }
        public DateTime ExpireDate { get; set; }
        public int ValueForProjectKey { get; set; }
        public int FoodTypeId { get; set; } = 1;
        public string FoodTypeName { get; set; } = "";
        public int UrbanPiperFoodTypeId { get; set; } = 1;
        public bool IsRecommended { get; set; }
        public decimal SupplierWarranty { get; set; }
        public decimal CustomerWarranty { get; set; }
        public string? ItemComboTitle
        {
            get
            {
                return ItemCode + " - " + ItemName;
            }
        }
        public string? PartNumber { get; set; }
        //public CodeBase Model { get; set; }
        public bool IsSerialNumber { get; set; }
        public decimal ReOrderLevel { get; set; }
        public decimal ReOrderQuantity { get; set; }
        public int SortingOrder { get; set; }

        public byte[] imageArr { get; set; }

    }
}
