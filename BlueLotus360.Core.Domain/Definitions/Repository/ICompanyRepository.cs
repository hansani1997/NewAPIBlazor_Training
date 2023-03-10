using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Core.Domain.Definitions.Repository
{
    public interface ICompanyRepository
    {

        BaseServerResponse<IList<Company>>  GetUserAssociatedCompanies(User user);
        BaseServerResponse<Company>  GetCompanyByCode(string CompanyCode);


    }
}
