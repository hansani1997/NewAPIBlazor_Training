using BlueLotus360.Core.Domain.Definitions.Repository;
using BlueLotus360.Core.Domain.DTOs.RequestDTO;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.Payment;
using BlueLotus360.Core.Domain.Entity.UberEats;
using BlueLotus360.Core.Domain.Responses;
using BlueLotus360.Data.SQL92.Definition;
using BlueLotus360.Data.SQL92.Extenstions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Data.SQL92.Repository
{
    internal class AccountRepository: BaseRepository, IAccountRepository
    {
        public AccountRepository(ISQLDataLayer dataLayer) : base(dataLayer)
        {

        }

        public BaseServerResponse<AddressResponse> GetAddressByAccount(Company company, User user, long AccountKey = 1)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                string SPName = "Address_SelectByAccKy";
                BaseServerResponse<AddressResponse> response = new BaseServerResponse<AddressResponse>();
                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    CreateAndAddParameter(dbCommand,"@AccKy", AccountKey);
                    response.ExecutionStarted = DateTime.UtcNow;
                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();
                    AddressResponse adrResponse = new AddressResponse();

                    while (reader.Read())
                    {
                        adrResponse.AddressId = reader.GetColumn<string>("AdrId");
                        adrResponse.AddressName = reader.GetColumn<string>("AdrNm");
                        adrResponse.AddressKey = reader.GetColumn<long>("AdrKy");
                    }
                    response.ExecutionEnded = DateTime.UtcNow;
                    response.Value = adrResponse;

                }
                catch (Exception exp)
                {
                    response.ExecutionEnded = DateTime.UtcNow;
                    response.Messages.Add(new ServerResponseMessae()
                    {
                        MessageType = ServerResponseMessageType.Exception,
                        Message = $"Error While Executing Proc {SPName}"
                    });
                    response.ExecutionException = exp;
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

        public BaseServerResponse<AccountResponse> GetAccountByAddress(AddressResponse address, Company company, User user)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader dataReader = null;
                string SPName = "AccAdr_SelectSingleWebV2";
                BaseServerResponse<AccountResponse> baseResponse = new BaseServerResponse<AccountResponse>();
                try
                {
                    AccountResponse response = new AccountResponse();
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    dbCommand.CreateAndAddParameter("@AdrKy", address.AddressKey);
                    dbCommand.CreateAndAddParameter("@Cky", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("@UsrKy", user.UserKey);

                    baseResponse.ExecutionStarted = DateTime.UtcNow;
                    dbCommand.Connection.Open();
                    dataReader = dbCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        response.AccountKey = dataReader.GetColumn<long>("AccKy");
                        response.AccountCode = dataReader.GetColumn<string>("AccCd");
                        response.AccountName = dataReader.GetColumn<string>("AccNm");
                    }
                    baseResponse.ExecutionEnded = DateTime.UtcNow;
                    baseResponse.Value = response;

                }
                catch (Exception exp)
                {
                    baseResponse.ExecutionEnded = DateTime.UtcNow;
                    baseResponse.Messages.Add(new ServerResponseMessae()
                    {
                        MessageType = ServerResponseMessageType.Exception,
                        Message = $"Error While Executing Proc {SPName}"
                    });
                    baseResponse.ExecutionException = exp;
                }

                finally
                {
                    IDbConnection dbConnection = dbCommand.Connection;
                    if (dataReader != null)
                    {
                        if (!dataReader.IsClosed)
                        {
                            dataReader.Close();
                        }
                    }
                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }
                    dataReader.Dispose();
                    dbCommand.Dispose();
                    dbConnection.Dispose();
                }
                return baseResponse;
            }
        }

        public BaseServerResponse<IList<AccountResponse>> GetAccounts(Company company, User user, ComboRequestDTO requestDTO)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader dataReader = null;
                string SPName = "AccCdNm_SelectMob";
                BaseServerResponse<IList<AccountResponse>> response = new BaseServerResponse<IList<AccountResponse>>();
                try
                {
                    IList<AccountResponse> accounts = new List<AccountResponse>();



                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    dbCommand.CreateAndAddParameter("@Cky", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("@UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("@ObjKy", requestDTO.RequestingElementKey);
                    dbCommand.CreateAndAddParameter("@PreKy", requestDTO.PreviousKey);
                    dbCommand.CreateAndAddParameter("@TrnTypKy", requestDTO.TransactionTypeKey);

                    response.ExecutionStarted = DateTime.UtcNow;
                    dbCommand.Connection.Open();
                    dataReader = dbCommand.ExecuteReader();
                    AccountResponse account;
                    while (dataReader.Read())
                    {
                        account = new AccountResponse();
                        account.AccountKey = dataReader.GetColumn<int>("AccKy");
                        account.AccountName = dataReader.GetColumn<string>("AccCdNm");
                        account.IsDefault = account.AccountKey == dataReader.GetColumn<int>("DefaultKey");
                        accounts.Add(account);
                    }

                    response.ExecutionEnded = DateTime.UtcNow;
                    response.Value = accounts;

                }
                catch (Exception exp)
                {
                    response.ExecutionEnded = DateTime.UtcNow;
                    response.Messages.Add(new ServerResponseMessae()
                    {
                        MessageType = ServerResponseMessageType.Exception,
                        Message = $"Error While Executing Proc  {SPName}"
                    });
                    response.ExecutionException = exp;
                }

                finally
                {
                    IDbConnection dbConnection = dbCommand.Connection;
                    if (dataReader != null)
                    {
                        if (!dataReader.IsClosed)
                        {
                            dataReader.Close();
                        }
                    }
                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }
                    dataReader.Dispose();
                    dbCommand.Dispose();
                    dbConnection.Dispose();
                }
                return response;
            }
        }

        public IList<AccPaymentMappingResponse> GetAccPaymentMappingV1(Company company, User user, AccPaymentMappingRequest requestDTO)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader dataReader = null;
                string SPName= "PmtModeAccDefLaund_SelectWeb";
                try
                {
                    IList<AccPaymentMappingResponse> accounts = new List<AccPaymentMappingResponse>();



                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    dbCommand.CreateAndAddParameter("@Cky", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("@UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("@ObjKy", requestDTO.ELementKey);
                    dbCommand.CreateAndAddParameter("@LocKy", requestDTO.Location.CodeKey);
                    dbCommand.CreateAndAddParameter("@PmtTrmKy", requestDTO.PayementTerm.CodeKey);
                    dbCommand.CreateAndAddParameter("@isLoadAll", requestDTO.LoadAll);
                    dbCommand.Connection.Open();
                    dataReader = dbCommand.ExecuteReader();
                    AccPaymentMappingResponse mapping;
                    while (dataReader.Read())
                    {
                        mapping = new AccPaymentMappingResponse();
                        mapping.Account.AccountKey = dataReader.GetColumn<int>("AccKy");
                        mapping.Account.AccountName = dataReader.GetColumn<string>("AccNm");
                        mapping.PayementMode.CodeKey = dataReader.GetColumn<int>("PmtModeKy");
                        mapping.PayementMode.CodeName = dataReader.GetColumn<string>("PmtMode");
                        accounts.Add(mapping);
                    }

                    return accounts;

                }
                catch (Exception exp)
                {
                    throw new Exception("Error While Executing SP PmtModeAccDefLaund_SelectWeb", exp);
                }

                finally
                {
                    IDbConnection dbConnection = dbCommand.Connection;
                    if (dataReader != null)
                    {
                        if (!dataReader.IsClosed)
                        {
                            dataReader.Close();
                        }
                    }
                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }
                    dataReader.Dispose();
                    dbCommand.Dispose();
                    dbConnection.Dispose();
                }

            }
        }

        public void SaveAccountResponseExRepo(Company company, User user, PayementModeReciept payment)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                //IDataReader dataReader = null;
                string SPName = "PSaleKyPmtModeWiseRecp_PostWeb";
                try
                {

                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    dbCommand.CreateAndAddParameter("@CKy", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("@UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("@ObjKy", payment.ElementKey);
                    dbCommand.CreateAndAddParameter("@Dt", payment.PayementDate);
                    dbCommand.CreateAndAddParameter("@TrnKy", payment.TransactionKey);
                    dbCommand.CreateAndAddParameter("@OurCd", payment.OurCode??"");
                    dbCommand.CreateAndAddParameter("@PmtAmt", GetPayementModeRecieptTable(payment.Payements));
                    dbCommand.Connection.Open();

                    dbCommand.ExecuteNonQuery();



                }
                catch (Exception exp)
                {
                    throw exp;
                }

                finally
                {
                    IDbConnection dbConnection = dbCommand.Connection;

                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }

                    dbCommand.Dispose();
                    dbConnection.Dispose();
                }

            }
        }
    }
}
