using BlueLotus360.Core.Domain.Definitions.DataLayer;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.Document;
using BlueLotus360.Web.APIApplication.Definitions.ServiceDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Web.APIApplication.Services
{
    public class DocumentService:IDocumentService
    {
        public readonly IUnitOfWork _unitOfWork;
        public DocumentService(IUnitOfWork unitOfWork) 
        { 
            _unitOfWork = unitOfWork;
        }

        public void UploadImages(Company company,User user, FileUpload document)
        {
            BinaryDocument binaryDocument = new BinaryDocument();
            binaryDocument.DocumentType = _unitOfWork.DocumentRepository.GetDocumentTypeByContentType(company,user, document.UploadedFile.Extension);
            binaryDocument.AddressKey = document.AdrKy;
            binaryDocument.Category = new CodeBaseResponse() { CodeKey = document.DocCategory1Key };
            binaryDocument.Description = document.Description;
            binaryDocument.Filename = document.UploadedFile.FileName;
            binaryDocument.Extention = binaryDocument.DocumentType.OurCode;
            binaryDocument.DocumentArray = document.Buffer;
            binaryDocument.ProcessDetKey = document.ProcessDetKey;
            binaryDocument.ProjectKey = document.ProjectKey;
            binaryDocument.ProjectStatusKey = document.ProjectStatusKey;
            binaryDocument.TransactionKey = document.TransactionKey;
            binaryDocument.ItemKey = (int)document.ItemKey;
            binaryDocument.ItemTranKey = document.ItemTransactionKey;
            binaryDocument.NextActionDate = document.NextActionDate;
            binaryDocument.OrderKey = document.OrderKey;
            binaryDocument.EmployeeCodeKey = document.EmployeeCodeKey;
            binaryDocument.EmployeeCodeDtKey = document.EmployeeCodeDtKey;
            binaryDocument.CdKey = document.CdKey;
            binaryDocument.BuKey = document.BUKey;
            binaryDocument.OrderDetailKey = document.OrderDetailKey;
            binaryDocument.FileSize = document.UploadedFile.Size;
            binaryDocument.Keyword = document.KeyWord;
            _unitOfWork.DocumentRepository.SaveUploadFileForTransaction(company,user,binaryDocument);
        }

        public IList<Base64Document> GetBase64Documents(Company company, User user, DocumentRetrivaltDTO document)
        {
            return _unitOfWork.DocumentRepository.GetBase64Documents(company,user,document);
        }
    }
}
