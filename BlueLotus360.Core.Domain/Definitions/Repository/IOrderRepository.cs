using BlueLotus360.Core.Domain.DTOs;
using BlueLotus360.Core.Domain.Entity.API;
using BlueLotus360.Core.Domain.Entity;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.Order;
using BlueLotus360.Core.Domain.Entity.WorkOrder;
using BlueLotus360.Core.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueLotus360.Core.Domain.Entity.UberEats;
using BlueLotus360.Core.Domain.DTOs.RequestDTO;
using BlueLotus360.Core.Domain.Entity.Transaction;

namespace BlueLotus360.Core.Domain.Definitions.Repository
{
    public interface IOrderRepository
    {
        void CreateOrder(OrderHeaderCreateDTO orderHeader, Company company, User user);
        void CreateOrderLineItem(OrderLineCreateDTO item, Company company, User user, UIObject uiobject);
        void PostInterLocationTransfers(Company company, User user, long OrderKey = 1, long ObjectKey = 1);
        OrderSimpleToWMS GetOrderByOrderKey(int OrderKey, User user);
        void UpdateGenericOrderHeader(OrderHeaderEditDTO orderV3, Company company, User user);
        void UpdateGenericOrderLineItem(OrderLineCreateDTO item, long ObjKy, Company company, User user);
        BaseServerResponse<IList<OrderFindResults>> GenericFindOrders(OrderFindDto dto, Company company, User user);
        BaseServerResponse<OrderHeaderEditDTO> GetGenericOrderByOrderKey(long Orderkey, Company company, User user);
        BaseServerResponse<OrderHeaderEditDTO> GetGenericOrderByOrderKeyV2(long Orderkey, Company company, User user);
        BaseServerResponse<IList<OrderLineCreateDTO>> GetGenericOrderLineItemsByOrderKey(long Orderkey, long ObjKy, Company company, User user);
        CodeBaseResponse OrderApproveStatusFindByOrdKy(Company company, User user, int objky, int ordky);
        BaseServerResponse<IList<GetFromQuotResults>> GenericRetrieveQuotation(GetFromQuoatationDTO request, Company company, User user);
        BaseServerResponse<IList<QuotationDetails>> GenericOpenQuotation(OrderOpenRequest request, Company company, User user);
        BaseServerResponse<WorkOrderAmountByAccount> OrderHeaderAccountInsertUpdate(Company company, User user, WorkOrderAmountByAccount accDet);
        BaseServerResponse<WorkOrderAmountByAccount> OrderDetailAccountInsertUpdate(Company company, User user, WorkOrderAmountByAccount accDet);
        BaseServerResponse<IList<WorkOrderAmountByAccount>> OrderHeaderAccountSelect(Company company, User user, WorkOrderAmountByAccount accDet);
        BaseServerResponse<IList<WorkOrderAmountByAccount>> OrderDetailAccountSelect(Company company, User user, WorkOrderAmountByAccount accDet);
        int PartnerOrders_Count(Company company, RequestParameters partnerOrder);
        BaseServerResponse<IList<PartnerOrder>> GetAllPartnerOrder(Company company, User user, RequestParameters order);
        BaseServerResponse<APIInformation> GetAPIDetails(Company company, User user, APIRequestParameters request);
        BaseServerResponse<IList<CodeBaseResponse>> GetOrderStatus(Company company);
        BaseServerResponse<APIInformation> GetAPIEndPoints(Company company, APIRequestParameters request);
        BaseServerResponse<PartnerOrder> GetLastOrderSyncTime(Company company, APIRequestParameters request);
        BaseServerResponse<PartnerOrder> GetOrdersFromOrderPlatforms(Company company, User user, PartnerOrder request);
        BaseServerResponse<CodeBaseResponse> GetOrderStatusByPartnerStatus(Company company, CodeBaseResponse codeBase);
        BaseServerResponse<ItemResponse> GetItemsByItemCode(Company company, ItemResponse item);
        BaseServerResponse<PartnerOrder> GetPartnerOrdersByOrderKy(Company company,RequestParameters order);
        bool InsertLastOrderSync(RequestParameters request, Company company);
        bool InsertApiEndPoint(APIRequestParameters request, Company company);
        BaseServerResponse<APIInformation> GetAPIDetailsByMerchantID(APIRequestParameters request);
        bool UberProvision_InsertUpdate(APIInformation request, Company company);
       // BaseServerResponse<IList<OrderMenuConfiguration>> GetAllOrderMenuConfiguration(Company company);
       // bool OrderMenuConfiguration_InsertUpdate(User user, OrderMenuConfiguration orderMenu);
        bool OrderHubStatus_UpdateWeb(RequestParameters request, User user);
        BaseServerResponse<IList<PartnerMenuItem>> GetAllOrderMenuItems(Company company, RequestParameters request);
        BaseServerResponse<IList<CodeBaseResponse>> GetNextOrderHubStatusByStatusKey(Company company, ComboRequestDTO request,int OrdStsKy);
        BaseServerResponse<PartnerOrder> GetPartnerOrdersByOrderID(Company company, RequestParameters order);
        string PostOrderHubStockResevation(int OrdKy, int OrdTypKy, Company company, User user);
        string PostOrderHubStockResevationReversal(int OrdKy, Company company, User user);
        BaseServerResponse<IList<CodeBaseResponse>> GetOrderHubBU(Company company);
        BaseServerResponse<IList<PartnerOrder>> GetAvailablePickmeOrders(Company company,  RequestParameters order);
        bool APIResponseDet_InsertWeb(ResponseDetails request);
        int GetPickMeOrderByOrderID(Company company, RequestParameters partnerOrder);
        bool UberMenu_DiscontinueWeb(UberDiscontinueItem request, Company company);

    }
}
