using BlueLotus360.Core.Domain.DTOs;
using BlueLotus360.Core.Domain.DTOs.RequestDTO;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.Transaction;
using BlueLotus360.Core.Domain.Responses;
using BlueLotus360.Web.API.Authentication;
using BlueLotus360.Web.API.Extension;
using BlueLotus360.Web.APIApplication.Definitions.ServiceDefinitions;
using BlueLotus360.Web.APIApplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Generic;
using System.Transactions;

namespace BlueLotus360.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [BLAuthorize]
    public class TransactionController : ControllerBase
    {

        ILogger<TransactionController> _logger;
        ITransactionService _transactionService;
        IObjectService _objectService;
        ICodeBaseService _codeBaseService;
        IItemService _itemService;
        public TransactionController(ILogger<TransactionController> logger, 
                                    ITransactionService transactionService,
                                    IObjectService objectService, ICodeBaseService codeBaseService, IItemService itemService)
        {
            _logger = logger;
            _transactionService = transactionService;
            _objectService = objectService;
            _codeBaseService = codeBaseService;
            _itemService = itemService;
        }


        [HttpPost("saveTransaction")]
        public IActionResult SaveTransaction(BLTransaction transaction)
        {
            var user =  Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();
            var uiObject = _objectService.GetObjectByObjectKey(transaction.ElementKey);
            var trnTyp = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, uiObject.Value.OurCode, "TrnTyp");
            var trn=_transactionService.SaveTransaction(transaction, company, user, uiObject.Value, trnTyp.Value);

            return Ok(trn.Value);
        }

        [HttpPost("findTransaction")]
        public IActionResult FindTransaction(TransactionFindRequest request)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();
            var uiObject = _objectService.GetObjectByObjectKey(request.ElementKey);
            var trnTyp = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, uiObject.Value.OurCode, "TrnTyp");
            request.TransactionType=trnTyp.Value;   
            var trn = _transactionService.FindTransaction(company, user, request);
            IList<GenericTransactionFindResponse> responses = trn.Value;
            return Ok(responses);
        }

        [HttpPost("openTransaction")]
        public IActionResult OpenTransaction(TransactionOpenRequest request)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();
            var trn = _transactionService.OpenTransaction(company, user, request);
            BLTransaction transaction=trn.Value;
            request.TrasctionTypeKey = transaction.TransactionType.CodeKey;
            var linItm= _transactionService.GetTransactionLineItems(company, user, request);
            transaction.InvoiceLineItems=linItm.Value;

            return Ok(transaction);

        }

        [HttpPost("getStockAsAtByLocation")]
        public IActionResult GetStockAsAtByLocation(StockAsAtRequest request)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();
            StockAsAtResponse response = _itemService.GetStockAsAtByLocation(company, user, request);
            return Ok(response);
        }

        [HttpPost("changeTransactionApproveState")]
        public IActionResult InsertTransactionApproveState(BLTransaction request)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();

           CodeBaseResponse latest_approvestate = _transactionService.ChangeTrnHdrAprSts(company, user,(int)request.TransactionKey, (int)request.ApproveState.CodeKey, (int)request.ElementKey, request.IsActive,request.ApproveState.OurCode);
           return Ok(latest_approvestate);
        }
        [HttpPost("checkPrintPermission")]
        public IActionResult CheckPrintPermission(BLTransaction request)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();

            var trnTyp = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, "Sale", "TrnTyp");
            request.TransactionType = trnTyp.Value;

           TransactionPermission print_permission= _transactionService.CheckSourceDocPrintPermission((int)request.TransactionKey, (int)request.ApproveState.CodeKey, (int)request.ElementKey, (int)request.TransactionType.CodeKey, company, user);
            return Ok(print_permission);
        }

        [HttpPost("getTrnNextApproveStatus")]
        public IActionResult GetTrnNextApproveStatus(BLTransaction request)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();

            var trnTyp = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, "Sale", "TrnTyp");
            request.TransactionType = trnTyp.Value;

            CodeBaseResponse nextApr = _transactionService.TrnHdrNextApproveStatus((int)request.ApproveState.CodeKey, (int)request.ElementKey, (int)request.TransactionType.CodeKey, company, user);
            return Ok(nextApr);
        }


        [HttpPost("getOrderTrnPermission")]
        public IActionResult GetOrderTrnPermission(BLTransaction request)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();

            var trnTyp = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, "Sale", "TrnTyp");
            request.TransactionType = trnTyp.Value;

            TransactionPermission per = _transactionService.GetPermissionForOrderTrn((int)request.ApproveState.CodeKey, (int)request.ElementKey, (int)request.TransactionType.CodeKey,(int)request.TransactionKey, company, user);
            return Ok(per);
        }

        [HttpPost("GetTransactionTotalPayed")]
        public IActionResult GetTransactionTotalPayed(RecieptDetailRequest request)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();

            RecviedAmountResponse response = _transactionService.GetRecviedAmountResponse(company, user, request);
            return Ok(response);
        }

        [HttpPost("SaveAccountRecieptEx")]
        public IActionResult SaveAccountRecieptEx(PayementModeReciept request)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();

            _transactionService.SaveAccountResponseEx(company, user, request);
            return Ok();
        }
    }
}
