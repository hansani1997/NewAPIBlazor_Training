using BlueLotus360.Core.Domain.DTOs.RequestDTO;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.Transaction;
using BlueLotus360.Core.Domain.Entity.WorkOrder;
using BlueLotus360.Core.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Core.Domain.Definitions.Repository
{
    public interface ITransactionRepository
    {
        BaseServerResponse<BLTransaction> SaveGenericTransaction(Company company, User user, BLTransaction transaction);
        BaseServerResponse<BLTransaction> UpdateGenericTransaction(Company company, User user, BLTransaction transaction);
        void SaveOrUpdateTranHeaderSerialNumber(Company company, User user, ItemSerialNumber serialNumber);
        void SaveTransactionLineItem(Company company, User user, GenericTransactionLineItem lineItem);
        void UpdateTransactionLineItem(Company company, User user, GenericTransactionLineItem lineItem);
        void SaveOrUpdateSerialNumber(Company company, User user, ItemSerialNumber serialNumber);
        void PostAfterTranSaveActions(Company company, User user, long TransactionKey, long ObjectKey);
        BaseServerResponse<IList<GenericTransactionFindResponse>> GenericFindTransaction(Company company, User user, TransactionFindRequest request);
        BaseServerResponse<BLTransaction> GenericOpenTransaction(Company company, User user, TransactionOpenRequest trnRequest);
        BaseServerResponse<IList<GenericTransactionLineItem>> GenericallyGetTransactionLineItems(Company company, User user, TransactionOpenRequest request);
        BaseServerResponse<BLTransaction> GenericOpenTransactionV2(Company company, User user, TransactionOpenRequest trnRequest);
        BaseServerResponse<IList<CodeBaseResponse>> AprStsNmSelect(int trnky, int aprstsky, int objky, Company company, User user);
        void TransactionHeaderApproveInsert(int trnky, int aprstsky, int objky, int isAct,string ourcd, Company company, User user);
        BaseServerResponse<TransactionPermission> CheckTranPrintPermission(int trnky, int aprstsky, int objky, int trnTypKy, Company company, User user);
        BaseServerResponse<CodeBaseResponse> TrnHdrNextApproveStatus(int aprstsky, int objky, int trnTypKy, Company company, User user);
        BaseServerResponse<TransactionPermission> GetIsALwAddUpdatePermissionForOrderTrn(Company company, User user, int objky = 1, int trnky = 1, int aprstsKy = 1);
        CodeBaseResponse TrnrApproveStatusFindByTrnKy(Company company, User user, int objky = 1, int trnky = 1);
        BaseServerResponse<IList<GenericTransactionLineItem>> GenericallyGetTransactionLineItemsV2(Company company, User user, TransactionOpenRequest request);
        RecviedAmountResponse GetRecviedAmountResponse(Company company, User user, RecieptDetailRequest request);
        BaseServerResponse<WorkOrderAmountByAccount> TrnDetailAccountInsertUpdate(Company company, User user, WorkOrderAmountByAccount accDet);
        BaseServerResponse<IList<WorkOrderAmountByAccount>> TransactionDetailAccountSelect(Company company, User user, WorkOrderAmountByAccount accDet);
    }
}
