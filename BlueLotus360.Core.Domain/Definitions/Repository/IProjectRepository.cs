using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.MastrerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Core.Domain.Definitions.Repository
{
    public  interface IProjectRepository
    {
        ProjectResponse CreateProjectHeader(Company company,User user,Project project);
        ProjectResponse UpdateProjectHeader(Company company, User user, Project request);
        Project SelectProjectHeader(Company company, User user, ProjectOpenRequest request);
    }
}
