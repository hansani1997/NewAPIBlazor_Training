using BlueLotus360.Core.Domain.Definitions.DataLayer;
using BlueLotus360.Core.Domain.Definitions.Repository;
using BlueLotus360.Data.SQL92.DataLayer;
using BlueLotus360.Data.SQL92.Definition;
using BlueLotus360.Data.SQL92.Repository;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Data.SQL92.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Privates
        private IUserRepository _userRepository;
        private ICompanyRepository _companyRepository;
        private IAPIRepository _apiRepository;
        private IObjectRepository _objectRepository;
        private ICodeBaseRepository _codeBaseRepository;
        private IAccountRepository _accountRepository;
        private ITransactionRepository _transactionRepository;
        private IOrderRepository _orderRepository;
        private IItemRepository _itemRepository;
        private IMenuRepository _menuRepository;
        private IUnitRepository _unitRepository;
        private IAddressRepository _addressRepository;
        private ICommonRepository _commonRepository;    
        private IWorkShopManagementRepository _workShopManagementRepository;
        private IDocumentRepository _documentRepository;
        private IProjectRepository _projectRepository;
        private IBookingModuleRepository _bookingModuleRepository;
        private IProfileRepository _profileRepository;
        #endregion



        public IUserRepository UserRepository { get { 
            if( _userRepository == null)
                {
                    _userRepository = new UserRepository(_dataLayer);
                }
            return _userRepository;
            }
        }

        public IBookingModuleRepository BookingModuleRepository
        {
            get 
            {
                if (_bookingModuleRepository == null) 
                {
                    _bookingModuleRepository = new BookingModuleRepository(_dataLayer);
                }
                return _bookingModuleRepository; 
            }
        }
        public IAddressRepository AddressRepository
        {
            get
            {
                if (_addressRepository == null)
                {
                    _addressRepository = new AddressRepository(_dataLayer);
                }
                return _addressRepository;
            }
        }

        public IAPIRepository APIRepository
        {
            get
            {
                if (_apiRepository == null)
                {
                    _apiRepository = new APIRepository(_dataLayer);
                }
                return _apiRepository;
            }
        }

        public ICommonRepository CommonRepository
        {
            get
            {
                if (_commonRepository == null)
                {
                    _commonRepository = new CommonRepository(_dataLayer);
                }
                return _commonRepository;
            }
        }

        public ICompanyRepository CompanyRepository
        {
            get
            {
                if (_companyRepository == null)
                {
                    _companyRepository = new CompanyRepository(_dataLayer);
                }
                return _companyRepository;
            }
        }

        public ICodeBaseRepository CodeBaseRepository
        {
            get
            {
                if (_codeBaseRepository == null)
                {
                    _codeBaseRepository = new CodeBaseRepository(_dataLayer);
                }
                return _codeBaseRepository;
            }
        }

        public IObjectRepository ObjectRepository
        {
            get
            {
                if (_objectRepository == null)
                {
                    _objectRepository = new ObjectRepository(_dataLayer);
                }
                return _objectRepository;
            }
        }

        public IAccountRepository AccountRepository
        {
            get
            {
                if (_accountRepository == null)
                {
                    _accountRepository = new AccountRepository(_dataLayer);
                }
                return _accountRepository;
            }
        }

        public ITransactionRepository TransactionRepository
        {
            get
            {
                if (_transactionRepository == null)
                {
                    _transactionRepository = new TransactionRepository(_dataLayer);
                }
                return _transactionRepository;
            }
        }
        public IOrderRepository OrderRepository
        {
            get
            {
                if (_orderRepository == null)
                {
                    _orderRepository = new OrderRepository(_dataLayer);
                }
                return _orderRepository;
            }
        }

        public IItemRepository ItemRepository
        {
            get
            {
                if (_itemRepository == null)
                {
                    _itemRepository = new ItemRepository(_dataLayer);
                }
                return _itemRepository;
            }
        }
        public IMenuRepository MenuRepository
        {
            get
            {
                if (_menuRepository == null)
                {
                    _menuRepository = new MenuRepository(_dataLayer);
                }
                return _menuRepository;
            }
        }

        public IUnitRepository UnitMasRepository
        {
            get
            {
                if (_unitRepository == null)
                {
                    _unitRepository = new UnitRepository(_dataLayer);
                }
                return _unitRepository;
            }
        }

        public IWorkShopManagementRepository WorkShopManagementRepository
        {
            get
            {
                if (_workShopManagementRepository == null)
                {
                    _workShopManagementRepository = new WorkShopManagementRepository(_dataLayer);
                }
                return _workShopManagementRepository;
            }
        }

        public IDocumentRepository DocumentRepository
        {
            get
            {
                if (_documentRepository == null)
                {
                    _documentRepository = new DocumentRepository(_dataLayer);
                }
                return _documentRepository;
            }
        }

        public IProjectRepository ProjectRepository
        {
            get
            {
                if (_projectRepository == null)
                {
                    _projectRepository = new ProjectRepository(_dataLayer);
                }
                return _projectRepository;
            }
        }

        //ProfileRepo
        public IProfileRepository ProfileRepository
        {
            get
            {
                if (_profileRepository == null)
                {
                    _profileRepository = new ProfileRepository(_dataLayer);
                }
                return _profileRepository;
            }
        }

        private ISQLDataLayer _dataLayer;
        private string _connectionStrin;
        IConfiguration _configuration;

        public UnitOfWork(IConfiguration configuration)
        {
            try
            {
                var configurationSection = configuration.GetSection("ConnectionStrings:BL10DATA"); 
                if(configurationSection != null && configurationSection.Value!=null)
                {
                    _connectionStrin = configurationSection.Value;
                    _dataLayer = new SQLDataLayer(_connectionStrin);

                }
                else
                {
                    throw new InvalidOperationException("Cannot Read Data Configuration");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ex");
            }


        }
    }
}
