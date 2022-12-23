using BlueLotus360.Core.Domain.Definitions.DataLayer;
using BlueLotus360.Core.Domain.DTOs;
using BlueLotus360.Core.Domain.Entity.API;
using BlueLotus360.Core.Domain.Entity;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.Order;
using BlueLotus360.Core.Domain.Entity.WorkOrder;
using BlueLotus360.Core.Domain.Responses;
using BlueLotus360.Web.APIApplication.Definitions.ServiceDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueLotus360.Core.Domain.Entity.UberEats;
using BlueLotus360.Core.Domain.DTOs.RequestDTO;
using BlueLotus360.Core.Domain.Entity.Transaction;

namespace BlueLotus360.Web.APIApplication.Services
{
    public class OrderService:IOrderService
    {
        public readonly IUnitOfWork _unitOfWork;
        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public BaseServerResponse<OrderSaveResponse> SaveOrder(Company company, User user,GenericOrder orderDetails,CodeBaseResponse ordTyp)
        {
            OrderHeaderCreateDTO OH = new OrderHeaderCreateDTO();
            OH.DocumentNumber = orderDetails.OrderDocumentNumber;
            OH.AddressKey = orderDetails.OrderCustomer.AddressKey;
            OH.ObjectKey = orderDetails.FormObjectKey;
            OH.AprroceStatusKey = 1;
            // OH.OrderStatusKey = OrderStatus.CodeKey;
            //OH.DeliveryDate = orderDetails.DeliveryDate;
            OH.OrderDate = DateTime.Now;
            OH.OrderType = new CodeBaseResponse();
            OH.OrderType.CodeKey = ordTyp.CodeKey;
            OH.AccountKey = 1;
            OH.Description = orderDetails.HeaderDescription;
            OH.PayementTerm = new CodeBaseResponse();
            OH.PayementTerm.CodeKey = orderDetails.OrderPaymentTerm.CodeKey;
            OH.Location2Key = orderDetails.OrderLocation.CodeKey;
            OH.OrderLocation = new CodeBaseResponse();
            OH.OrderLocation.CodeKey = orderDetails.OrderLocation.CodeKey;
            OH.RepAddessKey = orderDetails.OrderRepAddress.AddressKey;
            OH.DiscountPercentage = orderDetails.HeaderLevelDisountPrecentage;
            OH.IsActive = 1;
            OH.IsApproved = 1;
            OH.OrderCategory1Key =(int)orderDetails.OrderCategory1.CodeKey;
            OH.OrderCategory2Key = (int)orderDetails.OrderCategory2.CodeKey;
            OH.OrderCategory3Key = (int)orderDetails.OrderCategory3.CodeKey;
            OH.ProjectKey = (int)orderDetails.OrderProject.ProjectKey;
            OH.Code1Key = orderDetails.Cd1Ky;
            OH.AdrCat3Ky = (int)orderDetails.AddressCategory3.CodeKey;
            OH.FromOrderKey= orderDetails.FromOrderKey;

            if (!BaseComboResponse.IsEntityWithDefaultValue(orderDetails.OrderAccount))
            {
                OH.AccountKey = orderDetails.OrderAccount.AccountKey;
            }
            else
            {
                var acc = _unitOfWork.AccountRepository.GetAccountByAddress(orderDetails.OrderCustomer, company, user);
                OH.AccountKey = acc.Value.AccountKey;

            }

            IList<OrderLineCreateDTO> orderLineItems = new List<OrderLineCreateDTO>();
            _unitOfWork.OrderRepository.CreateOrder(OH, company, user);

            if (orderDetails.OrderItems.Count()>0)
            {
                foreach (GenericOrderItem item in orderDetails.OrderItems)
                {
                    OrderLineCreateDTO lineItem = new OrderLineCreateDTO();
                    lineItem.ItemKey = item.TransactionItem.ItemKey;
                    lineItem.OrderKey = OH.OrderKey;
                    lineItem.TransactionQuantity = item.TransactionQuantity;
                    lineItem.Rate = _unitOfWork.ItemRepository.GetCostPriceByLocAndItmKy(company, new CodeBaseResponse((int)orderDetails.OrderLocation.CodeKey), DateTime.Now, item.TransactionItem.ItemKey);
                    lineItem.TransactionRate = item.TransactionRate;
                    lineItem.AddressKey = OH.AddressKey;

                    lineItem.ObjectKey = orderDetails.FormObjectKey;
                    lineItem.OrderLineLocation = new CodeBaseResponse();
                    lineItem.OrderLineLocation.CodeKey = item.OrderLineLocation.CodeKey;
                    lineItem.CompanyKey = company.CompanyKey;
                    lineItem.UserKey = user.UserKey;
                    lineItem.DiscountPercentage = item.DiscountPercentage;
                    lineItem.AccountKey = OH.AccountKey;
                    //  lineItem.TransactionDiscountAmount = Math.Abs(item.GetLineDiscount());
                    lineItem.LineNumber = item.LineNumber;
                    lineItem.IsActive = item.IsActive;
                    lineItem.OriginalQuantity = item.RequestedQuantity;


                    lineItem.IsApproved = item.IsApproved;
                    lineItem.OrderType = new CodeBaseResponse();
                    lineItem.OrderType.CodeKey = ordTyp.CodeKey;
                    lineItem.IsTransfer = item.IsTransfer;
                    lineItem.IsConfirmed = item.IsTransferConfirmed;

                    lineItem.DisocuntAmount = item.DiscountAmount;
                    lineItem.TransactionDiscountAmount = item.DiscountAmount;
                    lineItem.ItemTaxType1 = item.ItemTaxType1;
                    lineItem.ItemTaxType2 = item.ItemTaxType2;
                    lineItem.ItemTaxType3 = item.ItemTaxType3;
                    lineItem.ItemTaxType4 = item.ItemTaxType4;
                    lineItem.ItemTaxType5 = item.ItemTaxType5;

                    lineItem.ItemTaxType1Per = item.ItemTaxType1Per;
                    lineItem.ItemTaxType2Per = item.ItemTaxType2Per;
                    lineItem.ItemTaxType3Per = item.ItemTaxType3Per;
                    lineItem.ItemTaxType4Per = item.ItemTaxType4Per;
                    lineItem.ItemTaxType5Per = item.ItemTaxType5Per;
                    lineItem.Remarks = item.Remark;
                    lineItem.Description = item.Description;
                    lineItem.FromOrderDetailKey= item.FromOrderDetailKey;   
                    //lineItem.FrmOrdDetKy = item.FromOrderDetKy;   

                    //   TotalDiscount += Math.Abs(item.GetLineDiscount()));
                    _unitOfWork.OrderRepository.CreateOrderLineItem(lineItem, company, user, new UIObject() { ObjectId = orderDetails.FormObjectKey });

                }
            }
            

            _unitOfWork.OrderRepository.PostInterLocationTransfers(company, user, OH.OrderKey, orderDetails.FormObjectKey);


            var ordb = _unitOfWork.OrderRepository.GetOrderByOrderKey(OH.OrderKey, user);
            OrderSaveResponse orderServerResponse = new OrderSaveResponse();
            orderServerResponse.OrderKey = ordb.OrderKey;
            orderServerResponse.OrderNumber = ordb.OrderReference.ToString();
            orderServerResponse.Prefix = ordb.Prefix;

            BaseServerResponse<OrderSaveResponse> orderSaveResponse=new BaseServerResponse<OrderSaveResponse>();
            orderSaveResponse.Value = orderServerResponse;

            return orderSaveResponse;
        }

        public OrderSaveResponse UpdateOrder(Company company, User user, GenericOrder orderDetails, CodeBaseResponse ordTyp)
        {
            OrderHeaderEditDTO OH = new OrderHeaderEditDTO();
            OH.OrderKey = orderDetails.OrderKey;
            OH.OrderLocation = new CodeBaseResponse();
            OH.OrderLocation.CodeKey = orderDetails.OrderLocation.CodeKey;
            OH.OrderDate = orderDetails.OrderDate;
            OH.OrderType = new CodeBaseResponse();
            OH.OrderType.CodeKey = ordTyp.CodeKey;
            OH.PayementTerm = new CodeBaseResponse();
            OH.PayementTerm.CodeKey = orderDetails.OrderPaymentTerm.CodeKey;
            OH.DocumentNumber = orderDetails.OrderDocumentNumber;
            OH.DiscountPercentage = orderDetails.HeaderLevelDisountPrecentage;
            OH.OrderAdress = orderDetails.OrderCustomer;
            OH.RepAdress = orderDetails.OrderRepAddress;
            OH.IsActive = orderDetails.IsActive;
            OH.IsApproved = orderDetails.IsApproved;
            OH.OrderNumber = Convert.ToInt32(orderDetails.OrderNumber);
            OH.BussinessUnit = orderDetails.BussinessUnit;
            OH.ObjectKey = orderDetails.FormObjectKey;
            OH.ApproveStatus = orderDetails.OrderApproveState;
            OH.AccountKey = 1;
            OH.Description = orderDetails.HeaderDescription;
            OH.FromOrderKey = orderDetails.FromOrderKey;
            OH.OrderCategory3Key = (int)orderDetails.OrderCategory3.CodeKey;

            if (!BaseComboResponse.IsEntityWithDefaultValue(orderDetails.OrderAccount))
            {
                OH.AccountKey = orderDetails.OrderAccount.AccountKey;
            }
            else
            {
                var acc = _unitOfWork.AccountRepository.GetAccountByAddress(orderDetails.OrderCustomer, company, user);
                OH.AccountKey = acc.Value.AccountKey;
            }

            _unitOfWork.OrderRepository.UpdateGenericOrderHeader(OH, company, user);


            foreach (GenericOrderItem item in orderDetails.OrderItems)
            {
                OrderLineCreateDTO lineItem = new OrderLineCreateDTO();
                if (item.FromOrderDetailKey > 1)
                {

                    lineItem.OrderKey = OH.OrderKey;
                    lineItem.FromOrderDetailKey = item.FromOrderDetailKey;
                    lineItem.OrderType = new CodeBaseResponse();
                    lineItem.OrderType.CodeKey = item.OrderType.CodeKey;
                    lineItem.TransactionQuantity = item.TransactionQuantity;
                    lineItem.Rate = _unitOfWork.ItemRepository.GetCostPriceByLocAndItmKy(company, new CodeBaseResponse((int)orderDetails.OrderLocation.CodeKey), DateTime.Now, item.TransactionItem.ItemKey);
                    lineItem.TransactionRate = item.TransactionRate;
                    lineItem.AddressKey = OH.AddressKey;
                    lineItem.IsActive = item.IsActive;
                    lineItem.IsApproved = item.IsApproved;
                    lineItem.OrderLineLocation = new CodeBaseResponse();
                    lineItem.OrderLineLocation.CodeKey = item.OrderLineLocation.CodeKey;
                    lineItem.BussinessUnitKey = (int)item.BussinessUnit.CodeKey;
                    lineItem.AccountKey = OH.AccountKey;
                    lineItem.LineNumber = item.LineNumber;
                    lineItem.ItemKey = item.TransactionItem != null ? item.TransactionItem.ItemKey : 1;
                    lineItem.OrderItemName = item.TransactionItem != null ? item.TransactionItem.ItemName : "";
                    lineItem.Rate = item.Rate;
                    lineItem.TransactionRate = item.TransactionRate;
                    lineItem.DiscountPercentage = item.DiscountPercentage;
                    lineItem.DisocuntAmount = item.DiscountAmount;
                    lineItem.ObjectKey = orderDetails.FormObjectKey;
                    lineItem.CompanyKey = company.CompanyKey;
                    lineItem.UserKey = user.UserKey;
                    lineItem.OriginalQuantity = item.RequestedQuantity;
                    lineItem.IsTransfer = item.IsTransfer;
                    lineItem.IsConfirmed = item.IsTransferConfirmed;

                    lineItem.ItemTaxType1 = item.ItemTaxType1;
                    lineItem.ItemTaxType2 = item.ItemTaxType2;
                    lineItem.ItemTaxType3 = item.ItemTaxType3;
                    lineItem.ItemTaxType4 = item.ItemTaxType4;
                    lineItem.ItemTaxType5 = item.ItemTaxType5;

                    lineItem.ItemTaxType1Per = item.ItemTaxType1Per;
                    lineItem.ItemTaxType2Per = item.ItemTaxType2Per;
                    lineItem.ItemTaxType3Per = item.ItemTaxType3Per;
                    lineItem.ItemTaxType4Per = item.ItemTaxType4Per;
                    lineItem.ItemTaxType5Per = item.ItemTaxType5Per;
                    //lineItem.FrmOrdDetKy = item.FromOrderDetKy;

                    _unitOfWork.OrderRepository.UpdateGenericOrderLineItem(lineItem, orderDetails.FormObjectKey, company, user);
                }
                else
                {
                    lineItem.ItemKey = item.TransactionItem.ItemKey;
                    lineItem.OrderKey = OH.OrderKey;
                    lineItem.TransactionQuantity = item.TransactionQuantity;
                    lineItem.Rate = _unitOfWork.ItemRepository.GetCostPriceByLocAndItmKy(company, new CodeBaseResponse((int)orderDetails.OrderLocation.CodeKey), DateTime.Now, item.TransactionItem.ItemKey);
                    lineItem.TransactionRate = item.TransactionRate;
                    lineItem.AddressKey = OH.AddressKey;
                    lineItem.ObjectKey = orderDetails.FormObjectKey;
                    lineItem.OrderLineLocation = new CodeBaseResponse();
                    lineItem.OrderLineLocation.CodeKey = item.OrderLineLocation.CodeKey;
                    lineItem.CompanyKey = company.CompanyKey;
                    lineItem.UserKey = user.UserKey;
                    lineItem.DiscountPercentage = item.DiscountPercentage;
                    lineItem.AccountKey = OH.AccountKey;
                    lineItem.LineNumber = item.LineNumber;
                    lineItem.IsActive = 1;
                    lineItem.OriginalQuantity = item.RequestedQuantity;
                    lineItem.IsApproved = 1;
                    lineItem.OrderType = new CodeBaseResponse();
                    lineItem.OrderType.CodeKey = ordTyp.CodeKey;
                    lineItem.IsTransfer = item.IsTransfer;
                    lineItem.IsConfirmed = item.IsTransferConfirmed;
                    lineItem.DisocuntAmount = item.DiscountAmount;
                    lineItem.TransactionDiscountAmount = item.DiscountAmount;
                    lineItem.ItemTaxType1 = item.ItemTaxType1;
                    lineItem.ItemTaxType2 = item.ItemTaxType2;
                    lineItem.ItemTaxType3 = item.ItemTaxType3;
                    lineItem.ItemTaxType4 = item.ItemTaxType4;
                    lineItem.ItemTaxType5 = item.ItemTaxType5;

                    lineItem.ItemTaxType1Per = item.ItemTaxType1Per;
                    lineItem.ItemTaxType2Per = item.ItemTaxType2Per;
                    lineItem.ItemTaxType3Per = item.ItemTaxType3Per;
                    lineItem.ItemTaxType4Per = item.ItemTaxType4Per;
                    lineItem.ItemTaxType5Per = item.ItemTaxType5Per;
                    //lineItem.FrmOrdDetKy = item.FromOrderDetKy;


                    _unitOfWork.OrderRepository.CreateOrderLineItem(lineItem, company, user, new UIObject() { ObjectId = orderDetails.FormObjectKey });
                }


            }

            _unitOfWork.OrderRepository.PostInterLocationTransfers(company,user, OH.OrderKey, orderDetails.FormObjectKey);
            
            var ordb = _unitOfWork.OrderRepository.GetOrderByOrderKey(OH.OrderKey, user);
            OrderSaveResponse orderServerResponse = new OrderSaveResponse();
            orderServerResponse.OrderKey = ordb.OrderKey;
            orderServerResponse.OrderNumber = ordb.OrderReference.ToString();
            orderServerResponse.Prefix = ordb.Prefix;

            

            return orderServerResponse;
        }

        public BaseServerResponse<IList<OrderFindResults>> FindOrders(OrderFindDto request, Company company, User user, CodeBaseResponse ordTyp) 
        {
            request.OrderTypeKey =(int)ordTyp.CodeKey;
            if (!string.IsNullOrEmpty(request.OrderNo))
            {
                request.FromOrderNumber = Convert.ToInt32(request.OrderNo);
            }
            else
            {
                request.FromOrderNumber = 0;
            }
            BaseServerResponse<IList<OrderFindResults>> responses = _unitOfWork.OrderRepository.GenericFindOrders(request, company, user);
             
            return responses;
        }

        public BaseServerResponse<GenericOrder> OpenOrder(Company company, User user, OrderOpenRequest request)
        {
            var ord = _unitOfWork.OrderRepository.GetGenericOrderByOrderKey(request.OrderKey,company, user);
            OrderHeaderEditDTO responses = ord.Value;

            var ordDet= _unitOfWork.OrderRepository.GetGenericOrderLineItemsByOrderKey(request.OrderKey, responses.ObjectKey, company, user);
            IList<OrderLineCreateDTO> itemList = ordDet.Value;

            GenericOrder order = new GenericOrder();
            order.OrderLocation = responses.OrderLocation;
            order.OrderCustomer = responses.OrderAdress;
            order.OrderRepAddress = responses.RepAdress;
            order.OrderAccount = new AccountResponse();
            order.HeaderLevelDisountPrecentage = responses.DiscountPercentage;
            order.OrderKey = responses.OrderKey;
            order.OrderNumber = responses.OrderNumber.ToString();
            order.OrderDocumentNumber = responses.DocumentNumber;
            order.OrderDate = responses.OrderDate;
            order.OrderPaymentTerm = responses.PayementTerm;
            order.OrderType = responses.OrderType;
            order.FormObjectKey = responses.ObjectKey;
            order.IsActive = responses.IsActive;
            order.IsApproved = responses.IsApproved;
            order.HeaderDescription = responses.Description;
            order.OrderPrefix = responses.OrderPrefix;
            order.OrderApproveState = _unitOfWork.OrderRepository.OrderApproveStatusFindByOrdKy(company, user, order.FormObjectKey, order.OrderKey);

            foreach (OrderLineCreateDTO item in itemList)
            {
                if (item.IsActive == 1)
                {
                    GenericOrderItem lineItem = new GenericOrderItem();
                    lineItem.TransactionItem = new ItemResponse() { ItemKey = item.ItemKey, ItemName = item.OrderItemName };
                    lineItem.TransactionQuantity = item.TransactionQuantity;
                    lineItem.Rate = _unitOfWork.ItemRepository.GetCostPriceByLocAndItmKy(company, new CodeBaseResponse(item.OrderLineLocation != null ? (int)item.OrderLineLocation.CodeKey : 1), DateTime.Now, item.ItemKey);
                    lineItem.TransactionRate = item.TransactionRate;
                    lineItem.ObjectKey = item.ObjectKey;
                    lineItem.OrderLineLocation = item.OrderLineLocation;
                    lineItem.CompanyKey = company.CompanyKey;
                    lineItem.UserKey = user.UserKey;
                    lineItem.DiscountPercentage = item.DiscountPercentage;
                    lineItem.LineNumber = item.LineNumber;
                    lineItem.IsActive = item.IsActive;
                    lineItem.RequestedQuantity = item.OriginalQuantity;
                    lineItem.IsApproved = item.IsApproved;
                    lineItem.IsTransfer = item.IsTransfer;
                    lineItem.IsTransferConfirmed = item.IsConfirmed;
                    lineItem.DiscountAmount = item.DisocuntAmount;
                    lineItem.ItemTaxType1 = item.ItemTaxType1;
                    lineItem.ItemTaxType2 = item.ItemTaxType2;
                    lineItem.ItemTaxType3 = item.ItemTaxType3;
                    lineItem.ItemTaxType4 = item.ItemTaxType4;
                    lineItem.ItemTaxType5 = item.ItemTaxType5;
                    lineItem.ItemTaxType1Per = item.ItemTaxType1Per;
                    lineItem.ItemTaxType2Per = item.ItemTaxType2Per;
                    lineItem.ItemTaxType3Per = item.ItemTaxType3Per;
                    lineItem.ItemTaxType4Per = item.ItemTaxType4Per;
                    lineItem.ItemTaxType5Per = item.ItemTaxType5Per;
                    lineItem.FromOrderDetailKey = item.FromOrderDetailKey;
                    lineItem.Remark = item.Remarks;
                    lineItem.Description = item.Description;
                    
                    order.OrderItems.Add(lineItem);
                }

            }

            BaseServerResponse<GenericOrder> response= new BaseServerResponse<GenericOrder>();
            response.Value = order;

            return response;
        }

        public BaseServerResponse<IList<GetFromQuotResults>> RetrieveQuotation(GetFromQuoatationDTO request, Company company, User user,CodeBaseResponse ordTyp,CodeBaseResponse preOrdTyp)
        {
            request.ToDate = DateTime.Now;
            request.OrdTypKy = (int)ordTyp.CodeKey;
            request.PreOrdTypKy = (int)preOrdTyp.CodeKey;
            request.PreOrdPreFixKy = (int)preOrdTyp.CodeKey;

            if (!string.IsNullOrEmpty(request.SoNo))
            {
                request.OrdNo = Convert.ToInt32(request.SoNo);
            }
            else
            {
                request.OrdNo = 0;
            }
            return  _unitOfWork.OrderRepository.GenericRetrieveQuotation(request, company, user);
        }

        public BaseServerResponse<GenericOrder> OpenQuotation(Company company, User user, OrderOpenRequest request,CodeBaseResponse ordTyp)
        {
            request.BaseTypKy = ordTyp.CodeKey;
            var quot = _unitOfWork.OrderRepository.GenericOpenQuotation(request, company, user);
            IList<QuotationDetails> responses=quot.Value;

            GenericOrder order = new GenericOrder();
            QuotationDetails qt = responses.FirstOrDefault();

            if (qt != null)
            {
                order.OrderLocation = qt.HdrLocation;
                order.OrderCustomer = qt.OrderAdress;
                order.OrderRepAddress = qt.RepAdress;
                order.OrderAccount = new AccountResponse();
                order.HeaderLevelDisountPrecentage = 0;
                order.OrderNumber = qt.OrdNo.ToString();
                order.OrderDocumentNumber = qt.DocNo;
                order.OrderDate = qt.OrderDate;
                order.OrderPaymentTerm = new CodeBaseResponse();
                order.OrderType = qt.OrderType;
                order.FormObjectKey = (int)request.ObjKy;
                order.IsActive = qt.IsActive;
                order.IsApproved = 1;
                order.OrderDetKy = qt.FromOrderDetailKey;
                order.FromOrderKey = qt.OrderKey;
                order.IsFromQuotation = true;
            }
            foreach (QuotationDetails item in responses)
            {
                GenericOrderItem lineItem = new GenericOrderItem();
                lineItem.TransactionItem = new ItemResponse() { ItemKey = item.ItemKey, ItemName = item.OrderItemName };
                lineItem.TransactionQuantity = item.TransactionQuantity;
                lineItem.Rate = _unitOfWork.ItemRepository.GetCostPriceByLocAndItmKy(company, new CodeBaseResponse(item.OrderLineLocation != null ? (int)item.OrderLineLocation.CodeKey : 1), DateTime.Now, item.ItemKey);
                lineItem.TransactionRate = item.TransactionRate;
                lineItem.ObjectKey = (int)request.ObjKy;
                lineItem.OrderLineLocation = item.OrderLineLocation;
                lineItem.CompanyKey = company.CompanyKey;
                lineItem.UserKey = user.UserKey;
                lineItem.DiscountPercentage = item.DiscountPercentage;
                lineItem.LineNumber = item.LineNumber;
                lineItem.IsActive = item.IsActive;
                lineItem.IsApproved = 1;
                lineItem.IsTransfer = 0;
                lineItem.IsTransferConfirmed = 0;
                lineItem.DiscountAmount = item.DisocuntAmount;
                lineItem.ItemTaxType1 = item.ItemTaxType1;
                lineItem.ItemTaxType2 = item.ItemTaxType2;
                lineItem.ItemTaxType3 = item.ItemTaxType3;
                lineItem.ItemTaxType4 = item.ItemTaxType4;
                lineItem.ItemTaxType5 = item.ItemTaxType5;
                lineItem.ItemTaxType1Per = item.ItemTaxType1Per;
                lineItem.ItemTaxType2Per = item.ItemTaxType2Per;
                lineItem.ItemTaxType3Per = item.ItemTaxType3Per;
                lineItem.ItemTaxType4Per = item.ItemTaxType4Per;
                lineItem.ItemTaxType5Per = item.ItemTaxType5Per;
                lineItem.FromOrderDetailKey = item.FromOrderDetailKey;
                lineItem.AvailableStock = item.AvailableQuantity;
                order.OrderItems.Add(lineItem);


            }
            BaseServerResponse<GenericOrder> quote = new BaseServerResponse<GenericOrder>();
            quote.Value = order;

            return quote;
        }
        public int PartnerOrders_Count(Company company, RequestParameters partnerOrder)
        {
            return _unitOfWork.OrderRepository.PartnerOrders_Count(company, partnerOrder);
        }

        public BaseServerResponse<IList<PartnerOrder>> GetAllPartnerOrder(Company company, User user, RequestParameters order)
        {
            return _unitOfWork.OrderRepository.GetAllPartnerOrder(company, user, order);
        }

        public BaseServerResponse<APIInformation> GetAPIDetails(Company company, User user, APIRequestParameters request)
        {
            return _unitOfWork.OrderRepository.GetAPIDetails(company, user, request);
        }

        public BaseServerResponse<IList<CodeBaseResponse>> GetOrderStatus(Company company)
        {
            return _unitOfWork.OrderRepository.GetOrderStatus(company);
        }

        public BaseServerResponse<APIInformation> GetAPIEndPoints(Company company, APIRequestParameters request)
        {
            return _unitOfWork.OrderRepository.GetAPIEndPoints(company, request);
        }

        public BaseServerResponse<PartnerOrder> GetLastOrderSyncTime(Company company, APIRequestParameters request)
        {
            return _unitOfWork.OrderRepository.GetLastOrderSyncTime(company, request);
        }

        public BaseServerResponse<PartnerOrder> GetOrdersFromOrderPlatforms(Company company, User user, PartnerOrder request)
        {
            return _unitOfWork.OrderRepository.GetOrdersFromOrderPlatforms(company,user, request);
        }

        public BaseServerResponse<CodeBaseResponse> GetOrderStatusByPartnerStatus(Company company, CodeBaseResponse codeBase)
        {
            return _unitOfWork.OrderRepository.GetOrderStatusByPartnerStatus(company, codeBase);
        }

        public BaseServerResponse<ItemResponse> GetItemsByItemCode(Company company, ItemResponse item)
        {
            return _unitOfWork.OrderRepository.GetItemsByItemCode(company, item);
        }

        public BaseServerResponse<PartnerOrder> GetPartnerOrdersByOrderKy(Company company, RequestParameters order)
        {
            return _unitOfWork.OrderRepository.GetPartnerOrdersByOrderKy(company, order);
        }

        public bool InsertLastOrderSync(RequestParameters request, Company company)
        {
            return _unitOfWork.OrderRepository.InsertLastOrderSync(request,company);
        }

        public bool InsertApiEndPoint(APIRequestParameters request, Company company)
        {
            return _unitOfWork.OrderRepository.InsertApiEndPoint(request, company);
        }
        public BaseServerResponse<APIInformation> GetAPIDetailsByMerchantID(APIRequestParameters request)
        {
            return _unitOfWork.OrderRepository.GetAPIDetailsByMerchantID(request);
        }
        public bool UberProvision_InsertUpdate(APIInformation request, Company company)
        {
            return _unitOfWork.OrderRepository.UberProvision_InsertUpdate(request,company);
        }
       
        public bool OrderHubStatus_UpdateWeb(RequestParameters request, User user)
        {
            return _unitOfWork.OrderRepository.OrderHubStatus_UpdateWeb(request, user);
        }

        public BaseServerResponse<IList<PartnerMenuItem>> GetAllOrderMenuItems(Company company, RequestParameters request)
        {
            return _unitOfWork.OrderRepository.GetAllOrderMenuItems(company,request);
        }
        public BaseServerResponse<IList<CodeBaseResponse>> GetNextOrderHubStatusByStatusKey(Company company, ComboRequestDTO request,int OrdStsKy)
        {
            return _unitOfWork.OrderRepository.GetNextOrderHubStatusByStatusKey(company, request,OrdStsKy);
        }
        public BaseServerResponse<PartnerOrder> GetPartnerOrdersByOrderID(Company company, RequestParameters order)
        {
            return _unitOfWork.OrderRepository.GetPartnerOrdersByOrderID(company, order);
        }
        public string PostOrderHubStockResevation(int OrdKy, int OrdTypKy, Company company, User user)
        {
            return _unitOfWork.OrderRepository.PostOrderHubStockResevation(OrdKy, OrdTypKy, company, user);
        }

        public string PostOrderHubStockResevationReversal(int OrdKy, Company company, User user)
        {
            return _unitOfWork.OrderRepository.PostOrderHubStockResevationReversal(OrdKy,company, user);
        }
        public BaseServerResponse<IList<CodeBaseResponse>> GetOrderHubBU(Company company)
        {
            return _unitOfWork.OrderRepository.GetOrderHubBU(company);
        }

        public BaseServerResponse<IList<PartnerOrder>> GetAvailablePickmeOrders(Company company,RequestParameters order)
        {
            return _unitOfWork.OrderRepository.GetAvailablePickmeOrders(company, order);
        }
        public bool APIResponseDet_InsertWeb(ResponseDetails request)
        {
            return _unitOfWork.OrderRepository.APIResponseDet_InsertWeb(request);
        }

        public int GetPickMeOrderByOrderID(Company company, RequestParameters partnerOrder)
        {
            return _unitOfWork.OrderRepository.GetPickMeOrderByOrderID(company, partnerOrder);
        }
    }
}
