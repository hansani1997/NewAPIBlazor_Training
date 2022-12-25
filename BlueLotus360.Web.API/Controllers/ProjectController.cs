using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.MastrerData;
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
    public class ProjectController : ControllerBase
    {
        ILogger<ProjectController> _logger;
        IProjectService _projectService;
        IObjectService _objectService;
        ICodeBaseService _codeBaseService;
        public ProjectController(ILogger<ProjectController> logger,IProjectService projectService,
                                IObjectService objectService, ICodeBaseService codeBaseService) 
        {
            _logger = logger;
            _projectService = projectService;
            _objectService = objectService;
            _codeBaseService = codeBaseService;
        }

        [HttpPost("projectHeaderInsert")]
        public IActionResult ProjectHeaderInsert(Project request)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();
            var uiObject = _objectService.GetObjectByObjectKey(request.ObjectKey);
            var prjTyp = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, uiObject.Value.OurCode2, "PrjTyp");
            request.ProjectType = prjTyp.Value ;
            ProjectResponse response= _projectService.InsertProject(company,user,request);
            return Ok(response);
        }

        [HttpPost("projectHeaderUpdate")]
        public IActionResult ProjectHeaderUpdate(Project request)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();
            var uiObject = _objectService.GetObjectByObjectKey(request.ObjectKey);
            var prjTyp = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, uiObject.Value.OurCode2, "PrjTyp");
            request.ProjectType = prjTyp.Value;
            ProjectResponse response = _projectService.EditProject(company, user, request);
            return Ok(response);
        }

        [HttpPost("projectHeaderSelect")]
        public IActionResult ProjectHeaderSelect(ProjectOpenRequest request)
        {
            var user = Request.GetAuthenticatedUser();
            var company = Request.GetAssignedCompany();
            var uiObject = _objectService.GetObjectByObjectKey(request.ObjectKey);
            var prjTyp = _codeBaseService.GetCodeByOurCodeAndConditionCode(company, user, uiObject.Value.OurCode2, "PrjTyp");
            request.ProjectType = prjTyp.Value;
            Project response = _projectService.SelectProject(company, user, request);
            return Ok(response);
        }
    }
}
