using BlueLotus360.Core.Domain.DTOs.RequestDTO;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.Transaction;
using BlueLotus360.Core.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Web.APIApplication.Definitions.ServiceDefinitions
{
    public interface IItemService
    {
        BaseServerResponse<IList<ItemSimple>> GetItems(Company company, User user, ComboRequestDTO comboRequest);
        ItemRateResponse GetItemRateEx(RateRetrivalModel rateRetrivalModel, Company company, User user,CodeBaseResponse type);
        StockAsAtResponse GetStockAsAtByLocation(Company company, User user, StockAsAtRequest request);
        IList<ItemSerialNumber> GetSerialNumbers(Company company, User user, ComboRequestDTO comboRequest);
        StockAsAtResponse GetAvailableStock(Company company, User user, StockAsAtRequest request);
    }
}
