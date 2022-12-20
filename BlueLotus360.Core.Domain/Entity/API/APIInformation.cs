using BlueLotus360.Core.Domain.Definitions.Repository;
using BlueLotus360.Core.Domain.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Core.Domain.Entity.API
{
    public class APIInformation:BaseEntity
    {
        public APIInformation()
        {
            Location = new CodeBaseResponse();
            BU= new CodeBaseResponse();
        }
        public int APIIntegrationKey { get; set; } =1;
        public string? APIIntegrationNmae { get; set; }
        public string? Description { get; set; }
        public string? ApplicationID { get; set; }
        public string? SecretKey { get; set; }
        public string? RestrictToIP { get; set; }
        public int MappedCompanyKey { get; set; } = 1;
        public int MappedUserKey { get; set; } = 1;
        public bool IsLocalOnly { get; set; } 
        public bool ISIPFilterd { get; set; }
        public bool ValidateTokenOnly { get; set; }
        public string? Scheme { get; set; }
        public int Direction { get; set; }
        public int MappedLocation { get; set; }
        public string? BaseURL { get; set; }
        public bool IsNonAutoMapped { get; set; }
        public string? AuthenticationType  { get; set; }
        public string? IntegrationId { get; set; }

        public string? SecretInstanceKey { get; set; }
        public int MappedLocationKey { get; set; } = 1;
        public string? MappedLocationName { get; set; }
        public long PartnerOrderTypeKey { get; set; }
        public string? PartnerOrderTypeCode { get; set; }
        public string? PartnerOrderTypeName { get; set; }
        public bool IsAllowedLocalOnly { get; set; }
        public bool IsRestrictedToIP { get; set; }
        public string? AlertnateBaseURL { get; set; }
        public string? EndPointURL { get; set; }
        public string? EndPointToken { get; set; }
        public DateTime TokenGeneratedTime { get; set; }
        public DateTime TokenValidTillTime { get; set; }
        public CodeBaseResponse Location { get; set; }
        public CodeBaseResponse BU { get; set; }
        public TokebGeneratioResponse TokenResp { get; set; }
    }

    public class APIRequestParameters
    {
        public string? EndPointName { get; set; }
        public int APIIntegrationKey { get; set; }
        public string? APIIntegrationName { get; set; }
        public int LocationKey { get; set; }
        public int BUKy { get; set; }
        public string? APIName { get; set; }
        public string? BaseURL { get; set; }
        public string? IntegrationID { get; set; }
        public string? EndPointToken { get; set; }
        public string? EndPointURL { get; set; }
        public DateTime TokenValidTillTime { get; set; }


      

        
    }


    public class TokebGeneratioResponse
    {
        public TokebGeneratioResponse() { 
        ResponseErrors=new Dictionary<string,object>();
        }

       public  IDictionary<string, object> ResponseErrors { get; set; }

        public string Value { get; set; }

    }


     
}
