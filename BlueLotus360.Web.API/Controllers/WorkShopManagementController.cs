using BlueLotus360.Core.Domain.DTOs;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.BookingModule;
using BlueLotus360.Core.Domain.Entity.MastrerData;
using BlueLotus360.Core.Domain.Entity.Order;
using BlueLotus360.Core.Domain.Entity.Transaction;
using BlueLotus360.Core.Domain.Entity.WorkOrder;
using BlueLotus360.Core.Domain.Responses;
using BlueLotus360.Web.API.Authentication;
using BlueLotus360.Web.API.Extension;
using BlueLotus360.Web.APIApplication.Definitions.ServiceDefinitions;
using BlueLotus360.Web.APIApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlueLotus360.Web.API.Controllers
{
    [BLAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WorkShopManagementController : ControllerBase
    {
        ILogger<WorkShopManagementController> _logger;
        IWorkshopManagementService _workshopManagementService;
        IObjectService _objectService;
        ICodeBaseService _codeBaseService;
        public WorkShopManagementController(ILogger<WorkShopManagementController> logger,
                                            IWorkshopManagementService workshopManagementService,
                                            IObjectService objectService,ICodeBaseService codeBase)
        {
            _logger = logger;
            _workshopManagementService = workshopManagementService;
            _objectService = objectService;
            _codeBaseService = codeBase;
        }

        [HttpPost("searchVehicle")]
        public IActionResult SearchVehicle(VehicleSearch request)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();
            IList<Vehicle> vehicles = _workshopManagementService.GetVehicleDetails(request, company, user);
            return Ok(vehicles);
        }

        [HttpPost("getJobHistories")]
        public IActionResult GetJobHistories(Vehicle request)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();
            IList<WorkOrder> WorkOrders = _workshopManagementService.GetJobHistoryDetails(request, company, user);
            return Ok(WorkOrders);
        }

        [HttpPost("getCarProgessingProjectDetails")]
        public IActionResult GetCarProgessingProjectDetails(Vehicle request)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();
            IList<ProjectResponse> list = _workshopManagementService.GetProgressingProjectDetails(request, company, user);
            return Ok(list);
        }

        [HttpPost("createWorkOrder")]
        public IActionResult CreateWorkOrder(GenericOrder orderDetails)
        {
            //
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();
            var uiObject = _objectService.GetObjectByObjectKey(orderDetails.FormObjectKey);
            var ordTyp = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, uiObject.Value.OurCode, "OrdTyp");
            orderDetails.OrderType = ordTyp.Value;
            var ordsts = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, orderDetails.OrderStatus.OurCode, "OrdSts");
            orderDetails.OrderStatus = ordsts.Value;
            var ordControlCon = _codeBaseService.GetControlConditionCode(company, user, orderDetails.FormObjectKey, "OrdDetAcc");
            orderDetails.OrderControlCondition=ordControlCon.Value;

            var ord = _workshopManagementService.SaveWorkOrder(company, user, orderDetails);
            OrderSaveResponse orderServerResponse = ord.Value;
            return Ok(orderServerResponse);

        }

        [HttpPost("createIRNWorkOrder")]
        public IActionResult CreateIRNWorkOrder(GenericOrder insurenceOrder) 
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();
            var uiObject = _objectService.GetObjectByObjectKey(insurenceOrder.FormObjectKey);

            if (insurenceOrder.IsIRNEstimateOrder) 
            {
                var ordTyp = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, "IrnEst", "OrdTyp");
                insurenceOrder.OrderType = ordTyp.Value;
            }
            else
            {
                var ordTyp = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, uiObject.Value.OurCode, "OrdTyp");
                insurenceOrder.OrderType = ordTyp.Value;
            }

            var ordsts = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, insurenceOrder.OrderStatus.OurCode, "PrcsFlow");
            insurenceOrder.OrderStatus = ordsts.Value;

            var ord = _workshopManagementService.SaveIRNOrder(company, user, insurenceOrder);
            OrderSaveResponse response = ord.Value; 

            return Ok(response);
        }

        [HttpPost("updateWorkOrder")]
        public IActionResult UpdateWorkOrder(GenericOrder orderDetails)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();
            var uiObject = _objectService.GetObjectByObjectKey(orderDetails.FormObjectKey);

            var ordTyp = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, uiObject.Value.OurCode, "OrdTyp");
            orderDetails.OrderType = ordTyp.Value;

            var ordsts = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, orderDetails.OrderStatus.OurCode, "OrdSts");
            orderDetails.OrderStatus = ordsts.Value;

            var ordControlCon = _codeBaseService.GetControlConditionCode(company, user, orderDetails.FormObjectKey, "OrdDetAcc");
            orderDetails.OrderControlCondition = ordControlCon.Value;

            OrderSaveResponse orderServerResponse = _workshopManagementService.UpdateWorkOrder(company, user, orderDetails);

            return Ok(orderServerResponse);
        }

        [HttpPost("updateIRNWorkOrder")]
        public IActionResult UpdateIRNWorkOrder(GenericOrder insurenceOrder)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();
            var uiObject = _objectService.GetObjectByObjectKey(insurenceOrder.FormObjectKey);

            if (insurenceOrder.IsIRNEstimateOrder)
            {
                var ordTyp = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, "IrnEst", "OrdTyp");
                insurenceOrder.OrderType = ordTyp.Value;
            }
            else
            {
                var ordTyp = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, uiObject.Value.OurCode, "OrdTyp");
                insurenceOrder.OrderType = ordTyp.Value;
            }

            var ordsts = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, insurenceOrder.OrderStatus.OurCode, "PrcsFlow");
            insurenceOrder.OrderStatus = ordsts.Value;

            var ord = _workshopManagementService.UpdateIRNOrder(company, user, insurenceOrder);
            OrderSaveResponse response = ord.Value;

            return Ok(response);
        }

        [HttpPost("openWorkOrder")]
        public IActionResult OpenWorkOrder(OrderOpenRequest request)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();

            BaseServerResponse<WorkOrder> order = _workshopManagementService.OpenWorkOrder(company, user, request);
            return Ok(order.Value);
        }

        [HttpPost("getRecentBookingDetails")]
        public IActionResult GetRecentBookingDetails(Vehicle request)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();
            IList<BookingDetails> booked = _workshopManagementService.GetRecentBooking(request, company, user);
            return Ok(booked);
        }

        [HttpPost("saveTransactionOfWorkOrder")]
        public IActionResult SaveTransactionOfWorkOrder(BLTransaction transaction)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();
            var uiObject = _objectService.GetObjectByObjectKey(transaction.ElementKey);
            var trnTyp = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, "Sale", "TrnTyp");
            transaction.TransactionType = trnTyp.Value;
            //var trnControlCon = _codeBaseService.GetControlConditionCode(company, user, (int)transaction.ElementKey, "OrdDetAcc");
            //transaction.TransactionControlCondition = trnControlCon.Value;

            var trn = _workshopManagementService.SaveWorkOrderTransaction(transaction, company, user, uiObject.Value);
            
            return Ok(trn.Value);
        }

        [HttpPost("openTransactionOfWorkOrder")]
        public IActionResult OpenTransactionOfWorkOrder(TransactionOpenRequest request)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();
            var trn = _workshopManagementService.OpenWorkOrderTransaction(company, user, request);
            BLTransaction transaction = trn.Value;
            request.TrasctionTypeKey = transaction.TransactionType.CodeKey;
            var linItm = _workshopManagementService.GetWorkOrderTransactionLineItems(company, user, request);
            transaction.InvoiceLineItems = linItm.Value;

            return Ok(transaction);

        }

        [HttpPost("validationWorkOrderEndPoint")]
        public IActionResult WorkOrderValidation(WorkOrder req)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();

            UserRequestValidation response = _workshopManagementService.WorkorderValidation(req,company, user);

            return Ok(response);

        }

        [HttpPost("getIRNByStatus")]
        public IActionResult GetIRNByStatus(WorkOrder req)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();

            var uiObject = _objectService.GetObjectByObjectKey(req.FormObjectKey);
            var ordTyp = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, uiObject.Value.OurCode, "OrdTyp");
            req.OrderType = ordTyp.Value;
            var ordsts = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, req.OrderStatus.OurCode, "PrcsFlow");
            req.OrderStatus = ordsts.Value;
            IList<WorkOrder> response = _workshopManagementService.GetIRNBasedOnStatus(req, company, user);

            return Ok(response);

        }

        [HttpPost("orderToOrderPosting")]
        public IActionResult WorkOrderToOrderPosting(CarOrdToOrdPostingRequest request)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();
            var ordTyp = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, request.ToOrderType.OurCode, "OrdTyp");
            request.ToOrderType = ordTyp.Value;
            OrderSaveResponse res=_workshopManagementService.CarOrdToOrdPosting(request, company, user);
            return Ok(res);
        }
    }
}
