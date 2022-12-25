using BlueLotus360.Core.Domain.Definitions.DataLayer;
using BlueLotus360.Core.Domain.DTOs.RequestDTO;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.Payment;
using BlueLotus360.Core.Domain.Responses;
using BlueLotus360.Web.APIApplication.Definitions.ServiceDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Web.APIApplication.Services
{
    public class AccountService:IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }

        public BaseServerResponse<IList<AccountResponse>> GetComboAccounts(Company company, User user, ComboRequestDTO dto)
        {
            return _unitOfWork.AccountRepository.GetAccounts(company,user,dto);
        }

        public IList<AccPaymentMappingResponse> GetAccPaymentMappingLaund(Company company, User user, AccPaymentMappingRequest requestDTO)
        {
            return _unitOfWork.AccountRepository.GetAccPaymentMappingV1(company, user, requestDTO);
        }
    }
}
