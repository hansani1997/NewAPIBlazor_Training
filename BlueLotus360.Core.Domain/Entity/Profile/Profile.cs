using BlueLotus360.Core.Domain.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus.Com.Domain.Entity.Profile
{

    //Grid Select
    public class AccountProfileRequest
    {
        public long ElementKey { get; set; }

        public int FrmRow { get; set; } = 1;

        public int ToRow { get; set; } = 999999;

        public string AccountCode { get; set; } = "";

        public string AccountName { get; set; } = "";

        public string OurCode { get; set; } = "";

    }

    public class AccountProfileResponse
    {
        public int AccountKey { get; set; }

        public string AccountCode { get; set; } = "";

        public string AccountName { get; set; } = "";

        public CodeBaseResponse AccountType { get; set; } = new CodeBaseResponse();

        public bool IsActive { get; set; }
    }

    //Insert Item

    public class AccountProfileInsertRequest
    {
        public string AccountCode { get; set; } = "";

        public string AccountName { get; set; } = "";

        public CodeBaseResponse AccountType { get; set; } = new CodeBaseResponse();

        public bool IsActive { get; set; } = true;

    }

    public class AccountProfileInsertResponse
    {
        public int AccountKey { get; set; }

        public int CKy { get; set; }

        public string AccountCode { get; set; } = "";

        public string AccountName { get; set; } = "";

        public int AccTypKy { get; set; }

    }
}