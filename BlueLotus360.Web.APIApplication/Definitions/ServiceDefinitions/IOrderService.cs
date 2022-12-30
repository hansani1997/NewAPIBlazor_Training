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

namespace BlueLotus360.Web.APIApplication.Definitions.ServiceDefinitions
{
    public interface IOrderService
    {
        BaseServerResponse<OrderSaveResponse> SaveOrder(Company company, User user, GenericOrder orderDetails, CodeBaseResponse ordTyp);

        OrderSaveResponse UpdateOrder(Company company, User user, GenericOrder orderDetails, CodeBaseResponse ordTyp);
        BaseServerResponse<IList<OrderFindResults>> FindOrders(OrderFindDto request, Company company, User user,CodeBaseResponse ordTyp);

        BaseServerResponse<GenericOrder> OpenOrder(Company company, User user, OrderOpenRequest request);
        BaseServerResponse<IList<GetFromQuotResults>> RetrieveQuotation(GetFromQuoatationDTO request, Company company, User user, CodeBaseResponse ordTyp, CodeBaseResponse preOrdTyp);

        BaseServerResponse<GenericOrder> OpenQuotation(Company company, User user, OrderOpenRequest request, CodeBaseResponse ordTyp);
        int PartnerOrders_Count(Company company, RequestParameters partnerOrder);
        BaseServerResponse<IList<PartnerOrder>> GetAllPartnerOrder(Company company, User user, RequestParameters order);
        BaseServerResponse<APIInformation> GetAPIDetails(Company company, User user, APIRequestParameters request);
        BaseServerResponse<IList<CodeBaseResponse>> GetOrderStatus(Company company);
        BaseServerResponse<APIInformation> GetAPIEndPoints(Company company, APIRequestParameters request);
        BaseServerResponse<PartnerOrder> GetLastOrderSyncTime(Company company, APIRequestParameters request);
        BaseServerResponse<PartnerOrder> GetOrdersFromOrderPlatforms(Company company, User user, PartnerOrder request);
        BaseServerResponse<CodeBaseResponse> GetOrderStatusByPartnerStatus(Company company, CodeBaseResponse codeBase);
        BaseServerResponse<ItemResponse> GetItemsByItemCode(Company company, ItemResponse item);
        BaseServerResponse<PartnerOrder> GetPartnerOrdersByOrderKy(Company company, RequestParameters order);
        bool InsertLastOrderSync(RequestParameters request, Company company);
        bool InsertApiEndPoint(APIRequestParameters request, Company company);
        BaseServerResponse<APIInformation> GetAPIDetailsByMerchantID(APIRequestParameters request);
        bool UberProvision_InsertUpdate(APIInformation request, Company company);
       
        bool OrderHubStatus_UpdateWeb(RequestParameters request, User user);
        BaseServerResponse<IList<PartnerMenuItem>> GetAllOrderMenuItems(Company company, RequestParameters request);
        BaseServerResponse<IList<CodeBaseResponse>> GetNextOrderHubStatusByStatusKey(Company company, ComboRequestDTO request,int OrdStsKy);
        BaseServerResponse<PartnerOrder> GetPartnerOrdersByOrderID(Company company, RequestParameters order);
        string PostOrderHubStockResevation(int OrdKy, int OrdTypKy, Company company, User user);
        string PostOrderHubStockResevationReversal(int OrdKy, Company company, User user);
        BaseServerResponse<IList<CodeBaseResponse>> GetOrderHubBU(Company company);
        BaseServerResponse<IList<PartnerOrder>> GetAvailablePickmeOrders(Company company, RequestParameters order);
        bool APIResponseDet_InsertWeb(ResponseDetails request);
        int GetPickMeOrderByOrderID(Company company, RequestParameters partnerOrder);
        bool UberMenu_DiscontinueWeb(UberDiscontinueItem request, Company company);
    }
}
