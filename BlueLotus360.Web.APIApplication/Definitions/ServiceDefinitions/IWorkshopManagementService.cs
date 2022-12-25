using BlueLotus360.Core.Domain.DTOs;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.BookingModule;
using BlueLotus360.Core.Domain.Entity.MastrerData;
using BlueLotus360.Core.Domain.Entity.Order;
using BlueLotus360.Core.Domain.Entity.Transaction;
using BlueLotus360.Core.Domain.Entity.WorkOrder;
using BlueLotus360.Core.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Web.APIApplication.Definitions.ServiceDefinitions
{
    public interface IWorkshopManagementService
    {
        public IList<Vehicle> GetVehicleDetails(VehicleSearch request, Company company, User user);
        IList<WorkOrder> GetJobHistoryDetails(Vehicle request, Company company, User user);
        IList<ProjectResponse> GetProgressingProjectDetails(Vehicle request, Company company, User user);
        BaseServerResponse<OrderSaveResponse> SaveWorkOrder(Company company, User user, GenericOrder orderDetails);
        OrderSaveResponse UpdateWorkOrder(Company company, User user, GenericOrder orderDetails);
        BaseServerResponse<WorkOrder> OpenWorkOrder(Company company, User user, OrderOpenRequest request);
        IList<BookingDetails> GetRecentBooking(Vehicle request, Company company, User user);
        BaseServerResponse<BLTransaction> SaveWorkOrderTransaction(BLTransaction transaction, Company company, User user, UIObject uIObject);
        BaseServerResponse<BLTransaction> OpenWorkOrderTransaction(Company company, User user, TransactionOpenRequest request);
        BaseServerResponse<IList<GenericTransactionLineItem>> GetWorkOrderTransactionLineItems(Company company, User user, TransactionOpenRequest request);

        //carmart insurence
        BaseServerResponse<OrderSaveResponse> SaveIRNOrder(Company company, User user, GenericOrder order);
        BaseServerResponse<OrderSaveResponse> UpdateIRNOrder(Company company, User user, GenericOrder order);
        UserRequestValidation WorkorderValidation(WorkOrder dto, Company company, User user);
        IList<WorkOrder> GetIRNBasedOnStatus(WorkOrder dto, Company company, User user);
        OrderSaveResponse CarOrdToOrdPosting(CarOrdToOrdPostingRequest dto, Company company, User user);
    }
}
