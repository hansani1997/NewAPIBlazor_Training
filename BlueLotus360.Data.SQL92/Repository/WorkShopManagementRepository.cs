using BlueLotus360.Core.Domain.Definitions.Repository;
using BlueLotus360.Core.Domain.DTOs;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.BookingModule;
using BlueLotus360.Core.Domain.Entity.MastrerData;
using BlueLotus360.Core.Domain.Entity.WorkOrder;
using BlueLotus360.Core.Domain.Responses;
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
    internal class WorkShopManagementRepository : BaseRepository, IWorkShopManagementRepository
    {
        public WorkShopManagementRepository(ISQLDataLayer dataLayer):base(dataLayer) { }

        public BaseServerResponse<IList<Vehicle>> SelectVehicle(VehicleSearch dto, Company company, User user)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                IList<Vehicle> list = new List<Vehicle>();
                BaseServerResponse<IList<Vehicle>> response = new BaseServerResponse<IList<Vehicle>>();
                string SPName = "CARVehDetails_FindWeb";
                try
                {

                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    dbCommand.CreateAndAddParameter("CKy", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("ObjKy",dto.ObjectKey);
                    dbCommand.CreateAndAddParameter("VehAdrKy",BaseComboResponse.GetKeyValue(dto.VehicleRegistration));
                    dbCommand.CreateAndAddParameter("ItmKy",dto.VehicleSerialNumber!=null? dto.VehicleSerialNumber.ItemKey:1);
                    dbCommand.CreateAndAddParameter("CusAdrKy", BaseComboResponse.GetKeyValue(dto.RegisteredCustomer));
                   

                    response.ExecutionStarted = DateTime.UtcNow;
                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        Vehicle vehicle = new Vehicle();
                        vehicle.VehicleAddress = new AddressResponse();
                        vehicle.VehicleAddress.AddressKey = reader.GetColumn<int>("VehAdrKy");
                        vehicle.VehicleRegistration = new ItemResponse();
                        vehicle.VehicleRegistration.ItemKey= reader.GetColumn<int>("VehItmKy");
                        vehicle.VehicleRegistration.ItemCode= reader.GetColumn<string>("VehicleNo");
                        vehicle.RegisteredCustomer = new AddressMaster();
                        vehicle.RegisteredCustomer.AddressKey = reader.GetColumn<int>("CusAdrKy");
                        vehicle.RegisteredCustomer.AddressName = reader.GetColumn<string>("CusNm");
                        vehicle.RegisteredCustomer.NIC= reader.GetColumn<string>("NIC");
                        vehicle.RegisteredCustomer.AddressId= reader.GetColumn<string>("Mobile");
                        vehicle.RegisteredCustomer.Address = reader.GetColumn<string>("Address");
                        vehicle.RegisteredCustomer.Email= reader.GetColumn<string>("Email");
                        vehicle.SerialNumber=new ItemSerialNumber();
                        vehicle.SerialNumber.SerialNumber= reader.GetColumn<string>("ChassisNo");
                        vehicle.SerialNumber.EngineNumber= reader.GetColumn<string>("EnginNo");
                        vehicle.Brand = new CodeBaseResponse() { CodeKey = reader.GetColumn<int>("BrandKy") ,CodeName= reader.GetColumn<string>("Brand") };
                        vehicle.Model= new CodeBaseResponse() { CodeKey = reader.GetColumn<int>("ModelKy"), CodeName = reader.GetColumn<string>("Model") };
                        vehicle.Category=new CodeBaseResponse() { CodeKey = reader.GetColumn<int>("AdrCat1Ky"), CodeName = reader.GetColumn<string>("Category") };
                        vehicle.SubCategory = new CodeBaseResponse() { CodeKey = reader.GetColumn<int>("AdrCat2Ky"), CodeName = reader.GetColumn<string>("SubCategory") };
                        vehicle.Fuel= reader.GetColumn<string>("FuelTyp");
                        vehicle.PreviousMilage= reader.GetColumn<decimal>("Milage");
                        vehicle.VehicleRegisterDate = reader.GetColumn<DateTime>("RegDt");
                        vehicle.Province = new CodeBaseResponse() { CodeKey = reader.GetColumn<int>("AdrCat4Ky"), CodeName = reader.GetColumn<string>("ProvinceNm") };
                        list.Add(vehicle);
                    }
                    response.ExecutionEnded = DateTime.UtcNow;
                    response.Value = list;

                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }




                }
                catch (Exception exp)
                {
                    response.ExecutionEnded = DateTime.UtcNow;
                    response.Messages.Add(new ServerResponseMessae()
                    {
                        MessageType = ServerResponseMessageType.Exception,
                        Message = $"Error While Executing Proc {SPName}"
                    });
                    response.ExecutionException = exp;
                }

                finally
                {
                    IDbConnection dbConnection = dbCommand.Connection;
                    if (reader != null)
                    {
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                        }
                    }
                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }
                    if (reader!=null)
                    {
                        reader.Dispose();
                    }
                    
                    dbCommand.Dispose();
                    dbConnection.Dispose();

                }

                return response;
            }
        }

        public BaseServerResponse<IList<WorkOrder>> SelectJobhistory(Vehicle dto, Company company, User user)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                IList<WorkOrder> list = new List<WorkOrder>();
                BaseServerResponse<IList<WorkOrder>> response = new BaseServerResponse<IList<WorkOrder>>();
                string SPName = "CARJobHistory_SelectWeb";
                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    dbCommand.CreateAndAddParameter("CKy", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("ObjKy", dto.ObjectKey);
                    dbCommand.CreateAndAddParameter("AdrKy", BaseComboResponse.GetKeyValue(dto.VehicleAddress));
                    dbCommand.CreateAndAddParameter("isInsurance", Convert.ToInt32(dto.IsInsurence));

                    response.ExecutionStarted = DateTime.UtcNow;
                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        WorkOrder order = new WorkOrder();
                        order.OrderKey= reader.GetColumn<int>("OrdKy");
                        order.OrderNumber = reader.GetColumn<string>("OrdNo");
                        order.OrderType = new CodeBaseResponse();
                        order.OrderType.CodeName = reader.GetColumn<string>("Type");
                        order.OrderProject.ProjectEndDate = reader.GetColumn<DateTime>("FinDt");
                        order.OrderProject.ProjectStartDate = reader.GetColumn<DateTime>("PrjStDt");
                        order.OrderStatus = new CodeBaseResponse(); 
                        order.OrderStatus.CodeName = reader.GetColumn<string>("Status");
                        order.TrnKy= reader.GetColumn<int>("InvoiceTrnKy");
                        order.OrderCategory1 = new CodeBaseResponse() { CodeKey= reader.GetColumn<int>("OrdCat1Ky") ,CodeName= reader.GetColumn<string>("OrdCat1") };
                        order.WorkOrderSimpleEstimation=new Estimation() { EstimateKey= reader.GetColumn<int>("EstimateKy"),EstimationNumber= reader.GetColumn<string>("EstimateNo")??"" };
                        order.IsActive = reader.GetColumn<int>("isAct");
                        list.Add(order);
                    }
                    response.ExecutionEnded = DateTime.UtcNow;
                    response.Value = list;

                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }




                }
                catch (Exception exp)
                {
                    response.ExecutionEnded = DateTime.UtcNow;
                    response.Messages.Add(new ServerResponseMessae()
                    {
                        MessageType = ServerResponseMessageType.Exception,
                        Message = $"Error While Executing Proc {SPName}"
                    });
                    response.ExecutionException = exp;
                }

                finally
                {
                    IDbConnection dbConnection = dbCommand.Connection;
                    if (reader != null)
                    {
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                        }
                    }
                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }
                    if (reader != null)
                    {
                        reader.Dispose();
                    }

                    dbCommand.Dispose();
                    dbConnection.Dispose();

                }

                return response;
            }
        }

        public BaseServerResponse<IList<ProjectResponse>> SelectOngoingProjectDetails(Vehicle dto, Company company, User user)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                IList<ProjectResponse> list = new List<ProjectResponse>();
                BaseServerResponse<IList<ProjectResponse>> response = new BaseServerResponse<IList<ProjectResponse>>();
                string SPName = "CARProgressingPrjDetails_SelectWeb";
                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    dbCommand.CreateAndAddParameter("CKy", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("ObjKy", dto.ObjectKey);
                    dbCommand.CreateAndAddParameter("AdrKy", BaseComboResponse.GetKeyValue(dto.VehicleAddress));
                    dbCommand.CreateAndAddParameter("isInsurance", Convert.ToInt32(dto.IsInsurence));

                    response.ExecutionStarted = DateTime.UtcNow;
                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        ProjectResponse project = new ProjectResponse();
                        project.ProjectKey= reader.GetColumn<int>("PrjKy");
                        project.ProjectId = reader.GetColumn<string>("PrjID");
                        project.ProjectName = reader.GetColumn<string>("PrjNm");

                        list.Add(project);
                    }
                    response.ExecutionEnded = DateTime.UtcNow;
                    response.Value = list;

                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }




                }
                catch (Exception exp)
                {
                    response.ExecutionEnded = DateTime.UtcNow;
                    response.Messages.Add(new ServerResponseMessae()
                    {
                        MessageType = ServerResponseMessageType.Exception,
                        Message = $"Error While Executing Proc {SPName}"
                    });
                    response.ExecutionException = exp;
                }

                finally
                {
                    IDbConnection dbConnection = dbCommand.Connection;
                    if (reader != null)
                    {
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                        }
                    }
                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }
                    if (reader != null)
                    {
                        reader.Dispose();
                    }

                    dbCommand.Dispose();
                    dbConnection.Dispose();

                }

                return response;
            }
        }

        public BaseServerResponse<IList<BookingDetails>> RecentBookingDetails(Vehicle dto, Company company, User user)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                IList<BookingDetails> list = new List<BookingDetails>();
                BaseServerResponse<IList<BookingDetails>> response = new BaseServerResponse<IList<BookingDetails>>();
                string SPName = "CARRecentBookings_SelectWeb";
                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    dbCommand.CreateAndAddParameter("CKy", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("ObjKy", dto.ObjectKey);
                    dbCommand.CreateAndAddParameter("Dt", DateTime.Now);
                    dbCommand.CreateAndAddParameter("AdrKy", BaseComboResponse.GetKeyValue(dto.VehicleAddress));

                    response.ExecutionStarted = DateTime.UtcNow;
                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        BookingDetails book = new BookingDetails();
                        book.ProcessDetailsKey= reader.GetColumn<int>("PrcsDetKy");
                        book.ProjectKey = reader.GetColumn<int>("PrjKy");
                        book.TaskID = reader.GetColumn<int>("TaskID");
                        book.TaskName = reader.GetColumn<string>("TaskSNm");
                        book.FromDate = reader.GetColumn<DateTime>("FrmDt");
                        book.ToDate = reader.GetColumn<DateTime>("ToDt");
                        list.Add(book);
                    }
                    response.ExecutionEnded = DateTime.UtcNow;
                    response.Value = list;

                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }




                }
                catch (Exception exp)
                {
                    response.ExecutionEnded = DateTime.UtcNow;
                    response.Messages.Add(new ServerResponseMessae()
                    {
                        MessageType = ServerResponseMessageType.Exception,
                        Message = $"Error While Executing Proc {SPName}"
                    });
                    response.ExecutionException = exp;
                }

                finally
                {
                    IDbConnection dbConnection = dbCommand.Connection;
                    if (reader != null)
                    {
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                        }
                    }
                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }
                    if (reader != null)
                    {
                        reader.Dispose();
                    }

                    dbCommand.Dispose();
                    dbConnection.Dispose();

                }

                return response;
            }
        }

        public UserRequestValidation WorkOrderValidation(WorkOrder dto, Company company, User user)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                UserRequestValidation response = new UserRequestValidation();
                string SPName = "CARWorkshop_ValidateWeb";

                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    dbCommand.CreateAndAddParameter("CKy", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("ObjKy", dto.FormObjectKey);
                    dbCommand.CreateAndAddParameter("MeterReading", dto.MeterReading);
                    dbCommand.CreateAndAddParameter("OrdKy", dto.OrderKey==1?null : dto.OrderKey);
                    dbCommand.CreateAndAddParameter("VehAdrKy",BaseComboResponse.GetKeyValue(dto.SelectedVehicle.VehicleAddress));

                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        response.IsError= reader.GetColumn<bool>("isError");
                        response.Message = reader.GetColumn<string>("Message");

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
                        }
                    }
                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }
                    if (reader != null)
                    {
                        reader.Dispose();
                    }

                    dbCommand.Dispose();
                    dbConnection.Dispose();

                }

                return response;
            }
        }

        public BaseServerResponse<IList<IRNResponse>> SelectIRNBasedOnStatus(WorkOrder dto, Company company, User user)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                IList<IRNResponse> list = new List<IRNResponse>();
                BaseServerResponse<IList<IRNResponse>> response = new BaseServerResponse<IList<IRNResponse>>();
                string SPName = "CarOrdDetailsByOrdSts_SelectWeb";
                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    dbCommand.CreateAndAddParameter("CKy", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("ObjKy", dto.FormObjectKey);
                    dbCommand.CreateAndAddParameter("OrdStsKy ", BaseComboResponse.GetKeyValue(dto.OrderStatus));
                    dbCommand.CreateAndAddParameter("OrdTypKy ", BaseComboResponse.GetKeyValue(dto.OrderType));

                    response.ExecutionStarted = DateTime.UtcNow;
                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        IRNResponse order = new IRNResponse();
                        order.OrderKey = reader.GetColumn<int>("OrdKy");
                        order.OrderNumber = reader.GetColumn<string>("OrdNo");
                        order.IRNType.CodeName = reader.GetColumn<string>("OrdCat2");
                        order.VehicleID = reader.GetColumn<string>("VehAdrID");
                        order.ServiceAdvisor.AddressName= reader.GetColumn<string>("RepAdrNm");
                        order.BusinessUnit.CodeName= reader.GetColumn<string>("BU");
                        order.Insertdate= reader.GetColumn<DateTime>("InsertDt");
                        order.Item.ItemKey= reader.GetColumn<int>("ItmKy");
                        order.Item.ItemName = reader.GetColumn<string>("ItmNm");
                        order.Item.ItemCode = reader.GetColumn<string>("ItmCd");
                        order.IsActive = reader.GetColumn<int>("isAct");
                        order.Quantity = reader.GetColumn<decimal>("Qty");
                        order.Rate = reader.GetColumn<decimal>("TrnPri");
                        order.Amount = reader.GetColumn<decimal>("Amt");
                        order.Insurance.AccountKey = reader.GetColumn<int>("AccKy");
                        order.Insurance.AccountName = reader.GetColumn<string>("AccNm");
                        order.HederIsActive = reader.GetColumn<int>("HdrAct");
                        order.OrderLocation = this.GetCdMasByCdKy(reader.GetColumn<int>("LocKy2"));
						//order.DisocuntAmount = reader.GetColumn<decimal>("DisAmt");
						//order.DiscountPercentage = reader.GetColumn<decimal>("DisPer");
						//order.AnalysisType1 = this.GetCdMasByCdKy(reader.GetColumn<int>("Anl2Ky"));
						order.TransactionUnit = new UnitResponse();
						list.Add(order);
                    }
                    response.ExecutionEnded = DateTime.UtcNow;
                    response.Value = list;

                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }




                }
                catch (Exception exp)
                {
                    response.ExecutionEnded = DateTime.UtcNow;
                    response.Messages.Add(new ServerResponseMessae()
                    {
                        MessageType = ServerResponseMessageType.Exception,
                        Message = $"Error While Executing Proc {SPName}"
                    });
                    response.ExecutionException = exp;
                }

                finally
                {
                    IDbConnection dbConnection = dbCommand.Connection;
                    if (reader != null)
                    {
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                        }
                    }
                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }
                    if (reader != null)
                    {
                        reader.Dispose();
                    }

                    dbCommand.Dispose();
                    dbConnection.Dispose();

                }

                return response;
            }
        }
		public CodeBaseResponse GetCdMasByCdKy(int cdKy)
		{
			using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
			{
				IDataReader dataReader = null;
				CodeBaseResponse codebase = new CodeBaseResponse();
				string SPName = "GetCdMasBy_CdKy";
				try
				{
					dbCommand.CommandType = CommandType.StoredProcedure;
					dbCommand.CommandText = SPName;

					dbCommand.CreateAndAddParameter("@CdKy", cdKy);


					dbCommand.Connection.Open();
					dataReader = dbCommand.ExecuteReader();

					while (dataReader.Read())
					{
						codebase.Code = dataReader.GetColumn<string>("Code");
						codebase.CodeName = dataReader.GetColumn<string>("CdNm");
						codebase.ConditionCode = dataReader.GetColumn<string>("ConCd");
						codebase.CodeKey = dataReader.GetColumn<int>("CdKy");
					}


				}
				catch (Exception exp)
				{
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
						dataReader.Dispose();
					}
					if (dbConnection.State != ConnectionState.Closed)
					{
						dbConnection.Close();
					}

					dbCommand.Dispose();
					dbConnection.Dispose();
				}

				return codebase;

			}
		}

	}
}
