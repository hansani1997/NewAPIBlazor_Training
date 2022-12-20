using BlueLotus360.Core.Domain.Definitions.Repository;
using BlueLotus360.Core.Domain.Entity.API;
using BlueLotus360.Web.API.Authentication.Providers;
using BlueLotus360.Web.API.Extension;
using BlueLotus360.Web.APIApplication.Definitions.ServiceDefinitions;
using BlueLotus360.Web.APIApplication.Services;
using Microsoft.Extensions.Options;
using System.Net;

namespace BlueLotus360.Web.API.MiddleWares
{
    public class UberDataInjectionMiddleware
    {
        private readonly RequestDelegate _next;
        private IAPIService _apiService;



        public UberDataInjectionMiddleware(RequestDelegate next)
        {
            _next = next;

        }


        public async Task Invoke(HttpContext context, IAPIService aPIService)
        {
            _apiService = aPIService;
            bool LoRequest = true;
            var appId = context.Request.Headers["IntegrationID"].FirstOrDefault();
            if (appId == null)
            {
                appId = "czJTRXfIQOVLxYO2VkQ8E0vvKoNttkyx";
                context.Request.Headers.Add("IntegrationID", appId);
                context.Request.Headers.Add("Authorization", $"Bearer {appId}");
                
            }
            foreach(var item in context.Request.Headers)
            {

            }

            
            await _next(context);
        }

        private async Task ReturnErrorResponse(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync("Bad Request: Integration Not Defined, " +
                "Request ID Sould be defined in the Header section of the request");


        }
    }
}
