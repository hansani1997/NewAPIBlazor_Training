using BlueLotus360.Core.Domain.Entity.API;
using BlueLotus360.Core.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Core.Domain.Definitions.Repository
{
    public interface IAPIRepository
    {
        BaseServerResponse<APIInformation> GetAPIInformationByAppId(string appId);

        BaseServerResponse<object> SaveRequestLog(APIRequestLogDetail detail);

    }
}
