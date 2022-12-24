using BlueLotus360.Core.Domain.DTOs;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.BookingModule;
using BlueLotus360.Core.Domain.Entity.MastrerData;
using BlueLotus360.Core.Domain.Entity.Order;
using BlueLotus360.Core.Domain.Entity.WorkOrder;
using BlueLotus360.Core.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Core.Domain.Definitions.Repository
{
    public interface IWorkShopManagementRepository
    {
        BaseServerResponse<IList<Vehicle>> SelectVehicle(VehicleSearch dto, Company company, User user);
        BaseServerResponse<IList<WorkOrder>> SelectJobhistory(Vehicle dto, Company company, User user);
        BaseServerResponse<IList<ProjectResponse>> SelectOngoingProjectDetails(Vehicle dto, Company company, User user);
        BaseServerResponse<IList<BookingDetails>> RecentBookingDetails(Vehicle dto, Company company, User user);
        UserRequestValidation WorkOrderValidation(WorkOrder dto, Company company, User user);
        BaseServerResponse<IList<IRNResponse>> SelectIRNBasedOnStatus(WorkOrder dto, Company company, User user);
        OrderSaveResponse CarOrdToOrdPosting(CarOrdToOrdPostingRequest dto, Company company, User user);
    }
}
