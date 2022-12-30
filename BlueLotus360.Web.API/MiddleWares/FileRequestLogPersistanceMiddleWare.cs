using BlueLotus360.Core.Domain.Entity.API;
using BlueLotus360.Web.API.Extension;
using BlueLotus360.Web.APIApplication.Definitions.ServiceDefinitions;
using BlueLotus360.Web.APIApplication.Services;

namespace BlueLotus360.Web.API.MiddleWares
{
    public class FileRequestLogPersistanceMiddleWare
    {
        private IAPIService _apiService;
        private readonly RequestDelegate _next;

        public FileRequestLogPersistanceMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IAPIService aPIService)
        {
            _apiService = aPIService;
            context.Request.EnableBuffering();
            string BodyString = "UN-Read";
            //using (StreamReader reader
            //  = new StreamReader(context.Request.Body))
            //{
            //    BodyString = await reader.ReadToEndAsync();
            //}

         var apiInfo =   context.Items["APIInformation"] as APIInformation;
            if(apiInfo != null)
            {
                if(apiInfo.LogType==RequestLogMode.DataBase)
                {
                    var requestBody = await context.Request.GetRequestBodyAsStringAsync();
                    APIRequestLogDetail detail = new APIRequestLogDetail();
                    string controller = context.Request.RouteValues["controller"].ToString();
                    string action = context.Request.RouteValues["action"].ToString();


                    detail.Action = action;
                    detail.Controller = controller;
                    detail.ApplicationId = apiInfo.ApplicationID;
                    detail.RequestBody = requestBody;
                   _apiService.SaveRequestLog(detail);

                  //  detail.RequestBody;   
                }


            }


            await _next(context);
        }
    }
}