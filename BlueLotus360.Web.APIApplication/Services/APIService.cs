using BlueLotus360.Core.Domain.Definitions.DataLayer;
using BlueLotus360.Core.Domain.Entity.API;
using BlueLotus360.Web.APIApplication.Definitions.ServiceDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Web.APIApplication.Services
{
    public class APIService : IAPIService
    {
        IUnitOfWork _unitofWork { get; set; }

        public APIService(IUnitOfWork unitOfWork)
        {
            _unitofWork=unitOfWork;
        }
        public APIInformation GetAPIInformationByAppId(string appId)
        {
            var  apiInformation = _unitofWork.APIRepository.GetAPIInformationByAppId(appId);
            return apiInformation.Value;
        }

        public void SaveRequestLog(APIRequestLogDetail logDetail)
        {
            _unitofWork.APIRepository.SaveRequestLog(logDetail);
        }
    }
}
