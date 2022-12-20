using BlueLotus360.Core.Domain.Entity;
using BlueLotus360.Core.Domain.Entity.API;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.Order;
using BlueLotus360.Core.Domain.Entity.UberEats;
using BlueLotus360.Core.Domain.Utility;
using BlueLotus360.Web.APIApplication.Definitions.ServiceDefinitions;
using Newtonsoft.Json;
using RestSharp;
using System.Reflection;

namespace BlueLotus360.Web.API.Integrations.Uber
{
    public class UberOrderHandler
    {
        IOrderService _orderService;
        ICodeBaseService _CodebaseService;
        IAddressService _addressService;
        public UberOrderHandler(IOrderService orderService,ICodeBaseService codeBaseService,IAddressService addressService)
        {
            _orderService = orderService;
            _CodebaseService = codeBaseService;
            _addressService = addressService;
        }
        public void GetUberDetailsByOrderID(string OrderID,string MerchantID)
        {
            APIRequestParameters ApiParams = new APIRequestParameters()
            {
                EndPointName = UberTokenEndpoints.Eats_Order_Read_Scope.GetDescription()

            };
            UberProvisionHandler uberProvisionHandler = new UberProvisionHandler(_orderService);
            UberTokenHandler uberTokenHandler = new UberTokenHandler(_orderService);
            APIInformation GetUberToken = new APIInformation();
            APIInformation APIInfo = uberProvisionHandler.GetCommonUberDetails(new Core.Domain.Entity.Base.User());
            if (APIInfo != null)
            {
                APIInformation ScopeendpointInfo = uberProvisionHandler.GetEndPoint(APIInfo.APIIntegrationKey, ApiParams.EndPointName);
                GetUberToken = uberTokenHandler.GetUberEatsTokensByEndPointName(APIInfo, ScopeendpointInfo, ApiParams.EndPointName, "", "",1);
                APIRequestParameters EndPointParams = new APIRequestParameters()
                {
                    EndPointName = UberEndpointURLS.GetOrder.ToString(),
                    APIIntegrationKey= APIInfo.APIIntegrationKey,
                    LocationKey=1,
                    BUKy=1
                };
                APIInformation endpointInfo = _orderService.GetAPIEndPoints(new Company(), EndPointParams).Value;

                var client = new RestClient(GetUberToken.BaseURL);
                var request = new RestRequest(endpointInfo.EndPointURL.Replace(UberRequestIDs.order_id.GetDescription(), OrderID),Method.Get);
                request.AddHeader("Authorization", "Bearer " + GetUberToken.EndPointToken);
                request.AddHeader("Content-Type", "application/json");
                RestResponse response = client.Execute(request);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                if (response.IsSuccessful)
                {
                    UberOrder UberOrder = JsonConvert.DeserializeObject<UberOrder>(response.Content, settings);
                    if(UberOrder != null && UberOrder.Cart.Items.Count > 0)
                    {
                       
                        ResponseDetails res = new ResponseDetails()
                        {
                            TriggerKey = 3,
                            SubscriberKey = 3,
                            ResponseCode = response.StatusDescription,
                            Response = response.Content,
                            ContenetPayload = endpointInfo.EndPointURL,
                            Reference = "Uber Orders: "+ OrderID,
                            TrnTyp = "PUOrd"

                        };
                        _orderService.APIResponseDet_InsertWeb(res);
                    }
                    SaveUberOrder(UberOrder, MerchantID);
                }
            }


        }

        public void SaveUberOrder(UberOrder uberOrder, string merchantId)
        {
            int LiNo = 0;
            try
            {
                string OurCd = "CUS";
                PartnerOrder saveUberOrder = new PartnerOrder();
                APIRequestParameters request = new APIRequestParameters()
                {
                    APIName = merchantId
                };
                APIInformation StoreInfo = _orderService.GetAPIDetailsByMerchantID(request).Value;
                Company company = new Company();
                company.CompanyKey = StoreInfo.MappedCompanyKey;
                //common order details
                //using uberOrder.Store.Id get relevant company id and location key and set to saveOrder
                saveUberOrder.Location = StoreInfo.Location;
                saveUberOrder.BU = StoreInfo.BU;
                saveUberOrder.OrderId = uberOrder.Id;
                saveUberOrder.OrderReference = uberOrder.Display_id;
                saveUberOrder.OrderDate = Convert.ToDateTime(uberOrder.Placed_at).ToString("yyyy/MM/dd hh:mm:ss tt");
                saveUberOrder.PickupTime = Convert.ToDateTime(uberOrder.Estimated_ready_for_pickup_at).ToString("yyyy/MM/dd hh:mm:ss tt");
                saveUberOrder.DeliveryBrand = uberOrder.Brand;
                saveUberOrder.IsActive = 1;
                saveUberOrder.IsApproved = 1;
                saveUberOrder.DeliveryCharges = Convert.ToDecimal(uberOrder.Payment.Charges.Delivery_fee.Amount/100);
                saveUberOrder.Platforms.AccountCode = "Uber";
                saveUberOrder.PaymentKey = Convert.ToInt32(_CodebaseService.GetCodeByOurCodeAndConditionCode(company, new User(), "UberWallet", "PmtTrm").Value.CodeKey);                                                                                                                                                                               //setting up order status
                saveUberOrder.OrderStatus.CodeName = uberOrder.Current_state;
                CodeBaseResponse code = new CodeBaseResponse()
                {
                    CodeName = saveUberOrder.OrderStatus.CodeName,
                    OurCode = saveUberOrder.Platforms.AccountCode
                };
                CodeBaseResponse record = _orderService.GetOrderStatusByPartnerStatus(company,code).Value;
                saveUberOrder.OrderStatus.CodeKey = record.CodeKey;
                saveUberOrder.OrderNote = uberOrder.Cart.Special_instructions;

                //item details
                foreach (UberItem item in uberOrder.Cart.Items)
                {
                    PartnerOrderDetails partnerOrderDetails = new PartnerOrderDetails();
                    ItemResponse itemrec = new ItemResponse()
                    {
                        ItemCode = item.Id
                    };
                    
                    partnerOrderDetails.OrderItem = _orderService.GetItemsByItemCode(company, itemrec).Value;
                    partnerOrderDetails.ItemQuantity = item.Quantity;
                    partnerOrderDetails.TransactionPrice = Decimal.Round(((decimal)item.Price.Base_total_price.Amount / 100) / item.Quantity, 2);
                    partnerOrderDetails.BaseTotalPrice = Decimal.Round(((decimal)item.Price.Base_total_price.Amount / 100), 2);
                    partnerOrderDetails.SpecialInstructions = item.Special_instructions == null ? "" : item.Special_instructions;
                    partnerOrderDetails.IsModifier = item.Selected_modifier_groups == null ? false : true;
                    partnerOrderDetails.ItemOfferKey = 1;
                    partnerOrderDetails.IsApproved = 1;
                    partnerOrderDetails.IsActive = 1;
                    partnerOrderDetails.Remarks = "";
                    LiNo = LiNo + 1;
                    partnerOrderDetails.OrderItem.LineNumber = LiNo;

                    saveUberOrder.OrderItemDetails.Add(partnerOrderDetails);
                }

                //customer details
                if (uberOrder.Cart.Items[0].Eater_id != "")
                {
                    //common function
                    string FullName = uberOrder.Eater.First_name + " " + uberOrder.Eater.Last_name;
                    AddressMaster adr = new AddressMaster()
                    {
                        AddressId = uberOrder.Cart.Items[0].Eater_id
                    };
                    AddressMaster addressMaster=_addressService.CheckAdvanceAnalysisAvailability(company, adr).Value;
                    if (addressMaster != null && addressMaster.AddressKey > 11)
                    {
                        saveUberOrder.Customer.AdrKy = Convert.ToInt32(addressMaster.AddressKey);
                        saveUberOrder.Customer.AdrId = addressMaster.AddressId;
                        saveUberOrder.Customer.Name = addressMaster.AddressName;
                        saveUberOrder.Customer.Phone = addressMaster.Mobile;
                    }
                    else
                    {
                        AddressMaster advanl = new AddressMaster();
                        advanl.AddressId = uberOrder.Cart.Items[0].Eater_id;
                        advanl.AddressName = FullName;
                        advanl.Mobile = uberOrder.Eater.Phone;
                        AddressMaster adrmst = _addressService.CreateAdvanceAnalysis(company, advanl).Value;
                        saveUberOrder.Customer.AdrKy = Convert.ToInt32(adrmst.AddressKey);
                        saveUberOrder.Customer.Name = FullName;
                        saveUberOrder.Customer.AdrId= uberOrder.Cart.Items[0].Eater_id;
                        saveUberOrder.Customer.Phone = uberOrder.Eater.Phone;
                    }
                }

                _orderService.GetOrdersFromOrderPlatforms(company, new User(), saveUberOrder);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
    }
}
