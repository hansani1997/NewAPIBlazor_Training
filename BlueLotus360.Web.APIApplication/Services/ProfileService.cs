using BlueLotus.Com.Domain.Entity.Profile;
using BlueLotus360.Core.Domain.Definitions.DataLayer;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Responses;
using BlueLotus360.Web.APIApplication.Definitions.ServiceDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Web.APIApplication.Services
{
    public  class ProfileService :IProfileService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProfileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public BaseServerResponse<IList<AccountProfileResponse>> GetAccountProfileList(Company company, User user, AccountProfileRequest request)
        {
            return _unitOfWork.ProfileRepository.GetAccountProfileList(company, user, request);
        }

        public bool InsertAccountProfile(Company company, AccountProfileInsertRequest accountProfileInsertRequest)
        {
            return _unitOfWork.ProfileRepository.InsertAccountProfile(company, accountProfileInsertRequest);
        }

        public bool UpdateAccountProfile(Company company, User user, AccountProfileResponse request)
        {
            return _unitOfWork.ProfileRepository.UpdateAccountProfile(company, user,request);
        }
    }
}
