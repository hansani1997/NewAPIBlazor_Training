using BlueLotus360.Core.Domain.Definitions.Repository;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.Document;
using BlueLotus360.Data.SQL92.Definition;
using BlueLotus360.Data.SQL92.Extenstions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Data.SQL92.Repository
{
    internal class DocumentRepository : BaseRepository, IDocumentRepository
    {
        public DocumentRepository(ISQLDataLayer dataLayer) : base(dataLayer) { }

        public CodeBaseResponse GetDocumentTypeByContentType(Company company, User user, string contentType)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {

                IDataReader dataReader = null;
                string SPName = "CdMas_CdExtChr1DoctypeSelectWeb";
                try
                {

                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    dbCommand.CreateAndAddParameter("@CKy", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("@CdExtChr1", contentType);
                    dbCommand.CreateAndAddParameter("@UsrKy", user.UserKey);

                    CodeBaseResponse codebase = new CodeBaseResponse();

                    dbCommand.Connection.Open();
                    dataReader = dbCommand.ExecuteReader();

                    while (dataReader.Read())
                    {
                        codebase.CodeKey = dataReader.GetColumn<int>("CdKy");
                        codebase.OurCode = dataReader.GetColumn<string>("OurCd");
                        codebase.CodeName = dataReader.GetColumn<string>("CdNm");
                    }


                    return codebase;

                }
                catch (Exception exp)
                {
                    throw exp;
                }

                finally
                {
                    IDbConnection dbConnection = dbCommand.Connection;
                    if (dataReader != null)
                    {
                        if (!dataReader.IsClosed)
                        {
                            dataReader.Close();
                        }
                    }
                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }
                    dataReader.Dispose();
                    dbCommand.Dispose();
                    dbConnection.Dispose();
                }

            }
        }

        public void SaveUploadFileForTransaction(Company company, User user, BinaryDocument binaryDocument)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {

                IDataReader dataReader = null;
                string SPName = "UploadTransactionFile";
                try
                {
                    IList<CodeBaseResponse> codeBases = new List<CodeBaseResponse>();
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    dbCommand.CreateAndAddParameter("@CKy", binaryDocument.CompanyKey);
                    dbCommand.CreateAndAddParameter("@UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("@TrnKy", binaryDocument.TransactionKey);
                    dbCommand.CreateAndAddParameter("@DocTypKy",BaseComboResponse.GetKeyValue(binaryDocument.DocumentType));
                    dbCommand.CreateAndAddParameter("@Des", binaryDocument.Description??"");
                    dbCommand.CreateAndAddParameter("@Keywords", binaryDocument.Keyword);
                    dbCommand.CreateAndAddParameter("@FileNm", binaryDocument.Filename??"");
                    dbCommand.CreateAndAddParameter("@FileContent", binaryDocument.DocumentArray);
                    dbCommand.CreateAndAddParameter("@AdrKy", binaryDocument.AddressKey);
                    dbCommand.CreateAndAddParameter("@ItmKy", binaryDocument.ItemKey);
                    dbCommand.CreateAndAddParameter("@PrcsDetKy", binaryDocument.ProcessDetKey);
                    dbCommand.CreateAndAddParameter("@PrjKy", binaryDocument.ProjectKey);
                    dbCommand.CreateAndAddParameter("@FileSize", binaryDocument.FileSize);
                    dbCommand.CreateAndAddParameter("@EmpCdKy", binaryDocument.EmployeeCodeKey);
                    dbCommand.CreateAndAddParameter("@EmpCdDtKy", binaryDocument.EmployeeCodeDtKey);
                    dbCommand.CreateAndAddParameter("@CdKy", binaryDocument.CdKey);
                    dbCommand.CreateAndAddParameter("@OrdKy", binaryDocument.OrderKey);
                    dbCommand.CreateAndAddParameter("@OrdDetKy", binaryDocument.OrderDetailKey);
                    dbCommand.CreateAndAddParameter("@BUKy", binaryDocument.BuKey);
                    dbCommand.CreateAndAddParameter("@ItmTrnKy", binaryDocument.ItemTranKey);
                    dbCommand.Connection.Open();
                    dataReader = dbCommand.ExecuteReader();

                    while (dataReader.Read())
                    {
                        binaryDocument.DocumentKey = dataReader.GetColumn<int>("DocKy");

                    }



                }
                catch (Exception exp)
                {
                    throw exp;
                }

                finally
                {
                    IDbConnection dbConnection = dbCommand.Connection;
                    if (dataReader != null)
                    {
                        if (!dataReader.IsClosed)
                        {
                            dataReader.Close();
                        }
                    }
                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }
                    dataReader.Dispose();
                    dbCommand.Dispose();
                    dbConnection.Dispose();
                }

            }

        }

        public IList<Base64Document> GetBase64Documents(Company company, User user, DocumentRetrivaltDTO documentRetrivaltDTO)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {

                IDataReader dataReader = null;
                string SPName = "DocMasV3_SelectWebFullEx";
                try
                {
                    IList<Base64Document> base64Documents = new List<Base64Document>();

                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    dbCommand.CreateAndAddParameter("@Cky", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("@UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("@TrnKy", documentRetrivaltDTO.TransactionKey);
                    dbCommand.CreateAndAddParameter("@ItmKy", documentRetrivaltDTO.ItemKey);
                    dbCommand.CreateAndAddParameter("@OrdKy", documentRetrivaltDTO.OrderKey);
                    dbCommand.CreateAndAddParameter("@PrjKy", documentRetrivaltDTO.ProjectKey);
                    dbCommand.CreateAndAddParameter("@DocTypKy", documentRetrivaltDTO.DocumentTypeKey);
                    dbCommand.CreateAndAddParameter("@EmpCdKy", documentRetrivaltDTO.EmployeeCodeKey);
                    dbCommand.CreateAndAddParameter("@EmpCdDtKy", documentRetrivaltDTO.EmployeeCodeDtKey);
                    dbCommand.CreateAndAddParameter("@ItmTrnKy", documentRetrivaltDTO.ItemTransactionKey);
                    dbCommand.CreateAndAddParameter("@OrdDetKy", documentRetrivaltDTO.OrderDetailKey);
                    dbCommand.CreateAndAddParameter("@PrcsDetKy", documentRetrivaltDTO.ProcessDetKey);
                    dbCommand.CreateAndAddParameter("@AdrKy", documentRetrivaltDTO.AdrKy);



                    dbCommand.Connection.Open();
                    dataReader = dbCommand.ExecuteReader();

                    while (dataReader.Read())
                    {
                        Base64Document baseDocument = new Base64Document();
                        baseDocument.DocumentType = new CodeBaseResponse()
                        {
                            CodeKey = dataReader.GetColumn<int>("DocTypKy"),
                            //CdExtraInformation1 = dataReader.GetColumn<string>("CdExtChar1")
                        };
                        baseDocument.Filename = dataReader.GetColumn<string>("FileNm");
                        baseDocument.Base64Source = Convert.ToBase64String(dataReader.GetColumn<byte[]>("FileStream"));
                        base64Documents.Add(baseDocument);

                    }


                    return base64Documents;

                }
                catch (Exception exp)
                {
                    throw exp;
                }

                finally
                {
                    IDbConnection dbConnection = dbCommand.Connection;
                    if (dataReader != null)
                    {
                        if (!dataReader.IsClosed)
                        {
                            dataReader.Close();
                        }
                    }
                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }
                    dataReader.Dispose();
                    dbCommand.Dispose();
                    dbConnection.Dispose();
                }

            }
        }
    }
}
