using BlueLotus360.Core.Domain.DTOs.RequestDTO;
using BlueLotus360.Core.Domain.DTOs.ResponseDTO;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Web.API.Authentication;
using BlueLotus360.Web.API.Extension;
using BlueLotus360.Web.APIApplication.Definitions.ServiceDefinitions;
using BlueLotus360.Web.APIApplication.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BlueLotus360.Web.API.Controllers
{
    [BLAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        ILogger<AddressController> _logger;
        IAddressService _addressService;
        ICodeBaseService _codeBaseService;
        public AddressController(ILogger<AddressController> logger,
                                IAddressService addressService, ICodeBaseService codeBaseService)
        {
            _logger = logger;
            _addressService = addressService;
            _codeBaseService = codeBaseService;
        }

        [HttpPost("readAddress")]
        public IActionResult ReadAddress(ComboRequestDTO comboRequest)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();

            var result = _addressService.GetComboAddresses(company, user, comboRequest);
            IList<AddressResponse> addresses = result.Value;

            return Ok(addresses);
        }

        [HttpPost("readMAUIAddress")]
        public IActionResult ReadMAUIAddress(ComboRequestDTO comboRequest)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();

            var result = _addressService.GetMAUIAddresses(company, user, comboRequest);
            IList<AddressResponse> addresses = result.Value;

            return Ok(addresses);
        }


        [HttpPost("createCustomer")]
        public IActionResult CreateCustomer(AddressMaster address)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();

            var result = _addressService.CreateCustomer(company, user, address);
            AddressMaster addressMaster = result.Value;

            return Ok(addressMaster);
        }

        [HttpPost("customerValidation")]
        public IActionResult CustomerValidation(AddressMaster address) 
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();

            var result = _addressService.CustomerValidation(company, user, address);
            AddressMaster addressMaster = result.Value;

            return Ok(addressMaster);
        }

        [HttpPost("CheckAdvanceAnalysisAvailability")]
        public IActionResult CheckAdvanceAnalysisAvailability(AddressMaster address)
        {
            var company = Request.GetAssignedCompany();

            var result = _addressService.CheckAdvanceAnalysisAvailability(company,address);
            AddressMaster addressMaster = result.Value;

            return Ok(addressMaster);
        }

        [HttpPost("CreateAdvanceAnalysis")]
        public IActionResult CreateAdvanceAnalysis(AddressMaster address)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();
            var AddressType= _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, "CUS", "AdrTyp");
            address.AddressType = AddressType.Value;
            address.IsActive = 1;
            var result = _addressService.CreateAdvanceAnalysis(company, address);
            AddressMaster addressMaster = result.Value;

            return Ok(addressMaster);
        }

        [HttpGet("GetAddressByUsrKy")]
        public IActionResult GetAdrressByUsrKy()
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();

            AddressResponse addressMaster = _addressService.GetAddressDetailsByUserKy(company, user);

            return Ok(addressMaster);
        }

        [HttpPost("getAddressByAdrKy")]
        public IActionResult GetAddressByAdrKy(AddressMaster req)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();

            AddressMaster addressMaster = _addressService.SelectAddressByAdrKy(company, user, req);

            return Ok(addressMaster);
        }
    }
}
