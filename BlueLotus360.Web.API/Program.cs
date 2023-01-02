using BlueLotus360.Core.Domain.Definitions.DataLayer;
using BlueLotus360.Data.SQL92.UnitOfWork;
using BlueLotus360.Web.API.Authentication;
using BlueLotus360.Web.API.Authentication.Providers;


using BlueLotus360.Web.API.Extension;
using BlueLotus360.Web.API.MiddleWares;
using BlueLotus360.Web.APIApplication.Definitions.ServiceDefinitions;
using BlueLotus360.Web.APIApplication.Services;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.OpenApi.Models;

namespace BlueLotus360.Web.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
            });
            
            builder.Services.AddScoped<IAuthenticationProvider, JwtAuthenticatonProvider>();
            builder.Services.ServicesBuilder();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            //app.UseResponseCompression();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.UseSwagger();
                //app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            app.UseMiddleware<UberDataInjectionMiddleware>();    
            app.UseMiddleware<ApplicationAPIBinderMidlleware>();
            app.UseMiddleware<FileRequestLogPersistanceMiddleWare>();
            app.UseMiddleware<AuthenticationProviderMiddleware>();

            app.Run();
        }

    }
}