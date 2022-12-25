using BlueLotus360.Core.Domain.DTOs.RequestDTO;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.Payment;
using BlueLotus360.Core.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Web.APIApplication.Definitions.ServiceDefinitions
{
    public interface IAccountService
    {
        BaseServerResponse<IList<AccountResponse>> GetComboAccounts(Company company, User user, ComboRequestDTO dto);
        IList<AccPaymentMappingResponse> GetAccPaymentMappingLaund(Company company, User user, AccPaymentMappingRequest requestDTO);
    }
}
