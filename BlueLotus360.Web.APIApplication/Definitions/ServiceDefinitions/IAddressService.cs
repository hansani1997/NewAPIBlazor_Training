using BlueLotus360.Core.Domain.DTOs.RequestDTO;
using BlueLotus360.Core.Domain.DTOs.ResponseDTO;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Web.APIApplication.Definitions.ServiceDefinitions
{
    public interface IAddressService
    {
        BaseServerResponse<IList<AddressResponse>>  GetComboAddresses(Company company, User user, ComboRequestDTO dto);
        BaseServerResponse<AddressMaster> CreateCustomer(Company company, User user, AddressMaster address);
        BaseServerResponse<AddressMaster> CustomerValidation(Company company, User user, AddressMaster address);
        BaseServerResponse<AddressMaster> CheckAdvanceAnalysisAvailability(Company company, AddressMaster addressMaster);
        BaseServerResponse<AddressMaster> CreateAdvanceAnalysis(Company company, AddressMaster addressMaster);
        AddressResponse GetAddressDetailsByUserKy(Company company, User user);
        BaseServerResponse<IList<AddressResponse>> GetMAUIAddresses(Company company, User user, ComboRequestDTO dto);
        AddressMaster SelectAddressByAdrKy(Company company, User user, AddressMaster adrmas);
    }
}
