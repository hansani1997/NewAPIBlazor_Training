using BlueLotus360.Core.Domain.Definitions.DataLayer;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.MastrerData;
using BlueLotus360.Core.Domain.Entity.UberEats;
using BlueLotus360.Web.APIApplication.Definitions.ServiceDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Web.APIApplication.Services
{
    public class ProjectService:IProjectService
    {
        public readonly IUnitOfWork _unitOfWork;
        public ProjectService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        public ProjectResponse InsertProject(Company company, User user, Project project)
        {
            if (project.ParentKey<=0)
            {
                project.ParentKey = 1;
            }
            return _unitOfWork.ProjectRepository.CreateProjectHeader(company, user, project); 
        }
        public ProjectResponse EditProject(Company company, User user, Project project)
        {
            project.OriginalProjectKey =(int) project.ProjectKey;
            return _unitOfWork.ProjectRepository.UpdateProjectHeader(company, user, project);
        }
        public Project SelectProject(Company company, User user, ProjectOpenRequest project)
        {
            return _unitOfWork.ProjectRepository.SelectProjectHeader(company, user, project);
        }
    }
}
