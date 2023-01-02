using BlueLotus360.Core.Domain.Entity.API;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.UberEats;
using BlueLotus360.Web.APIApplication.Definitions.ServiceDefinitions;
using BlueLotus360.Web.APIApplication.Services;
using Newtonsoft.Json;
using RestSharp;
using System.Text.Json;

namespace BlueLotus360.Web.API.Integrations.Uber
{
    public class UberTokenHandler
    {
        IOrderService _orderService;
        public UberTokenHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public TokebGeneratioResponse UberToken(APIInformation CommonAPIInfo,APIInformation aPIInformation,string EndPointName,string RedirectURL,string code,int CompanyKey)
        {
            /*
             * 1. Get client id, client secret, api integration key
             * 2.using api integration key get endpoint details
             * 3.check for provision or not and token available or expire
             * 4. use token if available
             * 5. token not availble or expire generate new token and save db
            */

            TokebGeneratioResponse response= new TokebGeneratioResponse();
            if (aPIInformation.EndPointToken != "" && aPIInformation.EndPointToken != "1" && DateTime.Compare(DateTime.Today, aPIInformation.TokenValidTillTime) < 0)
            {
                response.Value= aPIInformation.EndPointToken;

            }
            else
            {
                string genratedToken = string.Empty;

                if (EndPointName == "eats.pos_provisioning")
                {
                    response = UberProvisioningTokenGeneratingService(CommonAPIInfo, aPIInformation, EndPointName, RedirectURL, code,CompanyKey);
                }
                else
                {
                    response = UberTokenGeneratingService(CommonAPIInfo, aPIInformation, EndPointName, 1);
                }

              
            }
            return response;
        }

        public TokebGeneratioResponse UberProvisioningTokenGeneratingService(APIInformation UberData,APIInformation EndpointData,string EndPointName,string RedirectURL,string code,int CompanyKey)
        {

            TokebGeneratioResponse resp = new();
            var client = new RestClient(UberData.AlertnateBaseURL);
            var request = new RestRequest(EndpointData.EndPointURL,Method.Post);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("client_id", UberData.IntegrationId);
            request.AddParameter("client_secret", UberData.SecretInstanceKey);
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("scope", EndPointName);
            request.AddParameter("redirect_uri", RedirectURL);
            request.AddParameter("code", code);
            RestResponse response = client.Execute(request);
             
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            if (response.IsSuccessful)
            {
                UberTokenResponse uberTokenResponse = JsonConvert.DeserializeObject<UberTokenResponse>(response.Content, settings);
                APIRequestParameters endpoint = new APIRequestParameters()
                {

                    APIIntegrationKey = UberData.APIIntegrationKey,
                    EndPointName = EndPointName,
                    EndPointToken = uberTokenResponse.Access_token,
                    EndPointURL = EndpointData.EndPointURL,
                    TokenValidTillTime = DateTime.Today.AddDays(365 - 30)
                };
                Company Assignedcompany = new Company();
                Assignedcompany.CompanyKey = CompanyKey;
                _orderService.InsertApiEndPoint(endpoint, Assignedcompany);
                resp.Value =  uberTokenResponse.Access_token;
            }
            else
            {
                
                resp.Value = string.Empty;
            }
            resp.ResponseErrors.Add("UberResponse",response);
            return resp;
        }

        public TokebGeneratioResponse UberTokenGeneratingService(APIInformation CommonApiInfo, APIInformation EndPointApiInfo, string Scope, int CompanyKey)
        {
            /*
             * 1. check token is exists or expire (token expires in 30 days)
             * 2. generate new token if not exists or expire
            */
            TokebGeneratioResponse resp = new();
            if (CommonApiInfo.AlertnateBaseURL != null && EndPointApiInfo.EndPointURL != null && CommonApiInfo.SecretInstanceKey != null)
            {
                var client = new RestClient(CommonApiInfo.AlertnateBaseURL);
                var request = new RestRequest(EndPointApiInfo.EndPointURL,Method.Post);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("client_id", CommonApiInfo.IntegrationId);
                request.AddParameter("client_secret", CommonApiInfo.SecretInstanceKey);
                request.AddParameter("grant_type", "client_credentials");
                request.AddParameter("scope", Scope); // scope differs
                RestResponse response = client.Execute(request);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                if (response.IsSuccessful)
                {
                    UberTokenResponse uberTokenResponse = JsonConvert.DeserializeObject<UberTokenResponse>(response.Content, settings);
                    APIRequestParameters endpoint = new APIRequestParameters()
                    {

                        APIIntegrationKey = CommonApiInfo.APIIntegrationKey,
                        EndPointName = Scope,
                        EndPointToken = uberTokenResponse.Access_token,
                        EndPointURL = EndPointApiInfo.EndPointURL,
                        TokenValidTillTime = DateTime.Today.AddDays(30 - 7)
                    };
                    Company Assignedcompany = new Company();
                    Assignedcompany.CompanyKey = CompanyKey;
                    _orderService.InsertApiEndPoint(endpoint, Assignedcompany);
                    resp.Value = uberTokenResponse.Access_token;
                }
                else
                {
                    resp.Value = string.Empty;
                }
            }
            else
            {
                resp.Value = string.Empty;
            }

            return resp;

        }

        public APIInformation GetUberEatsTokensByEndPointName(APIInformation UberData, APIInformation EndpointData, string EndPointName, string RedirectURL, string code, int CompanyKey)
        {
            APIInformation returnInfo = new APIInformation();

           var obj = UberToken(UberData, EndpointData,  EndPointName, RedirectURL, code,CompanyKey);
            returnInfo.EndPointToken = obj.Value;
            returnInfo.TokenResp = obj;
           returnInfo.APIIntegrationKey = UberData.APIIntegrationKey;
            returnInfo.BaseURL = UberData.BaseURL;
            returnInfo.AlertnateBaseURL = UberData.AlertnateBaseURL;
            return returnInfo;
        }

    }
}
