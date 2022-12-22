using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.MastrerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Web.APIApplication.Definitions.ServiceDefinitions
{
    public interface IProjectService
    {
        ProjectResponse InsertProject(Company company,User user,Project project);
        ProjectResponse EditProject(Company company, User user, Project project);
        Project SelectProject(Company company, User user, ProjectOpenRequest project);
    }
}
