using BlueLotus360.Core.Domain.DTOs.ResponseDTO;
using BlueLotus360.Core.Domain.Entity.Auth;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Models;
using BlueLotus360.Web.API.Authentication;
using BlueLotus360.Web.API.Extension;
using BlueLotus360.Web.APIApplication.Definitions.ServiceDefinitions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace BlueLotus360.Web.API.Controllers
{
    [BLAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        ILogger<AuthenticationController> _logger;
        IUserService _userService;

      // ext
        public AuthenticationController(ILogger<AuthenticationController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [BLAllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(UserAuthenticationRequest model)
        {
            var response = _userService.AuthenticateUser(model, Request.GetRequestIP());
            return Ok(response);
        }

        [BLAuthorize(false)]
        [HttpPost("getUserCompanies")]
        public IActionResult GetUserCompanies()
        { 
            var companies = _userService.GetUserCompanies(Request.GetAuthenticatedUser());
            return Ok(companies);
        }

        [BLAuthorize(false)]
        [HttpPost("updateSelectedCompany")]
        public IActionResult UpdateSelectedCompany(CompanyResponse updateRequest)
        {
            //CompanyResponse updateRequest
            var companies = _userService.GetUserCompanies(Request.GetAuthenticatedUser());
            var selectedCompany = companies.Where(x => x.CompanyKey == updateRequest.CompanyKey).FirstOrDefault();
            if (Request.GetAuthenticatedUser() == null || selectedCompany == null)
            {
                return BadRequest();
            }
            var resp = _userService.UpdateCompanySelection(Request.GetAuthenticatedUser(), selectedCompany, Request.GetRequestIP());
            Response.SetRefeshTokenCookie(resp.RefreshToken);
            return Ok(resp);
        }

        [BLAuthorize]
        [HttpGet("getCompanyInformation")]
        public IActionResult GetCompanyInformation()
        {
            var company = new
            {
                User = Request.GetAuthenticatedUser(),
                Company = Request.GetAssignedCompany()
            };

            CompletedUserAuth auth = new CompletedUserAuth()
            {
                AuthenticatedCompany= company.Company,
                AuthenticatedUser = company.User,
            };
            return Ok(auth);
        }
    }
}
