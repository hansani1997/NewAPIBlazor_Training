using BlueLotus360.Core.Domain.Definitions.Repository;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.MastrerData;
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
    internal class ProjectRepository : BaseRepository, IProjectRepository
    {
        public ProjectRepository(ISQLDataLayer dataLayer) : base(dataLayer) { }

        public ProjectResponse CreateProjectHeader(Company company, User user, Project request)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                ProjectResponse response = new ProjectResponse();
                string SPName = "PrjHdr_InsertWeb";
                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;

                    dbCommand.CreateAndAddParameter("@Cky", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("@UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("@PrjNo", request.ProjectNumber ??"");
                    dbCommand.CreateAndAddParameter("@PrjID", request.ProjectID ?? "");
                    if (!string.IsNullOrEmpty(request.ProjectName))
                    {
                        dbCommand.CreateAndAddParameter("@PrjNm", request.ProjectName);
                    }
                    dbCommand.CreateAndAddParameter("@PrjTypKy", BaseComboResponse.GetKeyValue(request.ProjectType));
                    dbCommand.CreateAndAddParameter("@PrntKy", request.ParentKey);
                    dbCommand.CreateAndAddParameter("@Alias", request.Alias??"");
                    dbCommand.CreateAndAddParameter("@ItmKy", request.Item.ItemKey);
                    dbCommand.CreateAndAddParameter("@PrjStsKy", BaseComboResponse.GetKeyValue(request.ProjectStatus));
                    dbCommand.CreateAndAddParameter("@isAct", request.IsActive);
                    dbCommand.CreateAndAddParameter("@isApr",request.IsApproved);
                    dbCommand.CreateAndAddParameter("@isAlwTrn", request.IsAllowTransaction);
                    dbCommand.CreateAndAddParameter("@isPrnt", request.IsParent);
                    dbCommand.CreateAndAddParameter("@PrjStDt", request.ProjectStartDate);
                    dbCommand.CreateAndAddParameter("@FinDt", request.ProjectEndDate);
                    dbCommand.CreateAndAddParameter("@Adrky", BaseComboResponse.GetKeyValue(request.Address));

                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();

                    
                    while (reader.Read())
                    {
                        response.ProjectKey= reader.GetColumn<int>("PrjKy");
                    }

                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                    

                }
                catch (Exception exp)
                {
                    throw exp;
                }
                finally
                {
                    IDbConnection dbConnection = dbCommand.Connection;
                    if (reader != null)
                    {
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                        };
                    }
                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }
                    reader.Dispose();
                    dbCommand.Dispose();
                    dbConnection.Dispose();
                }
                return response;
            }
        }

        public ProjectResponse UpdateProjectHeader(Company company, User user, Project request)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                ProjectResponse response = new ProjectResponse();
                string SPName = "PrjHdr_UpdateWeb";
                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;

                    //dbCommand.CreateAndAddParameter("@Cky", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("@UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("@PrjPrefixKy", BaseComboResponse.GetKeyValue(request.ProjectPrefix));
                    dbCommand.CreateAndAddParameter("@PrjTypKy", BaseComboResponse.GetKeyValue(request.ProjectType));
                    dbCommand.CreateAndAddParameter("@PrjNo", request.ProjectNumber ?? "");
                    dbCommand.CreateAndAddParameter("@PrjID", request.ProjectID ?? "");
                    dbCommand.CreateAndAddParameter("@PrjNm", request.ProjectName??"");
                    dbCommand.CreateAndAddParameter("@YurRef", request.YourReference??"");
                    dbCommand.CreateAndAddParameter("@YurRefDt", request.YourReferenceDate);
                    dbCommand.CreateAndAddParameter("@AccKy",BaseComboResponse.GetKeyValue(request.Account));
                    dbCommand.CreateAndAddParameter("@AdrKy", BaseComboResponse.GetKeyValue(request.Address));
                    dbCommand.CreateAndAddParameter("@PrjDt", request.ProjectStartDate);     
                    dbCommand.CreateAndAddParameter("@PrjStsKy", BaseComboResponse.GetKeyValue(request.ProjectStatus));
                    dbCommand.CreateAndAddParameter("@isPrnt", request.IsPrint);
                    dbCommand.CreateAndAddParameter("@isAct", request.IsActive);
                    dbCommand.CreateAndAddParameter("@isApr", request.IsApproved);
                    dbCommand.CreateAndAddParameter("@AcsLvlKy", request.AccessLevelKey);
                    dbCommand.CreateAndAddParameter("@ConFinLvlKy", request.ConFinLevelKey);
                    dbCommand.CreateAndAddParameter("@Original_PrjKy", request.OriginalProjectKey);
                    byte[] byteArray = StringToByteArray(request.TimeStamp ?? "");
                    dbCommand.CreateAndAddParameter("@Original_TmStmp", byteArray);
                    dbCommand.CreateAndAddParameter("@PrjKy", request.ProjectKey);
                    dbCommand.CreateAndAddParameter("@PrjStDt", request.ProjectStartDate);
                    dbCommand.CreateAndAddParameter("@PlnStDt", request.PlanStartDate);
                    dbCommand.CreateAndAddParameter("@PlnFinDt", request.PlanFinishDate);
                    dbCommand.CreateAndAddParameter("@ExpiryDt", request.ExpiryDate);
                    dbCommand.CreateAndAddParameter("@RepAdrKy", BaseComboResponse.GetKeyValue(request.ProjectRep));
                    dbCommand.CreateAndAddParameter("@BUKy", BaseComboResponse.GetKeyValue(request.BusinessUnit));              
                    dbCommand.CreateAndAddParameter("@LocKy", BaseComboResponse.GetKeyValue(request.Location));

                    dbCommand.CreateAndAddParameter("@DisPer", 0);
                    dbCommand.CreateAndAddParameter("@CtrtAmt", 0);
                    dbCommand.CreateAndAddParameter("@ContegencyAmt", 0);
                    dbCommand.CreateAndAddParameter("@LiqdRate", 0);
                    dbCommand.CreateAndAddParameter("@Amt1", 0);
                    dbCommand.CreateAndAddParameter("@Amt2", 0);
                    dbCommand.CreateAndAddParameter("@MarPer",0 );
                    dbCommand.CreateAndAddParameter("@Des", request.Description ?? "");
                    dbCommand.CreateAndAddParameter("@Rem", request.Remark ?? "");
                    dbCommand.CreateAndAddParameter("@RetnPer",0 );
                    dbCommand.CreateAndAddParameter("@MaxRetnPer", 0);
                    dbCommand.CreateAndAddParameter("@RetnPeriod", 0);
                    dbCommand.CreateAndAddParameter("@RetnDays", 0);
                    dbCommand.CreateAndAddParameter("@PlnYY", 0);
                    dbCommand.CreateAndAddParameter("@PlnMM", 0);
                    dbCommand.CreateAndAddParameter("@PlnDD", 0);
                    dbCommand.CreateAndAddParameter("@PmtLagDay", 0);
                    dbCommand.CreateAndAddParameter("@FinancePer", 0);
                    dbCommand.CreateAndAddParameter("@AdvPer", 0);
                    dbCommand.CreateAndAddParameter("@AdvDedPer", 0);
                    dbCommand.CreateAndAddParameter("@RetnPeriodUnitKy", 1);
                    dbCommand.CreateAndAddParameter("@Guarantee", 0);
                    dbCommand.CreateAndAddParameter("@CostAdvBond", 0);
                    dbCommand.CreateAndAddParameter("@CostPerfBond",0 );
                    dbCommand.CreateAndAddParameter("@Tax1", 0);
                    dbCommand.CreateAndAddParameter("@Tax2", 0);
                    dbCommand.CreateAndAddParameter("@Tax3",0 );
                    dbCommand.CreateAndAddParameter("@FinDt", request.ProjectEndDate);
                    dbCommand.CreateAndAddParameter("@PrjSTypKy", 1);
                    dbCommand.CreateAndAddParameter("@PrjCat1Ky", 1);
                    dbCommand.CreateAndAddParameter("@PrjCat2Ky", 1);
                    dbCommand.CreateAndAddParameter("@PrjCat3Ky", 1);
                    dbCommand.CreateAndAddParameter("@PrjCat4Ky", 1);
                    dbCommand.CreateAndAddParameter("@PrjCat5Ky", 1);
                    dbCommand.CreateAndAddParameter("@PrjCat6Ky", 1);
                    dbCommand.CreateAndAddParameter("@PrjCat7Ky", 1);
                    dbCommand.CreateAndAddParameter("@PrjCat8Ky", 1);
                    dbCommand.CreateAndAddParameter("@PrjCat9Ky", 1);
                    dbCommand.CreateAndAddParameter("@PrjCat10Ky", 1);
                    dbCommand.CreateAndAddParameter("@PrjCat11Ky", 1);
                    dbCommand.CreateAndAddParameter("@PrjCat12Ky", 1);
                    dbCommand.CreateAndAddParameter("@PrntKy", request.ParentKey);
                    dbCommand.CreateAndAddParameter("@SO", 0);
                    dbCommand.CreateAndAddParameter("@Acres", 0);
                    dbCommand.CreateAndAddParameter("@Root", 0);
                    dbCommand.CreateAndAddParameter("@Perch", 0);
                    dbCommand.CreateAndAddParameter("@AdrKy2", BaseComboResponse.GetKeyValue(request.Address2));
                    dbCommand.CreateAndAddParameter("@North", "");
                    dbCommand.CreateAndAddParameter("@East", "");
                    dbCommand.CreateAndAddParameter("@West", "");
                    dbCommand.CreateAndAddParameter("@South", "");
                    dbCommand.CreateAndAddParameter("@PermitNo", "");
                    dbCommand.CreateAndAddParameter("@DeedNo", "");
                    dbCommand.CreateAndAddParameter("@PermitDate", DateTime.Now);
                    dbCommand.CreateAndAddParameter("@DeedDate", DateTime.Now);
                    dbCommand.CreateAndAddParameter("@BrnKy", request.BranchKey);
                    dbCommand.CreateAndAddParameter("@BnkKy", request.BankKey);
                    dbCommand.CreateAndAddParameter("@Distance", 0);
                    dbCommand.CreateAndAddParameter("@DistanceUnitKy", 1);
                    dbCommand.CreateAndAddParameter("@CalTypKy", 1);
                    dbCommand.CreateAndAddParameter("@StChainage", 0);
                    dbCommand.CreateAndAddParameter("@ToChainage", 0);
                    dbCommand.CreateAndAddParameter("@YurRepAdrKy", 1);
                    dbCommand.CreateAndAddParameter("@isAlwTrn", request.IsAllowTransaction);
                    dbCommand.CreateAndAddParameter("@ItmKy", 1);
                    dbCommand.CreateAndAddParameter("@SerNoKy", 1);
                    dbCommand.CreateAndAddParameter("@MeterReaing", 0);
                    dbCommand.CreateAndAddParameter("@PriCtrlLocKy", 1);
                    dbCommand.CreateAndAddParameter("@WIPLocKy", 1);
                    dbCommand.CreateAndAddParameter("@SiteStrLocKy", 1);
                    dbCommand.CreateAndAddParameter("@isSysRec", 0);
                    dbCommand.CreateAndAddParameter("@Alias", "");
                    dbCommand.CreateAndAddParameter("@isBOM", 1);
                    dbCommand.CreateAndAddParameter("@ResConsumDt", DateTime.Now);
                                    
                    
                    
                    

                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();


                    while (reader.Read())
                    {
                        response.ProjectKey = reader.GetColumn<int>("PrjKy");
                    }

                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }


                }
                catch (Exception exp)
                {
                    throw exp;
                }
                finally
                {
                    IDbConnection dbConnection = dbCommand.Connection;
                    if (reader != null)
                    {
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                        };
                    }
                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }
                    reader.Dispose();
                    dbCommand.Dispose();
                    dbConnection.Dispose();
                }
                return response;
            }
        }

        public Project SelectProjectHeader(Company company, User user, ProjectOpenRequest request)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                Project response = new Project();
                string SPName = "PrjHdr_SelectWeb";
                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;

                    dbCommand.CreateAndAddParameter("@Cky", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("@UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("@PrjKy", request.ProjectKey);
                    if (!string.IsNullOrEmpty(request.ProjectName))
                    {
                        dbCommand.CreateAndAddParameter("@PrjNm", request.ProjectName);
                    }
                    dbCommand.CreateAndAddParameter("@PrjTypKy", BaseComboResponse.GetKeyValue(request.ProjectType));
                    dbCommand.CreateAndAddParameter("@ObjKy", request.ObjectKey);
                    dbCommand.CreateAndAddParameter("@FrmRow", request.FromRow);
                    dbCommand.CreateAndAddParameter("@ToRow", request.ToRow);
                    dbCommand.CreateAndAddParameter("@PrjCd", request.ProjectCd??"");

                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();


                    while (reader.Read())
                    {
                        response.ProjectKey = reader.GetColumn<int>("PrjKy");
                        response.ProjectNumber= reader.GetColumn<string>("PrjNo")??"";
                        response.ProjectName= reader.GetColumn<string>("PrjNm")??"";
                        response.ProjectID= reader.GetColumn<string>("PrjID") ?? "";
                        response.ProjectType = new CodeBaseResponse() {CodeKey= reader.GetColumn<int>("PrjTypKy")  };
                        response.Account = new AccountResponse() { AccountKey= reader.GetColumn<int>("AccKy") };
                        response.Address = new AddressResponse() { AddressKey = reader.GetColumn<int>("AdrKy") };
                        response.ProjectStartDate= reader.GetColumn<DateTime>("PrjStDt");
                        response.ProjectEndDate = reader.GetColumn<DateTime>("FinDt");
                        response.ProjectStatus = new CodeBaseResponse() { CodeKey=reader.GetColumn<int>("PrjStsKy"),CodeName=reader.GetColumn<string>("PrjStsNm"),Code= reader.GetColumn<string>("PrjStsCd") };
                        
                        byte[] byteArray = reader.GetColumn<byte[]>("TmStmp");
                        string TimeStamp = ByteArrayToString(byteArray).ToUpper();
                        response.TimeStamp = TimeStamp;
                        response.IsActive= reader.GetColumn<int>("isAct");
                        response.AccessLevelKey = reader.GetColumn<int>("AcsLvlKy");
                        response.ConFinLevelKey = reader.GetColumn<int>("ConFinLvlKy");
                        response.AccessLevelKey = reader.GetColumn<int>("AcsLvlKy");
                        response.Alias= reader.GetColumn<string>("Alias")??"";
                        response.YourReference = reader.GetColumn<string>("YurRef")??"";

                    }

                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }


                }
                catch (Exception exp)
                {
                    throw exp;
                }
                finally
                {
                    IDbConnection dbConnection = dbCommand.Connection;
                    if (reader != null)
                    {
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                        };
                    }
                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }
                    reader.Dispose();
                    dbCommand.Dispose();
                    dbConnection.Dispose();
                }
                return response;
            }
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }



        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}
