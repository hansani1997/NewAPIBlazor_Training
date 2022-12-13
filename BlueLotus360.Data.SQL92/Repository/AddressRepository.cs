using BlueLotus360.Core.Domain.Definitions.Repository;
using BlueLotus360.Core.Domain.DTOs.RequestDTO;
using BlueLotus360.Core.Domain.DTOs.ResponseDTO;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Responses;
using BlueLotus360.Data.SQL92.DataLayer;
using BlueLotus360.Data.SQL92.Definition;
using BlueLotus360.Data.SQL92.Extenstions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Data.SQL92.Repository
{
    internal class AddressRepository :BaseRepository,IAddressRepository
    {
        public AddressRepository(ISQLDataLayer datalayer):base(datalayer)
        {

        }

        public BaseServerResponse<IList<AddressResponse>> GetAddresses(Company company, User user, ComboRequestDTO dto)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                string SPName = "AdrIDNm_SelectMob";
                BaseServerResponse<IList<AddressResponse>> response = new BaseServerResponse<IList<AddressResponse>>();
                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    IList<AddressResponse> addresses = new List<AddressResponse>();
                    dbCommand.CreateAndAddParameter("@Cky", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("@UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("@ObjKy", dto.RequestingElementKey);
                    dbCommand.CreateAndAddParameter("@SearchVal", dto.SearchQuery);

                    response.ExecutionStarted = DateTime.UtcNow;
                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        AddressResponse address = new AddressResponse()
                        {
                            AddressKey = reader.GetColumn<int>("AdrKy"),
                            AddressName = reader.GetColumn<string>("AdrIdNm"),
                            IsDefault = reader.GetColumn<int>("AdrKy") == reader.GetColumn<int>("DefaultKey")
                        };
                        addresses.Add(address);
                        
                    }
                    
                    response.ExecutionEnded= DateTime.UtcNow;
                    response.Value = addresses;

                }
                catch (Exception exp)
                {
                    throw exp;
                }

                finally
                {
                    IDbConnection dbConnection = dbCommand.Connection;
                    if (reader != null && !reader.IsClosed)
                    {
                        reader.Close();
                    }

                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }

                    dbCommand.Dispose();
                    dbConnection.Dispose();
                }
                return response;
            }
        }


        public BaseServerResponse<IList<AddressResponse>> GetMAUIAddresses(Company company, User user, ComboRequestDTO dto)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                string SPName = "AdrIDNm_SelectMobMAUI";
                BaseServerResponse<IList<AddressResponse>> response = new BaseServerResponse<IList<AddressResponse>>();
                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    IList<AddressResponse> addresses = new List<AddressResponse>();
                    dbCommand.CreateAndAddParameter("@Cky", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("@UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("@ObjKy", dto.RequestingElementKey);
                    dbCommand.CreateAndAddParameter("@SearchVal", dto.SearchQuery);

                    response.ExecutionStarted = DateTime.UtcNow;
                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        AddressResponse address = new AddressResponse()
                        {
                            AddressKey = reader.GetColumn<int>("AdrKy"),
                            AddressName = reader.GetColumn<string>("AdrNm"),
                            AddressId = reader.GetColumn<string>("AdrId"),
                            IsDefault = reader.GetColumn<int>("AdrKy") == reader.GetColumn<int>("DefaultKey")
                        };
                        addresses.Add(address);

                    }

                    response.ExecutionEnded = DateTime.UtcNow;
                    response.Value = addresses;

                }
                catch (Exception exp)
                {
                    throw exp;
                }

                finally
                {
                    IDbConnection dbConnection = dbCommand.Connection;
                    if (reader != null && !reader.IsClosed)
                    {
                        reader.Close();
                    }

                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }

                    dbCommand.Dispose();
                    dbConnection.Dispose();
                }
                return response;
            }
        }
        public BaseServerResponse<AddressMaster> CustomerRegistration(Company company, User user, AddressMaster addressMaster)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                string SPName = "CARCusVeh_InsertWeb";
                BaseServerResponse<AddressMaster> response = new BaseServerResponse<AddressMaster>();

                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;

                    dbCommand.CreateAndAddParameter("@Cky", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("@UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("@ObjKy", addressMaster.ElementKey);
                    dbCommand.CreateAndAddParameter("@RegDt", addressMaster.RegistrationDate);
                    dbCommand.CreateAndAddParameter("@RegNo", addressMaster.RegistraionNumber);
                    dbCommand.CreateAndAddParameter("@ChassiNo", addressMaster.ChassiNumber);
                    dbCommand.CreateAndAddParameter("@MakeKy", addressMaster.Make.CodeKey);
                    dbCommand.CreateAndAddParameter("@ModelKy", addressMaster.Model.CodeKey);
                    dbCommand.CreateAndAddParameter("@MakeYr", addressMaster.MakeYear);
                    dbCommand.CreateAndAddParameter("@AdrCat1Ky", addressMaster.Category.CodeKey);
                    dbCommand.CreateAndAddParameter("@AdrCat2Ky", addressMaster.SubCategory.CodeKey);
                    dbCommand.CreateAndAddParameter("@MaintPckg", addressMaster.MaintainPackage.CodeKey);
                    dbCommand.CreateAndAddParameter("@CusAdrNm", addressMaster.Address);
                    dbCommand.CreateAndAddParameter("@NIC", addressMaster.NIC);
                    dbCommand.CreateAndAddParameter("@Email", addressMaster.Email);
                    dbCommand.CreateAndAddParameter("@AdrCat4Ky", addressMaster.Province.CodeKey);
                    dbCommand.CreateAndAddParameter("@Address", addressMaster.PostalAddress??"");

                    response.ExecutionStarted = DateTime.UtcNow;
                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();

                    while (reader.Read())
                    {

                    }

                    response.ExecutionEnded = DateTime.UtcNow;

                }
                catch (Exception exp)
                {
                    throw exp;
                }

                finally
                {
                    IDbConnection dbConnection = dbCommand.Connection;
                    if (reader != null && !reader.IsClosed)
                    {
                        reader.Close();
                    }

                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }

                    dbCommand.Dispose();
                    dbConnection.Dispose();
                }

                return response;
            }
        }

        public BaseServerResponse<AddressMaster> CustomerRegistrationValidation(Company company, User user, AddressMaster addressMaster)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                string SPName = "CARCusVeh_ValidateWeb";
                BaseServerResponse<AddressMaster> response = new BaseServerResponse<AddressMaster>();
                AddressMaster addressvalid = new AddressMaster();
                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;

                    dbCommand.CreateAndAddParameter("@Cky", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("@UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("@ObjKy", addressMaster.ElementKey);
                    dbCommand.CreateAndAddParameter("@RegNo", addressMaster.RegistraionNumber);
                    dbCommand.CreateAndAddParameter("@ChassiNo", addressMaster.ChassiNumber);
                    dbCommand.CreateAndAddParameter("@NIC", addressMaster.NIC);

                    response.ExecutionStarted = DateTime.UtcNow;
                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        addressvalid.Message = reader.GetColumn<string>("Msg");
                        addressvalid.HasError = reader.GetColumn<bool>("hasError");
                    }

                    response.Value = addressvalid;
                    response.ExecutionEnded = DateTime.UtcNow;

                }
                catch (Exception exp)
                {
                    throw exp;
                }

                finally
                {
                    IDbConnection dbConnection = dbCommand.Connection;
                    if (reader != null && !reader.IsClosed)
                    {
                        reader.Close();
                    }

                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }

                    dbCommand.Dispose();
                    dbConnection.Dispose();
                }

                return response;
            }
        }

        public BaseServerResponse<AddressMaster> CheckAdvanceAnalysisAvailability(Company company,AddressMaster addressMaster)
        {
            AddressMaster adrr = new AddressMaster();
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                string SPName = "GetEmpKy";
                BaseServerResponse<AddressMaster> response = new BaseServerResponse<AddressMaster>();

                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;

                    dbCommand.CreateAndAddParameter("@CKy", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("@EmpNo", addressMaster.AddressId);

                    response.ExecutionStarted = DateTime.UtcNow;
                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        adrr.AddressKey = reader.GetColumn<int>("EmpKy");
                        adrr.AddressId = reader.GetColumn<string>("AdrID");
                        adrr.AddressName = reader.GetColumn<string>("AdrNm");
                        adrr.Address = reader.GetColumn<string>("Address");
                        adrr.Mobile = reader.GetColumn<string>("Telephone");
                    }

                    response.ExecutionEnded = DateTime.UtcNow;
                    response.Value = adrr;
                }
                catch (Exception exp)
                {
                    throw exp;
                }

                finally
                {
                    IDbConnection dbConnection = dbCommand.Connection;
                    if (reader != null && !reader.IsClosed)
                    {
                        reader.Close();
                    }

                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }

                    dbCommand.Dispose();
                    dbConnection.Dispose();
                }

                return response;
            }
        }

        public BaseServerResponse<AddressMaster> CreateAdvanceAnalysis(Company company, AddressMaster addressMaster)
        {
            AddressMaster adrr = new AddressMaster();
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                string SPName = "AdrMas_InsertWeb";
                BaseServerResponse<AddressMaster> response = new BaseServerResponse<AddressMaster>();

                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;

                    dbCommand.CreateAndAddParameter("@AdrID", addressMaster.AddressId);
                    dbCommand.CreateAndAddParameter("@AdrNm", addressMaster.AddressName);
                    dbCommand.CreateAndAddParameter("@isAct", addressMaster.IsActive);
                    dbCommand.CreateAndAddParameter("@isApr", 1);
                    dbCommand.CreateAndAddParameter("@CKy", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("@DefAdrTypKy", addressMaster.AddressType.CodeKey);
                    dbCommand.CreateAndAddParameter("@Address", addressMaster.Address);
                    dbCommand.CreateAndAddParameter("@Telephone", addressMaster.Mobile);

                    response.ExecutionStarted = DateTime.UtcNow;
                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        adrr.AddressKey = reader.GetColumn<int>("AdrKy");
                    }

                    response.ExecutionEnded = DateTime.UtcNow;
                    response.Value = adrr;
                }
                catch (Exception exp)
                {
                    throw exp;
                }

                finally
                {
                    IDbConnection dbConnection = dbCommand.Connection;
                    if (reader != null && !reader.IsClosed)
                    {
                        reader.Close();
                    }

                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }

                    dbCommand.Dispose();
                    dbConnection.Dispose();
                }

                return response;
            }
        }

        public BaseServerResponse<AddressResponse> GetAddressByUserKey(Company company, User user)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                string SPName = "GetEmployeeByUsrKy";
                BaseServerResponse<AddressResponse> response = new BaseServerResponse<AddressResponse>();
                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    AddressResponse address = new AddressResponse();
                    dbCommand.CreateAndAddParameter("@Cky", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("@UsrKy", user.UserKey);

                    response.ExecutionStarted = DateTime.UtcNow;
                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        address = new AddressResponse()
                        {
                            AddressKey = reader.GetColumn<int>("AdrKy"),
                            AddressName = reader.GetColumn<string>("AdrNm"),
                        };                      

                    }

                    response.ExecutionEnded = DateTime.UtcNow;
                    response.Value = address;

                }
                catch (Exception exp)
                {
                    throw exp;
                }

                finally
                {
                    IDbConnection dbConnection = dbCommand.Connection;
                    if (reader != null && !reader.IsClosed)
                    {
                        reader.Close();
                    }

                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }

                    dbCommand.Dispose();
                    dbConnection.Dispose();
                }
                return response;
            }
        }

        public BaseServerResponse<AddressMaster> GetAddressByAdrKy(Company company, User user,AddressMaster adrmas)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                string SPName = "CarAdrDetailsByAdrKy_SelectWeb";
                BaseServerResponse<AddressMaster> response = new BaseServerResponse<AddressMaster>();
                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    AddressMaster address = new AddressMaster();
                    dbCommand.CreateAndAddParameter("@Cky", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("@UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("@ObjKy", adrmas.ElementKey);
                    dbCommand.CreateAndAddParameter("@AdrKy", adrmas.AddressKey);

                    response.ExecutionStarted = DateTime.UtcNow;
                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        address = new AddressMaster()
                        {
                            AddressKey = reader.GetColumn<int>("AdrKy"),
                            Address = reader.GetColumn<string>("AdrNm"),
                            AddressId= reader.GetColumn<string>("AdrID"),
                            PostalAddress= reader.GetColumn<string>("Address"),
                            Email= reader.GetColumn<string>("Email"),
                            Mobile= reader.GetColumn<string>("Telephone"),
                            NIC= reader.GetColumn<string>("NIC"),
                        };

                    }

                    response.ExecutionEnded = DateTime.UtcNow;
                    response.Value = address;

                }
                catch (Exception exp)
                {
                    throw exp;
                }

                finally
                {
                    IDbConnection dbConnection = dbCommand.Connection;
                    if (reader != null && !reader.IsClosed)
                    {
                        reader.Close();
                    }

                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }

                    dbCommand.Dispose();
                    dbConnection.Dispose();
                }
                return response;
            }
        }

    }
}
