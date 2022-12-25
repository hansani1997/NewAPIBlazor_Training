using BlueLotus360.Core.Domain.Definitions.DataLayer;
using BlueLotus360.Core.Domain.DTOs.RequestDTO;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.Order;
using BlueLotus360.Core.Domain.Entity.Transaction;
using BlueLotus360.Core.Domain.Responses;
using BlueLotus360.Web.APIApplication.Definitions.ServiceDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BlueLotus360.Web.APIApplication.Services
{
    public class TransactionService : ITransactionService
    {
        public readonly IUnitOfWork _unitOfWork;
        public TransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public BaseServerResponse<BLTransaction> SaveTransaction(BLTransaction transaction, Company company, User user,UIObject uIObject, CodeBaseResponse trnTyp)
        {

            transaction.TransactionType = new CodeBaseResponse();
            transaction.TransactionType.CodeKey = trnTyp.CodeKey;

            BaseServerResponse <BLTransaction> response = new BaseServerResponse<BLTransaction>();

            if (BaseComboResponse.GetKeyValue(transaction.Address) < 11)
            {
                var address = _unitOfWork.AccountRepository.GetAddressByAccount(company, user, transaction.Account.AccountKey);
                if(address!=null)
                    transaction.Address = address.Value;
            }
            if (!transaction.IsPersisted)
            {
                response = _unitOfWork.TransactionRepository.SaveGenericTransaction(company, user, transaction);

            }
            else if (transaction.IsPersisted)
            {
                response = _unitOfWork.TransactionRepository.UpdateGenericTransaction(company, user, transaction );
            }

            if (transaction.SerialNumber != null && !string.IsNullOrWhiteSpace(transaction.SerialNumber.SerialNumber))
            {
                transaction.SerialNumber.TransactionKey = transaction.TransactionKey;
                _unitOfWork.TransactionRepository.SaveOrUpdateTranHeaderSerialNumber(company, user, transaction.SerialNumber);
            }

            foreach (GenericTransactionLineItem line in transaction.InvoiceLineItems)
            {
                line.ElementKey = transaction.ElementKey;
                line.TransactionKey = transaction.TransactionKey;
                line.TransactionType = transaction.TransactionType;
                line.Address = transaction.Address;
                line.TransactionLocation = transaction.Location;
                line.EffectiveDate = transaction.TransactionDate;
                line.DeliveryDate = transaction.DeliveryDate;
                if (!line.IsPersisted)
                {
                    _unitOfWork.TransactionRepository.SaveTransactionLineItem(company, user, line);
                }
                else if (line.IsPersisted && line.IsDirty)
                {
                    _unitOfWork.TransactionRepository.UpdateTransactionLineItem(company, user, line);
                }


                foreach (ItemSerialNumber serialNumber in line.SerialNumbers)
                {
                    serialNumber.ItemTransactionKey = line.ItemTransactionKey;
                    serialNumber.ItemKey = line.TransactionItem.ItemKey;
                    serialNumber.PersistingElementKey = transaction.ElementKey;
                    _unitOfWork.TransactionRepository.SaveOrUpdateSerialNumber(company, user, serialNumber);
                }

            }
            _unitOfWork.TransactionRepository.PostAfterTranSaveActions(company, user, transaction.TransactionKey, transaction.ElementKey);

            return response;
        }

        public BaseServerResponse<IList<GenericTransactionFindResponse>> FindTransaction(Company company, User user, TransactionFindRequest request)
        {
            return _unitOfWork.TransactionRepository.GenericFindTransaction(company, user, request);
        }
        public BaseServerResponse<BLTransaction> OpenTransaction(Company company, User user, TransactionOpenRequest request)
        {
            return _unitOfWork.TransactionRepository.GenericOpenTransaction(company, user, request);
        }
        public BaseServerResponse<IList<GenericTransactionLineItem>> GetTransactionLineItems(Company company, User user, TransactionOpenRequest request)
        {
            return _unitOfWork.TransactionRepository.GenericallyGetTransactionLineItems(company, user, request);    
        }

        public CodeBaseResponse ChangeTrnHdrAprSts(Company company, User user, int trnky, int aprstsky, int objky, int isAct, string ourcd)
        {
            _unitOfWork.TransactionRepository.TransactionHeaderApproveInsert(trnky, aprstsky, objky, isAct,ourcd, company, user);
            CodeBaseResponse latedtApproveState= _unitOfWork.TransactionRepository.TrnrApproveStatusFindByTrnKy(company, user, objky, trnky);
            return latedtApproveState;  
        }
        public TransactionPermission CheckSourceDocPrintPermission(int trnky, int aprstsky, int objky, int trnTypKy, Company company, User user)
        {
            var per=_unitOfWork.TransactionRepository.CheckTranPrintPermission(trnky, aprstsky, objky, trnTypKy, company, user);
            return per.Value;
        }

        public CodeBaseResponse TrnHdrNextApproveStatus(int aprstsky, int objky, int trnTypKy, Company company, User user)
        {
            var per = _unitOfWork.TransactionRepository.TrnHdrNextApproveStatus(aprstsky, objky, trnTypKy, company, user);
            return per.Value;
        }
        public TransactionPermission GetPermissionForOrderTrn(int aprstsky, int objky, int trnTypKy,int trnky, Company company, User user)
        {
            var per = _unitOfWork.TransactionRepository.GetIsALwAddUpdatePermissionForOrderTrn(company,user, objky, trnky, aprstsky);
            return per.Value;
        }

        public RecviedAmountResponse GetRecviedAmountResponse(Company company, User user, RecieptDetailRequest request)
        {
            return _unitOfWork.TransactionRepository.GetRecviedAmountResponse(company,user,request);
        }

        public void SaveAccountResponseEx(Company company, User user, PayementModeReciept payementModeReciept)
        {
            _unitOfWork.AccountRepository.SaveAccountResponseExRepo(company,user,payementModeReciept);
        }
    }
}
