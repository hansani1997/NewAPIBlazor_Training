using BlueLotus360.Core.Domain.DTOs;
using BlueLotus360.Core.Domain.DTOs.RequestDTO;
using BlueLotus360.Core.Domain.Entity;
using BlueLotus360.Core.Domain.Entity.API;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.Order;
using BlueLotus360.Core.Domain.Entity.Transaction;
using BlueLotus360.Core.Domain.Entity.UberEats;
using BlueLotus360.Core.Domain.Entity.WorkOrder;
using BlueLotus360.Core.Domain.Responses;
using BlueLotus360.Core.Domain.Utility;
using BlueLotus360.Web.API.Authentication;
using BlueLotus360.Web.API.Extension;
using BlueLotus360.Web.API.Integrations.Uber;
using BlueLotus360.Web.APIApplication.Definitions.ServiceDefinitions;
using BlueLotus360.Web.APIApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Drawing;
using System.Reflection.Emit;
using System.Text.Json;
using System.Transactions;
using static BlueLotus360.Core.Domain.Entity.UberEats.UberWebHook;
using static BlueLotus360.Core.Domain.Entity.UberEats.UberWebHook.DelegateSubscriber;

namespace BlueLotus360.Web.API.Controllers
{
    [BLAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        ILogger<OrderController> _logger;
        IOrderService _orderService;
        IObjectService _objectService;
        ICodeBaseService _codeBaseService;
        IAddressService _addressService;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private static readonly Queue<UberWebhookResponseModel> webHookQueue = new Queue<UberWebhookResponseModel>();
        public static event IncomingWebHookEvent TriggerIncomingWebHookEvent;
        public OrderController(ILogger<OrderController> logger,
                                IOrderService orderService,
                                IObjectService objectService, ICodeBaseService codeBaseService, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IAddressService addressService)
        {
            _logger = logger;
            _orderService = orderService;
            _objectService = objectService;
            _codeBaseService = codeBaseService;
            _hostingEnvironment = hostingEnvironment;
            _addressService = addressService;
        }

        [HttpPost("createGenericOrder")]
        public IActionResult CreateGenericOrder(GenericOrder orderDetails)
        {
            //
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();
            var uiObject = _objectService.GetObjectByObjectKey(orderDetails.FormObjectKey);

            CodeBaseResponse ordTyp = new CodeBaseResponse();
            if (!string.IsNullOrEmpty(uiObject.Value.OurCode))
            {
                var ordtp = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, uiObject.Value.OurCode, "OrdTyp");
                ordTyp = ordtp.Value;
            }
            else
            {
                var ordtp = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, orderDetails.OrderType.OurCode, "OrdTyp");
                ordTyp = ordtp.Value;
            }
            if (ordTyp == null)
            {
                ordTyp = new CodeBaseResponse();
            }
            var ord = _orderService.SaveOrder(company, user, orderDetails, ordTyp);
            OrderSaveResponse orderServerResponse = ord.Value;
            return Ok(orderServerResponse);

        }

        [HttpPost("updateGenericOrder")]
        public IActionResult UpdateGenericOrder(GenericOrder orderDetails)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();
            var uiObject = _objectService.GetObjectByObjectKey(orderDetails.FormObjectKey);
            var ordTyp = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, uiObject.Value.OurCode, "OrdTyp");
            OrderSaveResponse orderServerResponse = _orderService.UpdateOrder(company, user, orderDetails, ordTyp.Value);

            return Ok(orderServerResponse);
        }

        [HttpPost("findOrders")]
        public IActionResult FindOrder(OrderFindDto request)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();
            var uiObject = _objectService.GetObjectByObjectKey((int)request.ObjectKey);
            var ordTyp = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, uiObject.Value.OurCode, "OrdTyp");

            BaseServerResponse<IList<OrderFindResults>> orders = _orderService.FindOrders(request, company, user, ordTyp.Value);
            return Ok(orders.Value);
        }

        [HttpPost("openOrder")]
        public IActionResult OpenOrder(OrderOpenRequest request)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();

            BaseServerResponse<GenericOrder> order = _orderService.OpenOrder(company, user, request);
            return Ok(order.Value);
        }



        [HttpPost("getFromQuotation")]
        public IActionResult GetFromQuotation(GetFromQuoatationDTO request)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();
            var uiObject = _objectService.GetObjectByObjectKey((int)request.ObjKy);

            var ordTyp = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, uiObject.Value.OurCode, "OrdTyp");
            var preOrdTyp = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, uiObject.Value.OurCode2, "OrdTyp");

            IList<GetFromQuotResults> QuotationList = _orderService.RetrieveQuotation(request, company, user, ordTyp.Value, preOrdTyp.Value).Value;

            return Ok(QuotationList);
        }

        [HttpPost("openQuotation")]
        public IActionResult OpenQuotation(OrderOpenRequest request)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();
            var uiObject = _objectService.GetObjectByObjectKey((int)request.ObjKy);

            var ordTyp = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, uiObject.Value.OurCode, "OrdTyp");


            GenericOrder ord = _orderService.OpenQuotation(company, user, request, ordTyp.Value).Value;

            return Ok(ord);
        }

        [HttpPost("GetPartnerCountByStatus")]
        public IActionResult GetPartnerCountByStatus(RequestParameters request)
        {
            var company = Request.GetAssignedCompany();

            int OrderCount = _orderService.PartnerOrders_Count(company, request);

            return Ok(OrderCount);
        }

        [HttpPost("GetAllPartnerOrders")]
        public IActionResult GetAllPartnerOrders(RequestParameters request)
        {
            var company = Request.GetAssignedCompany();
            var user = Request.GetAuthenticatedUser();
            IList<PartnerOrder> Orders = _orderService.GetAllPartnerOrder(company, user, request).Value;

            return Ok(Orders);
        }

        [HttpPost("GetAPIDetails")]
        public IActionResult GetAPIDetails(APIRequestParameters request)
        {
            var company = Request.GetAssignedCompany();
            if (request.APIIntegrationName == "Ubereats")
            {
                company.CompanyKey = 1;
            }
            var user = Request.GetAuthenticatedUser();
            APIInformation APIInfo = _orderService.GetAPIDetails(company, user, request).Value;

            return Ok(APIInfo);
        }

        [HttpPost("GetOrderStatus")]
        public IActionResult GetOrderStatus(ComboRequestDTO comboRequest)
        {
            var company = Request.GetAssignedCompany();
            var user = Request.GetAuthenticatedUser();
            IList<CodeBaseResponse> codes = _orderService.GetOrderStatus(company).Value;

            return Ok(codes);
        }

        [HttpPost("GetOrderEndPoints")]
        public IActionResult GetOrderEndPoints(APIRequestParameters request)
        {
            var company = Request.GetAssignedCompany();
            APIInformation codes = _orderService.GetAPIEndPoints(company, request).Value;

            return Ok(codes);
        }

        [HttpPost("GetLastOrderSyncTime")]
        public IActionResult GetLastOrderSyncTime(APIRequestParameters request)
        {
            var company = Request.GetAssignedCompany();
            PartnerOrder codes = _orderService.GetLastOrderSyncTime(company, request).Value;

            return Ok(codes);
        }

        [HttpPost("GetOrdersFromOrderPlatforms")]
        public IActionResult GetOrdersFromOrderPlatforms(PartnerOrder request)
        {
            var company = Request.GetAssignedCompany();
            var user = Request.GetAuthenticatedUser();
            PartnerOrder codes = _orderService.GetOrdersFromOrderPlatforms(company, user, request).Value;
            if (codes.PartnerOrderId > 11)
            {
                long ConfirmKy = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, "Confirm", "OrdSts").Value.CodeKey;
                long CancelKy = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, "Cancel", "OrdSts").Value.CodeKey;
                long RejectKy = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, "Reject", "OrdSts").Value.CodeKey;
                long OrdTypKy = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, "SLSORD", "OrdTyp").Value.CodeKey;
                if (company.CompanyKey == 541 && request.OrderStatus.CodeKey == ConfirmKy)
                {
                    StockInjection stockInjection = new StockInjection()
                    {
                        OrderKey = Convert.ToInt32(codes.PartnerOrderId),
                        IntegrationId = "4824fc92-10fa-4eca-a7d0-e7048892bc84",
                        RequestId = "JKLL_TST"
                    };
                    StockUpdateAfterConfirmation(stockInjection);
                }
                else if(company.CompanyKey == 541 && request.OrderStatus.CodeKey == CancelKy)
                {
                    StockInjection stockInjection = new StockInjection()
                    {
                        OrderKey = Convert.ToInt32(codes.PartnerOrderId),
                        IntegrationId = "4824fc92-10fa-4eca-a7d0-e7048892bc84",
                        RequestId = "JKLL_TST"
                    };

                    StockUpdateAfterCancellation(stockInjection);
                }
                else
                {
                    //if (request.StatusKey == ConfirmKy)
                    //{
                    //    _orderService.PostOrderHubStockResevation(request.OrderKey, Convert.ToInt32(OrdTypKy), company, user);
                    //}
                    //else if (request.StatusKey == CancelKy)
                    //{
                    //    _orderService.PostOrderHubStockResevationReversal(request.OrderKey, company, user);
                    //}
                }
            }
            return Ok(codes);
        }

        [HttpPost("GetOrderStatusByPartnerStatus")]
        public IActionResult GetOrderStatusByPartnerStatus(CodeBaseResponse request)
        {
            var company = Request.GetAssignedCompany();
            CodeBaseResponse codes = _orderService.GetOrderStatusByPartnerStatus(company, request).Value;

            return Ok(codes);
        }

        [HttpPost("GetItemsByItemCode")]
        public IActionResult GetItemsByItemCode(ItemResponse request)
        {
            var company = Request.GetAssignedCompany();
            ItemResponse codes = _orderService.GetItemsByItemCode(company, request).Value;

            return Ok(codes);
        }

        [HttpPost("GetPartnerOrdersByOrderKy")]
        public IActionResult GetPartnerOrdersByOrderKy(RequestParameters request)
        {
            var company = Request.GetAssignedCompany();
            PartnerOrder codes = _orderService.GetPartnerOrdersByOrderKy(company, request).Value;

            return Ok(codes);
        }

        [HttpPost("InsertLastOrderSync")]
        public IActionResult InsertLastOrderSync(RequestParameters request)
        {
            var company = Request.GetAssignedCompany();
            bool success = _orderService.InsertLastOrderSync(request, company);

            return Ok(success);
        }

        [HttpPost("GenerateProvisionURL")]
        public IActionResult GenerateProvisionURL(APIRequestParameters request)
        {
            var company = Request.GetAssignedCompany();
            string Redirect_uri = request.BaseURL + "Order/GenerateProvisionToken?CompanyCode=" + CryptoService.ToEncryptedData(company.CompanyKey.ToString());
            string link = "https://login.uber.com/oauth/v2/authorize?response_type=code&client_id=" + request.IntegrationID + "&scope=eats.pos_provisioning&redirect_uri=" + Redirect_uri;
            return Ok(link);
        }

        [BLAllowAnonymous]
        [HttpGet("GenerateProvisionToken")]
        public IActionResult GenerateProvisionToken(string CompanyCode, string code)
        {
            var user = Request.GetAuthenticatedUser();
            CompanyCode = CompanyCode.Replace(" ", "+");
            int mod4 = CompanyCode.Length % 4;
            if (mod4 > 0)
            {
                CompanyCode += new string('=', 4 - mod4);
            }
            string decryptedCompanyKeyAsString = CryptoService.ToDecryptedData(CompanyCode);
            int decryptedCompanyKey = Convert.ToInt32(decryptedCompanyKeyAsString);
            UberProvisionHandler uberProvisionHandler = new UberProvisionHandler(_orderService);
            UberTokenHandler uberTokenHandler = new UberTokenHandler(_orderService);
            APIInformation APIInfo = uberProvisionHandler.GetCommonUberDetails(user == null ? new Core.Domain.Entity.Base.User() : user);
            if (APIInfo != null)
            {
                APIInformation endpointInfo = uberProvisionHandler.GetEndPoint(APIInfo.APIIntegrationKey, UberEndpointURLS.AuthCode.ToString());
                uberProvisionHandler.InsertAuthEndpoint(code, APIInfo.APIIntegrationKey, endpointInfo.EndPointURL, decryptedCompanyKey);
                var RedirectURL = "https://bluelotus360.co/CoreAPI/api/Order/GenerateProvisionToken?CompanyCode=";                    //HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path+ "?CompanyCode=";
                APIInformation ScopeendpointInfo = uberProvisionHandler.GetEndPoint(APIInfo.APIIntegrationKey, UberTokenEndpoints.Eats_Provisioning_Scope.GetDescription());
                APIInformation GetProvisionToken = uberTokenHandler.GetUberEatsTokensByEndPointName(APIInfo, ScopeendpointInfo, UberTokenEndpoints.Eats_Provisioning_Scope.GetDescription(), RedirectURL, code, decryptedCompanyKey);
                if (GetProvisionToken.EndPointToken != string.Empty && GetProvisionToken.EndPointToken != null)
                {
                    return Ok("Token Generation Done");
                }
                else
                {
                    return Ok("Invalid Integration Token");
                }
                GetProvisionToken.TokenResp = new();
                return Ok("Done");
            }
            else
            {

            }

            return Ok();
        }

        [HttpPost("GenerateUberToken")]
        public IActionResult GenerateUberToken(APIRequestParameters request)
        {
            UberProvisionHandler uberProvisionHandler = new UberProvisionHandler(_orderService);
            UberTokenHandler uberTokenHandler = new UberTokenHandler(_orderService);
            APIInformation GetUberToken = new APIInformation();
            User user = Request.GetAuthenticatedUser();
            Company company = Request.GetAssignedCompany();
            APIInformation APIInfo = uberProvisionHandler.GetCommonUberDetails(user == null ? new Core.Domain.Entity.Base.User() : user);
            if (APIInfo != null)
            {
                APIInformation ScopeendpointInfo = uberProvisionHandler.GetEndPoint(APIInfo.APIIntegrationKey, request.EndPointName);
                GetUberToken = uberTokenHandler.GetUberEatsTokensByEndPointName(APIInfo, ScopeendpointInfo, request.EndPointName, "", "", company.CompanyKey);
            }
            return Ok(GetUberToken);
        }

        [HttpPost("GetUberEndPoints")]
        public IActionResult GetUberEndPoints(APIRequestParameters request)
        {
            UberProvisionHandler uberProvisionHandler = new UberProvisionHandler(_orderService);
            APIInformation GetUberEndPoint = new APIInformation();
            User user = Request.GetAuthenticatedUser();
            APIInformation APIInfo = uberProvisionHandler.GetCommonUberDetails(user == null ? new Core.Domain.Entity.Base.User() : user);
            if (APIInfo != null)
            {
                GetUberEndPoint = uberProvisionHandler.GetEndPoint(APIInfo.APIIntegrationKey, request.EndPointName);
            }
            return Ok(GetUberEndPoint);
        }

        [HttpPost("GetAPIDetailsByMerchantID")]
        public IActionResult GetAPIDetailsByMerchantID(APIRequestParameters request)
        {
            APIInformation apiinfo = _orderService.GetAPIDetailsByMerchantID(request).Value;
            return Ok(apiinfo);
        }


        [HttpPost("UberProvision_InsertUpdate")]
        public IActionResult UberProvision_InsertUpdate(APIInformation request)
        {
            var company = Request.GetAssignedCompany();
            bool success = _orderService.UberProvision_InsertUpdate(request, company);

            return Ok(success);
        }

        //[HttpPost("GetOrderMenuConfiguration")]
        //public IActionResult GetOrderMenuConfiguration(OrderMenuConfiguration request)
        //{
        //    var company = Request.GetAssignedCompany();
        //    IList<OrderMenuConfiguration> config = _orderService.GetAllOrderMenuConfiguration(company).Value;

        //    return Ok(config);
        //}

        //[HttpPost("OrderMenuConfiguration_InsertUpdate")]
        //public IActionResult OrderMenuConfiguration_InsertUpdate(OrderMenuConfiguration request)
        //{
        //    var user = Request.GetAuthenticatedUser();
        //    bool success = _orderService.OrderMenuConfiguration_InsertUpdate(user,request);

        //    return Ok(success);
        //}

        [HttpPost("OrderHubStatus_UpdateWeb")]
        public IActionResult OrderHubStatus_UpdateWeb(RequestParameters request)
        {
            bool success = false;
            var user = Request.GetAuthenticatedUser();
            Company company = Request.GetAssignedCompany();
            success = _orderService.OrderHubStatus_UpdateWeb(request, user);
            if (success)
            {
                long ConfirmKy = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, "Confirm", "OrdSts").Value.CodeKey;
                long CancelKy = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, "Cancel", "OrdSts").Value.CodeKey;
                long RejectKy = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, "Reject", "OrdSts").Value.CodeKey;
                long OrdTypKy = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, "SLSORD", "OrdTyp").Value.CodeKey;
                if (company.CompanyKey == 541 && request.StatusKey == ConfirmKy)
                {
                    StockInjection stockInjection = new StockInjection()
                    {
                        OrderKey = request.OrderKey,
                        IntegrationId = "4824fc92-10fa-4eca-a7d0-e7048892bc84",
                        RequestId = "JKLL_TST"
                    };
                    success = StockUpdateAfterConfirmation(stockInjection);
                }
                else if (company.CompanyKey == 541 && request.StatusKey == CancelKy)
                {
                    StockInjection stockInjection = new StockInjection()
                    {
                        OrderKey = request.OrderKey,
                        IntegrationId = "4824fc92-10fa-4eca-a7d0-e7048892bc84",
                        RequestId = "JKLL_TST"
                    };

                    StockUpdateAfterCancellation(stockInjection);
                }
                else
                {
                    //if (request.StatusKey == ConfirmKy)
                    //{
                    //    _orderService.PostOrderHubStockResevation(request.OrderKey, Convert.ToInt32(OrdTypKy), company, user);
                    //}
                    //else if (request.StatusKey == CancelKy)
                    //{
                    //    _orderService.PostOrderHubStockResevationReversal(request.OrderKey, company, user);
                    //}
                }

            }
            return Ok(success);
        }

        private string SetupItemPicUrl(PartnerMenuItem menuItem, byte[] imgArr, string URL)
        {
            try
            {
                Company company = Request.GetAssignedCompany();
                string folderPathforUrl = string.Empty;
                string imageFileName = string.Empty;
                
                
                    string folderPath = Path.Combine(URL, "/" + CryptoService.ToEncryptedData(company.CompanyKey.ToString()) + "/");
                    folderPathforUrl = URL + "/" + CryptoService.ToEncryptedData(company.CompanyKey.ToString()) + "/";
                    imageFileName = menuItem.ItemCode + ".jpg";
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    //as a url
                    using (MemoryStream ms = new MemoryStream(imgArr))
                    {
                        ms.Position = 0;
                        Image img = Image.FromStream(ms);
                        string finalImgPath = folderPath + imageFileName;

                        if (!System.IO.File.Exists(finalImgPath))
                        {

                            img.Save(finalImgPath, System.Drawing.Imaging.ImageFormat.Png);
                        }


                    }
                

                return folderPathforUrl + imageFileName;
            }
            catch (Exception e)
            {
                return "";
            }
            finally
            {

            }




            //menuItem.ItemImageUrl = folderPathforUrl + imageFileName;



        }

        [HttpPost("GetAllOrderMenuItems")]
        public IActionResult GetAllOrderMenuItems(RequestParameters request)
        {
            var company = Request.GetAssignedCompany();
            IList<PartnerMenuItem> items = _orderService.GetAllOrderMenuItems(company, request).Value;
            if (items.Count > 0)
            {
                foreach (PartnerMenuItem item in items)
                {
                    if (item.imageArr != null)
                    {
                        item.ItemImage = Convert.ToBase64String(item.imageArr, 0, item.imageArr.Length);
                        item.ItemImageUrl = SetupItemPicUrl(item, item.imageArr, request.PlatformName);
                    }
                    if (company.CompanyKey == 541 && item.imageArr == null)
                    {
                        if (System.IO.File.Exists("https://bluelotus360.co/3pl/nst/" + item.ItemCode + ".jpg"))
                        {
                            item.ItemImageUrl = "https://bluelotus360.co/3pl/nst/" + item.ItemCode + ".jpg";
                        }
                    }

                }
            }


            return Ok(items);
        }

        [HttpPost("GetNextOrderHubStatusByStatusKey")]
        public IActionResult GetNextOrderHubStatusByStatusKey(ComboRequestDTO request)
        {
            var company = Request.GetAssignedCompany();
            object StatusKey;
            int OrdStsKy = 1;

            if (request.AddtionalData.TryGetValue("StatusKey", out StatusKey))
            {
                long value = 1;
                value = Convert.ToInt64(StatusKey.ToString());
                OrdStsKy = Convert.ToInt32(value);
            }
            IList<CodeBaseResponse> items = _orderService.GetNextOrderHubStatusByStatusKey(company, request, OrdStsKy).Value;
            return Ok(items);
        }


        [BLAllowAnonymous]
        [HttpPost("UberWebhook")]
        public IActionResult UberWebhook(UberWebhookResponseModel model)
        {

            /*
             * 1. set incoming webhook to the webhook queue
             * 2. check for duplicate events
             * 3. trigger webhook handleing event
             * 4. send 200 ok response to uber
             * 5. process webhook handling method
             * 6. unsubscribe event
            */

            var duplicateEvent = webHookQueue.FirstOrDefault(x => x.Event_id == model.Event_id);
            if (duplicateEvent == null)
            {
                //2
                webHookQueue.Enqueue(model);

                //3
                TriggerIncomingWebHookEvent += HandleWebHookEvent;
                TriggerIncomingWebHookEvent.Invoke(new EventArgs());
            }



            //4

            return Ok();





        }

        //5
        [BLAllowAnonymous]
        [HttpPost("HandleWebHookEvent")]
        public void HandleWebHookEvent(EventArgs args)
        {
            try
            {

                foreach (UberWebhookResponseModel model in webHookQueue)
                {
                    /*
                     * 1.first check the event type
                     * 2.if notification event call use resource id to call GetUberOrderDetailsByOrderId() in UberOrderApiHandler and save order in database
                     * 3.if cancel event call change the order status in db using resource id
                     * 4. proceesed webhook dequeue from the webhook queue
                     * 5.event needed to trigger notify the react frontend
                    */
                    UberOrderHandler orderHandler = new UberOrderHandler(_orderService, _codeBaseService, _addressService);
                    APIRequestParameters request = new APIRequestParameters()
                    {
                        APIName = model.Meta.User_id
                    };

                    APIInformation StoreInfo = _orderService.GetAPIDetailsByMerchantID(request).Value;
                    //1 & 2
                    if (model.Event_type == "orders.notification")
                    {
                        orderHandler.GetUberDetailsByOrderID(model.Meta.Resource_id, model.Meta.User_id);
                    }

                    //1 & 3
                    if (model.Event_type == "orders.cancel")
                    {
                        //companykey and locationkey must be extract from store id which comes with model.Meta.User_id
                        RequestParameters orderreq = new RequestParameters()
                        {
                            OrderID = model.Meta.Resource_id
                        };
                        Company company = new Company();
                        company.CompanyKey = StoreInfo.MappedCompanyKey;
                        PartnerOrder partnerorder = _orderService.GetPartnerOrdersByOrderID(company, orderreq).Value;
                        IList<CodeBaseResponse> orderStatusList = _orderService.GetOrderStatus(company).Value;
                        CodeBaseResponse CancelStatus = orderStatusList.Where(x => x.CodeName == "Cancelled").FirstOrDefault();
                        RequestParameters updateeq = new RequestParameters()
                        {
                            StatusKey = Convert.ToInt32(CancelStatus.CodeKey),
                            OrderKey = Convert.ToInt32(partnerorder.PartnerOrderId)
                        };
                        _orderService.OrderHubStatus_UpdateWeb(updateeq, new Core.Domain.Entity.Base.User());

                    }

                    if (model.Event_type == "store.provisioned")
                    {

                    }

                    //4
                    webHookQueue.Dequeue();
                    //webHookQueue.Peek();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (webHookQueue.Count() > 0)
                {

                    webHookQueue.Dequeue();
                }
                //webHookQueue.Peek();

            }
            finally
            {
                TriggerIncomingWebHookEvent -= HandleWebHookEvent;
            }


        }

        private bool StockUpdateAfterConfirmation(StockInjection stockInjection)
        {
            string Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            var client = new RestClient("https://bl360x.com/BLEFutureAPI/api/");
            var request = new RestRequest("Reconciliation/PorpergateStockTransactions", Method.Post);
            request.AddHeader("Timestamp", Timestamp);
            request.AddHeader("Authorization", "Bearer 6a92fb8b0532d2370aef1f912f72568dcda21c6853a6dbc2be531fcb02002e5c");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", JsonConvert.SerializeObject(stockInjection), ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            if (response.IsSuccessful)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpPost("GetOrderHubBU")]
        public IActionResult GetOrderHubBU()
        {
            var company = Request.GetAssignedCompany();

            IList<CodeBaseResponse> items = _orderService.GetOrderHubBU(company).Value;
            return Ok(items);
        }

        [HttpPost("GetAvailablePickmeOrders")]
        public IActionResult GetAvailablePickmeOrders(RequestParameters request)
        {
            var company = Request.GetAssignedCompany();

            IList<PartnerOrder> items = _orderService.GetAvailablePickmeOrders(company, request).Value;
            return Ok(items);
        }

        [HttpPost("APIResponseDet_InsertWeb")]
        public IActionResult APIResponseDet_InsertWeb(ResponseDetails request)
        {
            var company = Request.GetAssignedCompany();
            request.Reference = request.Reference + company.CompanyKey.ToString();
            bool success = _orderService.APIResponseDet_InsertWeb(request);
            return Ok(success);
        }

        private bool StockUpdateAfterCancellation(StockInjection stockInjection)
        {
            string Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            var client = new RestClient("https://bl360x.com/BLEFutureAPI/api/");
            var request = new RestRequest("Reconciliation/PorpergateOrderCancelation", Method.Post);
            request.AddHeader("Timestamp", Timestamp);
            request.AddHeader("Authorization", "Bearer 6a92fb8b0532d2370aef1f912f72568dcda21c6853a6dbc2be531fcb02002e5c");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", JsonConvert.SerializeObject(stockInjection), ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            if (response.IsSuccessful)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
