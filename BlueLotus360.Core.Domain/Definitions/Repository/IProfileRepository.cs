using BlueLotus.Com.Domain.Entity;
using BlueLotus.Com.Domain.Entity.Profile;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Core.Domain.Definitions.Repository
{
    public interface IProfileRepository
    {
        BaseServerResponse<IList<AccountProfileResponse>> GetAccountProfileList (Company company, User user, AccountProfileRequest request);
        bool InsertAccountProfile(Company company, AccountProfileInsertRequest accountProfileInsertRequest);

        bool UpdateAccountProfile(Company company, User user, AccountProfileResponse request);
    }
}
