using BlueLotus360.Core.Domain.Definitions.Repository;
using BlueLotus360.Core.Domain.Entity.API;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Responses;
using BlueLotus360.Data.SQL92.Definition;
using BlueLotus360.Data.SQL92.Extenstions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Data.SQL92.Repository
{
    internal class APIRepository : BaseRepository, IAPIRepository
    {
        public APIRepository(ISQLDataLayer dataLayer) : base(dataLayer)
        {
        }

        public BaseServerResponse<APIInformation> GetAPIInformationByAppId(string appId)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                BaseServerResponse<APIInformation> response = new BaseServerResponse<APIInformation>();
                APIInformation information = new APIInformation();
                string SPName = "BL10API_SelectWeb";
                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    CreateAndAddParameter(dbCommand, "@integrationId", appId);
                    response.ExecutionStarted = DateTime.UtcNow;
                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();
                    

                    while (reader.Read())
                    {
                        information.APIIntegrationKey = reader.GetColumn<int>("ApiIntgrKy");
                        information.APIIntegrationNmae = reader.GetColumn<string>("ApiIntNm");
                        information.Description = reader.GetColumn<string>("Des");
                        information.ApplicationID = reader.GetColumn<string>("APPID");
                        information.SecretKey = reader.GetColumn<string>("SecretKey");
                        information.MappedCompanyKey = reader.GetColumn<int>("MappedCky");
                        information.MappedUserKey = reader.GetColumn<int>("MappedCky");
                        information.IsLocalOnly = reader.GetColumn<bool>("IsLocalOnly");
                        information.IsActive = reader.GetColumn<int>("IsAct");
                        information.RestrictToIP = reader.GetColumn<string>("RestricrtToIP");
                        information.ISIPFilterd = reader.GetColumn<bool>("ISIPFilterd");
                        information.ValidateTokenOnly = reader.GetColumn<bool>("ValTokenOnly");
                        information.Scheme = reader.GetColumn<string>("Scheme");
                        information.BaseURL = reader.GetColumn<string>("BaseURL");
                        information.AuthenticationType = reader.GetColumn<string>("Type");
                        information.LogType = (RequestLogMode)(reader.GetColumn<int>("LogMode")) ;
                    }
                    response.ExecutionEnded = DateTime.UtcNow;
                    response.Value = information;

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



        public BaseServerResponse<object> SaveRequestLog(APIRequestLogDetail logDetail)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {

                BaseServerResponse<object> serverResponse = new BaseServerResponse<object>(); ;
                try
                {
                    long logDetailKey = 1;
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = "APIRequestLogDet_InsertWeb";
                    dbCommand.CreateAndAddParameter("@ApplicationID", logDetail.ApplicationId);
                    dbCommand.CreateAndAddParameter("@Controller", logDetail.Controller);
                    dbCommand.CreateAndAddParameter("@Action", logDetail.Action);
                    dbCommand.CreateAndAddParameter("@RequestBody", logDetail.RequestBody);
                    dbCommand.CreateAndAddParameter("@IPAddress", logDetail.IPAddress);
                    IDbDataParameter outParam = dbCommand.CreateAndAddParameter("@APIRequestLogDetKy", "1");
                    outParam.Direction = ParameterDirection.Output;
                    outParam.DbType = DbType.Int64;
                    dbCommand.Connection.Open();
                    dbCommand.ExecuteNonQuery();
                    logDetail.APIRequestLogDetailKey = Convert.ToInt64(outParam.Value);

                    serverResponse.Value = logDetailKey;
                 



                }
                catch (Exception exp)
                {
                    
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

                return serverResponse;

            }
        }
    }
}
