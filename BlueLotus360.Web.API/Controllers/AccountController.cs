using BlueLotus360.Core.Domain.DTOs.RequestDTO;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.Payment;
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
    public class AccountController : ControllerBase
    {
        ILogger<AccountController> _logger;
        IAccountService  _accountService;
        public AccountController(ILogger<AccountController> logger,
                                IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        [HttpPost("readAccounts")]
        public IActionResult ReadAccounts(ComboRequestDTO comboRequest)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();

            var result = _accountService.GetComboAccounts(company, user, comboRequest);
            IList<AccountResponse> accounts = result.Value;

            return Ok(accounts);
        }

        [HttpPost("GetAccMappingByPaymentType")]
        public IActionResult GetAccMappingByPaymentType(AccPaymentMappingRequest request)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();

            IList<AccPaymentMappingResponse> accounts = null;
            accounts = _accountService.GetAccPaymentMappingLaund(company,user, request);




            return Ok(accounts);
        }
    }
}
