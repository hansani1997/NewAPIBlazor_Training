using BlueLotus.Com.Domain.Entity.Profile;
using BlueLotus360.Core.Domain.Definitions.Repository;
using BlueLotus360.Core.Domain.Entity.Auth;
using BlueLotus360.Core.Domain.Responses;
using BlueLotus360.Web.API.Authentication;
using BlueLotus360.Web.API.Extension;
using BlueLotus360.Web.APIApplication.Definitions.ServiceDefinitions;
using Microsoft.AspNetCore.Mvc;


namespace BlueLotus360.Web.API.Controllers
{

    [BLAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        ILogger<ProfileController> _logger;
        IProfileService _profileService;
        IObjectService _objectService;
        ICodeBaseService _codeBaseService;

        public ProfileController(ILogger<ProfileController> logger, IProfileService profileService, IObjectService objectService, ICodeBaseService codeBaseService)
        {
           _logger = logger;
            _profileService = profileService;
            _objectService = objectService;
            _codeBaseService = codeBaseService;
        }

        //build request to call api for get account profile list
        [HttpPost("getProfileList")]

        public IActionResult GetProfileList(AccountProfileRequest request)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();
            BaseServerResponse<IList<AccountProfileResponse>> list = _profileService.GetAccountProfileList(company, user, request);
            return Ok(list.Value);
        }

        [HttpPost("insertAccountRecord")]
        public IActionResult InsertAccountRecord(AccountProfileInsertRequest request)
        {
            var company = Request.GetAssignedCompany();
            var response =_profileService.InsertAccountProfile(company, request);
            return Ok(response);
        }

        [HttpPost("updateAccountRecord")]

        public IActionResult UpdateAccountRecord(AccountProfileResponse request)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();
            var response = _profileService.UpdateAccountProfile(company,user,request);
            return Ok(response);
        }
    }
}
