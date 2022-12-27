using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Core.Domain.Definitions.Repository
{
    public interface IDocumentRepository
    {
        CodeBaseResponse GetDocumentTypeByContentType(Company company, User user, string contentType);
        void SaveUploadFileForTransaction(Company company, User user, BinaryDocument binaryDocument);
        IList<Base64Document> GetBase64Documents(Company company, User user, DocumentRetrivaltDTO documentRetrivaltDTO);
    }
}
