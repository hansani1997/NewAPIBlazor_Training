using BlueLotus360.Core.Domain.Definitions.DataLayer;
using BlueLotus360.Core.Domain.DTOs.RequestDTO;
using BlueLotus360.Core.Domain.DTOs.ResponseDTO;
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
    public class AddressService:IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddressService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public BaseServerResponse<IList<AddressResponse>> GetComboAddresses(Company company, User user, ComboRequestDTO dto)
        {
           return _unitOfWork.AddressRepository.GetAddresses(company, user, dto);
        }

        public BaseServerResponse<IList<AddressResponse>> GetMAUIAddresses(Company company, User user, ComboRequestDTO dto)
        {
            return _unitOfWork.AddressRepository.GetMAUIAddresses(company, user, dto);
        }

        public BaseServerResponse<AddressMaster> CreateCustomer(Company company, User user, AddressMaster address)
        {
            return _unitOfWork.AddressRepository.CustomerRegistration(company, user, address);
        }

        public BaseServerResponse<AddressMaster> CustomerValidation(Company company, User user, AddressMaster address)
        { 
            return _unitOfWork.AddressRepository.CustomerRegistrationValidation(company, user, address);
        }

        public BaseServerResponse<AddressMaster> CheckAdvanceAnalysisAvailability(Company company, AddressMaster addressMaster)
        {
            return _unitOfWork.AddressRepository.CheckAdvanceAnalysisAvailability(company, addressMaster);
        }
        public BaseServerResponse<AddressMaster> CreateAdvanceAnalysis(Company company, AddressMaster addressMaster)
        {
            return _unitOfWork.AddressRepository.CreateAdvanceAnalysis(company, addressMaster);
        }

        public AddressResponse GetAddressDetailsByUserKy(Company company, User user)
        {
            var response = _unitOfWork.AddressRepository.GetAddressByUserKey(company, user);
            return response.Value;
        }

        public AddressMaster SelectAddressByAdrKy(Company company, User user, AddressMaster adrmas)
        {
            var adr = _unitOfWork.AddressRepository.GetAddressByAdrKy(company,user,adrmas);
            return adr.Value;
        }
    }
}
