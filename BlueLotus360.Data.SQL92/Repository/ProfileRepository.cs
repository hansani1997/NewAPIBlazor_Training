using BlueLotus.Com.Domain.Entity;
using BlueLotus.Com.Domain.Entity.Profile;
using BlueLotus360.Core.Domain.Definitions.Repository;
using BlueLotus360.Core.Domain.Entity.Base;
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
    internal class ProfileRepository : BaseRepository, IProfileRepository
    {
        public ProfileRepository(ISQLDataLayer datalayer) : base(datalayer)
        {

        }
        //get account list
        public BaseServerResponse<IList<AccountProfileResponse>> GetAccountProfileList(Company company, User user, AccountProfileRequest request)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                string SPName = "AccMas_SelectWeb";
                BaseServerResponse<IList<AccountProfileResponse>> responses = new BaseServerResponse<IList<AccountProfileResponse>>();
                IList<AccountProfileResponse> list = new List<AccountProfileResponse>();

                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    dbCommand.CreateAndAddParameter("@Cky", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("@UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("@ObjKy", request.ElementKey);
                    dbCommand.CreateAndAddParameter("@FrmRow", request.FrmRow);
                    dbCommand.CreateAndAddParameter("@ToRow", request.ToRow);
                    dbCommand.CreateAndAddParameter("@AccCd", request.AccountCode);
                    dbCommand.CreateAndAddParameter("@AccNm", request.AccountName);
                    if (request.OurCode.Equals("")) { request.OurCode = null; }
                    dbCommand.CreateAndAddParameter("@OurCode", request.OurCode);

                    responses.ExecutionStarted = DateTime.UtcNow;
                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        AccountProfileResponse response = new AccountProfileResponse();

                        response.AccountKey = reader.GetColumn<int>("AccKy");
                        response.AccountCode = reader.GetColumn<string>("AccCd");
                        response.AccountName = reader.GetColumn<string>("AccNm");
                        response.AccountType.CodeKey = reader.GetColumn<long>("AccTypKy");
                        response.AccountType.CodeName = reader.GetColumn<string>("AccTypNm");
                        response.IsActive = Convert.ToBoolean(reader.GetColumn<byte>("isAct"));

                        if (response.IsActive)
                        {
                            list.Add(response);
                        }
                    }
                    responses.ExecutionEnded = DateTime.UtcNow;
                    responses.Value = list;
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
                        reader.Dispose();
                    }

                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }

                    dbCommand.Dispose();
                    dbConnection.Dispose();
                }

                return responses;
            }

        }

        //Insert account
        public bool InsertAccountProfile(Company company,AccountProfileInsertRequest request)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                AccountProfileInsertResponse response = new AccountProfileInsertResponse();
                IDataReader reader = null;
                string SPName = "AccMas_InsertWeb";

                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;

                    dbCommand.CreateAndAddParameter("@CKy", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("@AccCd ", request.AccountCode);
                    dbCommand.CreateAndAddParameter("@AccNm  ", request.AccountName);
                    dbCommand.CreateAndAddParameter("@AccTypKy", request.AccountType.CodeKey < 11 ? 1 : request.AccountType.CodeKey);
                    dbCommand.CreateAndAddParameter("@isAct", Convert.ToInt32(request.IsActive));

                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();

                    while (reader.Read())
                    {

                    }


                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                    return true;
                }
                catch (Exception exp)
                {
                    return false;
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
                    if (reader!=null)
                    {
                        reader.Dispose();
                    }
                    
                    dbCommand.Dispose();
                    dbConnection.Dispose();
                }
            }
        }

        //update account
        public bool UpdateAccountProfile(Company company, User user, AccountProfileResponse request)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                string SPName = "AccMasV2_UpdateWeb";

                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;

                    dbCommand.CreateAndAddParameter("@AccKy", request.AccountKey);
                    dbCommand.CreateAndAddParameter("@CKy", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("@UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("@AccCd", request.AccountCode);
                    dbCommand.CreateAndAddParameter("@AccNm", request.AccountName);

                    //need not to update account type
                    //dbCommand.CreateAndAddParameter("@AccTypKy", request.AccountType.CodeKey < 11 ? 1 : request.AccountType.CodeKey);

                    int ch = Convert.ToInt32(request.IsActive);
                    dbCommand.CreateAndAddParameter("@isAct", ch);

                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();

                    while (reader.Read())
                    {

                    }

                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                    return true;
                }
                catch (Exception exp)
                {
                    return false;
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
                    reader.Dispose();
                    dbCommand.Dispose();
                    dbConnection.Dispose();
                }
            }
        }

       
    }
}
