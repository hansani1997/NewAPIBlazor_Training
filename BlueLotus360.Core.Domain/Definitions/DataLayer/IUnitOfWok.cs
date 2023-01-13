using BlueLotus360.Core.Domain.Definitions.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Core.Domain.Definitions.DataLayer
{
    public interface IUnitOfWork     {


        IUserRepository UserRepository { get; }
        IMenuRepository MenuRepository { get; } 
        ICompanyRepository CompanyRepository { get; }
        IAPIRepository APIRepository { get; }
        IObjectRepository ObjectRepository { get; }
        ICodeBaseRepository CodeBaseRepository { get; }
        IAccountRepository AccountRepository { get; }
        ITransactionRepository TransactionRepository { get; }
        IOrderRepository OrderRepository{get;}
        IItemRepository ItemRepository { get; }
        IUnitRepository UnitMasRepository { get; }
        IAddressRepository AddressRepository { get; }
        ICommonRepository CommonRepository { get; }
        IWorkShopManagementRepository WorkShopManagementRepository { get; }
        IDocumentRepository DocumentRepository { get; }
        IProjectRepository ProjectRepository { get; }
        IBookingModuleRepository BookingModuleRepository { get; }
        IProfileRepository ProfileRepository { get; }
    }
}
