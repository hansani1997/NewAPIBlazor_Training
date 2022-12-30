using BlueLotus360.Core.Domain.Entity.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Web.APIApplication.Definitions.ServiceDefinitions
{
    public interface IAPIService
    {
        APIInformation GetAPIInformationByAppId(string appId);
        void SaveRequestLog(APIRequestLogDetail detail);

      
    }
}
