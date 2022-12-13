using BlueLotus360.Core.Domain.DTOs.RequestDTO;
using BlueLotus360.Core.Domain.DTOs.ResponseDTO;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Core.Domain.Definitions.Repository
{
    public interface IAddressRepository
    {
        BaseServerResponse<IList<AddressResponse>> GetAddresses(Company company, User user, ComboRequestDTO dto);
        BaseServerResponse<IList<AddressResponse>> GetMAUIAddresses(Company company, User user, ComboRequestDTO dto);
        BaseServerResponse<AddressMaster> CustomerRegistration(Company company, User user, AddressMaster addressMaster);
        BaseServerResponse<AddressMaster> CustomerRegistrationValidation(Company company, User user, AddressMaster addressMaster);
        BaseServerResponse<AddressMaster> CheckAdvanceAnalysisAvailability(Company company, AddressMaster addressMaster);
        BaseServerResponse<AddressMaster> CreateAdvanceAnalysis(Company company, AddressMaster addressMaster);
        BaseServerResponse<AddressResponse> GetAddressByUserKey(Company company, User user);
        BaseServerResponse<AddressMaster> GetAddressByAdrKy(Company company, User user, AddressMaster adrmas);
    }
}
