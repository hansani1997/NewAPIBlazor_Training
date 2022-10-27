﻿using BlueLotus360.Core.Domain.Definitions.Repository;
using BlueLotus360.Core.Domain.DTOs;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.Order;
using BlueLotus360.Core.Domain.Entity.Transaction;
using BlueLotus360.Core.Domain.Responses;
using BlueLotus360.Data.SQL92.Definition;
using BlueLotus360.Data.SQL92.Extenstions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Data.SQL92.Repository
{
    internal class OrderRepository : BaseRepository, IOrderRepository
    {
        public OrderRepository(ISQLDataLayer dataLayer):base(dataLayer)
        {

        }

        public void CreateOrder(OrderHeaderCreateDTO orderHeader, Company company, User user)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                string SPName = "OrdHdr_InsertWeb";
                BaseServerResponse<OrderHeaderCreateDTO> response = new BaseServerResponse<OrderHeaderCreateDTO>();
                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    dbCommand.CreateAndAddParameter("CKy", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("OrdDt", orderHeader.OrderDate);
                    dbCommand.CreateAndAddParameter("OrdTypKy", BaseComboResponse.GetKeyValue(orderHeader.OrderType));
                    dbCommand.CreateAndAddParameter("PmtTrmKy", BaseComboResponse.GetKeyValue(orderHeader.PayementTerm));
                    dbCommand.CreateAndAddParameter("DocNo", orderHeader.DocumentNumber);
                    dbCommand.CreateAndAddParameter("YurRef", orderHeader.YourReference);
                    dbCommand.CreateAndAddParameter("YurRefDt", orderHeader.YourReferenceDate);
                    dbCommand.CreateAndAddParameter("Amt", orderHeader.Amount);
                    dbCommand.CreateAndAddParameter("TrnAmt", orderHeader.TransactionAmount);
                    dbCommand.CreateAndAddParameter("TrnCrnKy", orderHeader.TransactionCurrencyKey);
                    dbCommand.CreateAndAddParameter("TrnExRate", orderHeader.TransactionExchangeRate);
                    dbCommand.CreateAndAddParameter("DisPer", orderHeader.DiscountPercentage);
                    dbCommand.CreateAndAddParameter("DisAmt", orderHeader.DiscountAmount);
                    dbCommand.CreateAndAddParameter("TrnDisAmt", orderHeader.TransactionDisountAmount);
                    dbCommand.CreateAndAddParameter("MarkUpPer", orderHeader.MarkupPercentage);
                    dbCommand.CreateAndAddParameter("RetnPer", orderHeader.RetensionPercentage);
                    dbCommand.CreateAndAddParameter("BillFrqKy", orderHeader.BillFerquencyKey);
                    dbCommand.CreateAndAddParameter("BillToPmt", orderHeader.BillToPayement);
                    dbCommand.CreateAndAddParameter("LocAdrKy", orderHeader.LocationAddressKey);
                    dbCommand.CreateAndAddParameter("IsOrdSetOff", orderHeader.IsOrderSetOff);
                    dbCommand.CreateAndAddParameter("Des", orderHeader.Description);
                    dbCommand.CreateAndAddParameter("Rem", orderHeader.Remarks);
                    dbCommand.CreateAndAddParameter("OrdFrqKy", orderHeader.OrderFerquencyKey);
                    dbCommand.CreateAndAddParameter("OrdStsKy", orderHeader.OrderStatusKey);
                    dbCommand.CreateAndAddParameter("AdrKy", orderHeader.AddressKey);
                    dbCommand.CreateAndAddParameter("RepAdrKy", orderHeader.RepAddessKey);
                    dbCommand.CreateAndAddParameter("DistAdrKy", orderHeader.DistributerAddressKey);
                    dbCommand.CreateAndAddParameter("DlryDt", orderHeader.DeliveryDate);
                    dbCommand.CreateAndAddParameter("OrdFinDt", orderHeader.OrderFinishDate);
                    dbCommand.CreateAndAddParameter("CusItmKy", orderHeader.CustomItemKey);
                    dbCommand.CreateAndAddParameter("IsAct", orderHeader.IsActive);
                    dbCommand.CreateAndAddParameter("IsApr", orderHeader.IsApproved);
                    dbCommand.CreateAndAddParameter("IsPrinted", orderHeader.IsPrinted);
                    dbCommand.CreateAndAddParameter("IsLocked", orderHeader.IsLocked);
                    dbCommand.CreateAndAddParameter("AcsLvlKy", orderHeader.AccessLevelKey);
                    dbCommand.CreateAndAddParameter("ConFinLvlKy", orderHeader.ConfidentialLevelKey);
                    dbCommand.CreateAndAddParameter("RecurDlvNo", orderHeader.RecurenceDeliveryNo);
                    dbCommand.CreateAndAddParameter("RecurOrdDy", orderHeader.RecurenceOrderDay);
                    dbCommand.CreateAndAddParameter("RecurOrdTim", orderHeader.RecurenceOrderTime);
                    dbCommand.CreateAndAddParameter("ObjKy", orderHeader.ObjectKey);
                    dbCommand.CreateAndAddParameter("LocKy", BaseComboResponse.GetKeyValue(orderHeader.OrderLocation));
                    dbCommand.CreateAndAddParameter("LocKy2", orderHeader.Location2Key);
                    dbCommand.CreateAndAddParameter("AccKy", orderHeader.AccountKey);
                    dbCommand.CreateAndAddParameter("OrdCat1", orderHeader.OrderCategory1Key);
                    dbCommand.CreateAndAddParameter("OrdCat2", orderHeader.OrderCategory2Key);
                    dbCommand.CreateAndAddParameter("PrjKy", orderHeader.ProjectKey);
                    dbCommand.CreateAndAddParameter("Cd1Ky", orderHeader.Code1Key);

                    response.ExecutionStarted = DateTime.UtcNow;
                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        orderHeader.OrderKey = reader.GetColumn<int>("OrdKy");
                    }
                    response.ExecutionEnded = DateTime.UtcNow;
                    response.Value=orderHeader;

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
                    reader.Dispose();
                    dbCommand.Dispose();
                    dbConnection.Dispose();
                }


            }
        }
        public void CreateOrderLineItem(OrderLineCreateDTO item, Company company, User user, UIObject uiobject)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                string SPName = "OrdDet_InsertWeb";
                BaseServerResponse<OrderLineCreateDTO> response = new BaseServerResponse<OrderLineCreateDTO>();
                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    dbCommand.CreateAndAddParameter("@OrdKy", item.OrderKey);
                    dbCommand.CreateAndAddParameter("@OrdTypKy", BaseComboResponse.GetKeyValue(item.OrderType));
                    dbCommand.CreateAndAddParameter("@OrdDt", item.OrderDate);
                    dbCommand.CreateAndAddParameter("@Cky", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("@UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("@isAct", item.IsActive);
                    dbCommand.CreateAndAddParameter("@isApr", item.IsApproved);
                    dbCommand.CreateAndAddParameter("@LocKy", BaseComboResponse.GetKeyValue(item.OrderLineLocation));
                    dbCommand.CreateAndAddParameter("@BUKy", item.BussinessUnitKey);
                    dbCommand.CreateAndAddParameter("@AdrKy", item.AddressKey);
                    dbCommand.CreateAndAddParameter("@AccKy", item.AccountKey);
                    dbCommand.CreateAndAddParameter("@LiNo", item.LineNumber);
                    dbCommand.CreateAndAddParameter("@ItmKy", item.ItemKey);
                    dbCommand.CreateAndAddParameter("@TrnQty", item.TransactionQuantity);
                    dbCommand.CreateAndAddParameter("@TrnUnitKy", item.UnitKey);
                    dbCommand.CreateAndAddParameter("@Rate", item.Rate);
                    dbCommand.CreateAndAddParameter("@TrnRate", item.TransactionRate);
                    dbCommand.CreateAndAddParameter("@DisPer", item.DiscountPercentage);
                    dbCommand.CreateAndAddParameter("@DOHPer", item.DohPercentage);
                    dbCommand.CreateAndAddParameter("@GOHPer", item.GohPercentage);
                    dbCommand.CreateAndAddParameter("@ProfitPer", item.ProfitProcentage);
                    dbCommand.CreateAndAddParameter("@DisAmt", item.DisocuntAmount);
                    dbCommand.CreateAndAddParameter("@TrnDisAmt", item.TransactionDiscountAmount);
                    dbCommand.CreateAndAddParameter("@ReqDt", item.RequiredDate);
                    dbCommand.CreateAndAddParameter("@ItmPrpKy", item.ItemPropertyKey);
                    dbCommand.CreateAndAddParameter("@IsSetOff", item.IsSetOff);
                    //dbCommand.CreateAndAddParameter("@PrjKy", item.Project1.ProjectKey);
                    //dbCommand.CreateAndAddParameter("@Prj2Ky", item.Project2.ProjectKey);
                    //dbCommand.CreateAndAddParameter("@Prj2Ky", item.Project3.ProjectKey);
                    //dbCommand.CreateAndAddParameter("@Prj2Ky", item.Project4.ProjectKey);
                    //dbCommand.CreateAndAddParameter("@Prj2Ky", item.Project5.ProjectKey);
                    //dbCommand.CreateAndAddParameter("@Prj2Ky", item.Project5.ProjectKey);
                    dbCommand.CreateAndAddParameter("@Des", item.Description);
                    dbCommand.CreateAndAddParameter("@Rem", item.Remarks);
                    dbCommand.CreateAndAddParameter("@Amt1", item.Amount1);
                    dbCommand.CreateAndAddParameter("@DlvAdrKy", item.DeliveryAddressKey);
                    dbCommand.CreateAndAddParameter("@ResrAdrKy", item.ReserveAddressKey);
                    dbCommand.CreateAndAddParameter("@ObjKy", uiobject.ObjectId);
                    dbCommand.CreateAndAddParameter("@isTransfer", item.IsTransfer);
                    dbCommand.CreateAndAddParameter("@isConfirmed", item.IsConfirmed);
                    dbCommand.CreateAndAddParameter("@OrgQty", item.OriginalQuantity);

                    dbCommand.CreateAndAddParameter("@ItmTaxTyp1", item.ItemTaxType1);
                    dbCommand.CreateAndAddParameter("@ItmTaxTyp2", item.ItemTaxType2);
                    dbCommand.CreateAndAddParameter("@ItmTaxTyp3", item.ItemTaxType3);
                    dbCommand.CreateAndAddParameter("@ItmTaxTyp4", item.ItemTaxType4);
                    dbCommand.CreateAndAddParameter("@ItmTaxTyp5", item.ItemTaxType5);

                    dbCommand.CreateAndAddParameter("@ItmTaxTyp1Per", item.ItemTaxType1Per);
                    dbCommand.CreateAndAddParameter("@ItmTaxTyp2Per", item.ItemTaxType2Per);
                    dbCommand.CreateAndAddParameter("@ItmTaxTyp3Per", item.ItemTaxType3Per);
                    dbCommand.CreateAndAddParameter("@ItmTaxTyp4Per", item.ItemTaxType4Per);
                    dbCommand.CreateAndAddParameter("@ItmTaxTyp5Per", item.ItemTaxType5Per);
                    dbCommand.CreateAndAddParameter("@PrjKy", item.ProjectKey);

                    //  dbCommand.CreateAndAddParameter("ItmPrpKy", item.ItemProperty1);

                    // dbCommand.CreateAndAddParameter("IsSetOff",item.)

                    response.ExecutionStarted = DateTime.UtcNow;
                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        item.OrderLineItemKey = reader.GetColumn<int>("ReturnVal");
                    }


                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }


                    response.ExecutionEnded = DateTime.UtcNow;
                    response.Value = item;
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
                    reader.Dispose();
                    dbCommand.Dispose();
                    dbConnection.Dispose();
                }


            }
        }
        public void PostInterLocationTransfers(Company company, User user, long OrderKey = 1, long ObjectKey = 0)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                string SPName= "POrdKySlsOrdToTrn_PostWeb";
                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    dbCommand.CreateAndAddParameter("@OrdKy", OrderKey);
                    dbCommand.CreateAndAddParameter("@Cky", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("@UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("@ObjKy", ObjectKey);

                    //   dbCommand.CreateAndAddParameter("@ObjKy", user.FormObjectKey);
                    dbCommand.Connection.Open();
                    dbCommand.ExecuteNonQuery();

                }
                catch (Exception exp)
                {
                    throw exp;
                }

                finally
                {
                    IDbConnection dbConnection = dbCommand.Connection;



                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }

                    dbCommand.Dispose();
                    dbConnection.Dispose();
                }


            }
        }
        public OrderSimpleToWMS GetOrderByOrderKey(int OrderKey, User user)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader dataReader = null;
                OrderSimpleToWMS orderTransafer = new OrderSimpleToWMS();
                orderTransafer.LineItems = new List<OrderSimpleLineItem>();
                string SPName = "PurchaseOrderAPI_Reportweb";
                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    dbCommand.CreateAndAddParameter("@OrdKy", OrderKey);
                    dbCommand.CreateAndAddParameter("@UsrKy", 342247);

                    //   dbCommand.CreateAndAddParameter("@ObjKy", user.FormObjectKey);
                    dbCommand.Connection.Open();
                    dataReader = dbCommand.ExecuteReader();

                    while (dataReader.Read())
                    {
                        orderTransafer.OrderReference = dataReader.GetColumn<string>("PONo");
                        orderTransafer.SupplierName = dataReader.GetColumn<string>("SuplierName");
                        orderTransafer.Prefix = dataReader.GetColumn<string>("PreifxCode");
                        orderTransafer.SupplierReference = dataReader.GetColumn<string>("SuplierCode");
                        orderTransafer.OrderDate = dataReader.GetColumn<DateTime>("PODate");
                        orderTransafer.DeliveryDate = dataReader.GetColumn<DateTime>("DlryDt");
                        orderTransafer.SUSR1 = "Local Supplier";
                        orderTransafer.EXTERNRECEIPTKEY = dataReader.GetColumn<string>("PONo");
                        orderTransafer.REQUIREDDELIVERYDATE = dataReader.GetColumn<DateTime>("DlryDt");

                        orderTransafer.OrderKey = dataReader.GetColumn<int>("OrdKy");
                        OrderSimpleLineItem line = new OrderSimpleLineItem();
                        line.ProductName = dataReader.GetColumn<string>("ItmNm");

                        // line.SKU = dataReader.GetColumn<string>("ItmCd");
                        line.TransactionRate = dataReader.GetColumn<decimal>("TrnRate");
                        line.LineTotal = dataReader.GetColumn<decimal>("TrnAmt");
                        line.LineDiscountAmount = dataReader.GetColumn<decimal>("TrnDisAmt");
                        line.Quantity = dataReader.GetColumn<decimal>("TrnQty");
                        line.LineNumber = dataReader.GetColumn<int>("OrdDetKy");
                        orderTransafer.LineItems.Add(line);

                    }





                }
                catch (Exception exp)
                {

                }

                finally
                {
                    IDbConnection dbConnection = dbCommand.Connection;

                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }

                    dbCommand.Dispose();
                    dbConnection.Dispose();
                }
                return orderTransafer;

            }
        }
        public void UpdateGenericOrderHeader(OrderHeaderEditDTO orderV3, Company company, User user)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                string SPName= "OrdHdr_UpdateWeb";
                BaseServerResponse<OrderHeaderEditDTO> response = new BaseServerResponse<OrderHeaderEditDTO>();
                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;

                    dbCommand.CreateAndAddParameter("CKy", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("Original_OrdKy", orderV3.OrderKey);
                    dbCommand.CreateAndAddParameter("OrdKy", orderV3.OrderKey);
                    dbCommand.CreateAndAddParameter("LocKy", BaseComboResponse.GetKeyValue(orderV3.OrderLocation));
                    dbCommand.CreateAndAddParameter("UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("OrdDt", orderV3.OrderDate);
                    dbCommand.CreateAndAddParameter("OrdTypKy", BaseComboResponse.GetKeyValue(orderV3.OrderType));
                    dbCommand.CreateAndAddParameter("PmtTrmKy", BaseComboResponse.GetKeyValue(orderV3.PayementTerm));
                    dbCommand.CreateAndAddParameter("DocNo", orderV3.DocumentNumber == null ? "" : orderV3.DocumentNumber);
                    dbCommand.CreateAndAddParameter("YurRef", orderV3.YourReference == null ? "" : orderV3.YourReference);
                    dbCommand.CreateAndAddParameter("YurRefDt", orderV3.YourReferenceDate);
                    dbCommand.CreateAndAddParameter("Amt", orderV3.Amount);
                    dbCommand.CreateAndAddParameter("TrnAmt", orderV3.TransactionAmount);
                    dbCommand.CreateAndAddParameter("TrnCrnKy", BaseComboResponse.GetKeyValue(orderV3.TransactionCurrency));
                    dbCommand.CreateAndAddParameter("TrnExRate", orderV3.TransactionExchangeRate);
                    dbCommand.CreateAndAddParameter("DisPer", orderV3.DiscountPercentage);
                    dbCommand.CreateAndAddParameter("DisAmt", orderV3.DiscountAmount);
                    dbCommand.CreateAndAddParameter("TrnDisAmt", orderV3.TransactionDiscountAmount);
                    dbCommand.CreateAndAddParameter("IsOrdSetOff", orderV3.IsOrderSetOff);
                    dbCommand.CreateAndAddParameter("Des", orderV3.Description == null ? "" : orderV3.Description);
                    dbCommand.CreateAndAddParameter("Rem", orderV3.Remarks);
                    dbCommand.CreateAndAddParameter("OrdFrqKy", BaseComboResponse.GetKeyValue(orderV3.OrderFrequency));
                    dbCommand.CreateAndAddParameter("OrdStsKy", BaseComboResponse.GetKeyValue(orderV3.OrderStatus));
                    dbCommand.CreateAndAddParameter("AdrKy", BaseComboResponse.GetKeyValue(orderV3.OrderAdress));
                    dbCommand.CreateAndAddParameter("RepAdrKy", BaseComboResponse.GetKeyValue(orderV3.RepAdress));
                    dbCommand.CreateAndAddParameter("DistAdrKy", BaseComboResponse.GetKeyValue(orderV3.DistributerAddress));
                    dbCommand.CreateAndAddParameter("DlryDt", orderV3.DeliveryDate);
                    dbCommand.CreateAndAddParameter("OrdFinDt", orderV3.OrderFinishDate);
                    dbCommand.CreateAndAddParameter("CusItmKy", orderV3.CustomeItemKey);
                    dbCommand.CreateAndAddParameter("IsAct", orderV3.IsActive);
                    dbCommand.CreateAndAddParameter("IsApr", orderV3.IsApproved);
                    dbCommand.CreateAndAddParameter("IsPrinted", orderV3.IsPrinted);
                    dbCommand.CreateAndAddParameter("IsLocked", orderV3.IsRecordLocked);
                    dbCommand.CreateAndAddParameter("AcsLvlKy", BaseComboResponse.GetKeyValue(orderV3.AccessLevel));
                    dbCommand.CreateAndAddParameter("ConFinLvlKy", BaseComboResponse.GetKeyValue(orderV3.ConfidentialLevel));
                    dbCommand.CreateAndAddParameter("RecurDlvNo", orderV3.RecurenceDeliveryKey);
                    dbCommand.CreateAndAddParameter("RecurOrdDy", orderV3.RecurenceDeliveryKey);
                    dbCommand.CreateAndAddParameter("RecurOrdTim", orderV3.RecurenceDeliveryKey);
                    dbCommand.CreateAndAddParameter("OrdPrefixKy", BaseComboResponse.GetKeyValue(orderV3.OrderPrefix));
                    dbCommand.CreateAndAddParameter("OrdNo", orderV3.OrderNumber.ToString());
                    dbCommand.CreateAndAddParameter("OrdCat1", BaseComboResponse.GetKeyValue(orderV3.OrderCategory1));
                    dbCommand.CreateAndAddParameter("OrdCat2", BaseComboResponse.GetKeyValue(orderV3.OrderCategory2));
                    dbCommand.CreateAndAddParameter("OrdCat3", BaseComboResponse.GetKeyValue(orderV3.OrderCategory3));
                    dbCommand.CreateAndAddParameter("LocKy2", BaseComboResponse.GetKeyValue(orderV3.Location2));
                    dbCommand.CreateAndAddParameter("AprStsKy", BaseComboResponse.GetKeyValue(orderV3.ApproveStatus));
                    dbCommand.CreateAndAddParameter("AprPrtyKy", BaseComboResponse.GetKeyValue(orderV3.ApprovePriorty));
                    dbCommand.CreateAndAddParameter("ObjKy", orderV3.ObjectKey);
                    dbCommand.CreateAndAddParameter("Maint", 1);
                    dbCommand.CreateAndAddParameter("@Original_TmStmp", 1);
                    dbCommand.CreateAndAddParameter("BuKy", BaseComboResponse.GetKeyValue(orderV3.BussinessUnit));
                    dbCommand.CreateAndAddParameter("PrjKy", orderV3.ProjectKey);
                    dbCommand.CreateAndAddParameter("Cd1Ky", orderV3.Code1Key);

                    response.ExecutionStarted = DateTime.UtcNow;
                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        orderV3.OrderKey = reader.GetColumn<int>("OrdKy");
                        //req qty -org qty 
                    }
                    response.ExecutionEnded = DateTime.UtcNow;
                    response.Value = orderV3;
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
                    reader.Dispose();
                    dbCommand.Dispose();
                    dbConnection.Dispose();
                }


            }
        }
        public void UpdateGenericOrderLineItem(OrderLineCreateDTO item, long ObjKy, Company company, User user)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                string SPName = "OrdDet_UpdateWeb";
                BaseServerResponse<OrderLineCreateDTO> response = new BaseServerResponse<OrderLineCreateDTO>();
                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    dbCommand.CreateAndAddParameter("@OrdKy", item.OrderKey);
                    dbCommand.CreateAndAddParameter("@OrdDetKy", item.FromOrderDetailKey);
                    dbCommand.CreateAndAddParameter("@OrdTypKy", BaseComboResponse.GetKeyValue(item.OrderType));
                    dbCommand.CreateAndAddParameter("@Cky", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("@UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("@isAct", item.IsActive);
                    dbCommand.CreateAndAddParameter("@isApr", item.IsApproved);
                    dbCommand.CreateAndAddParameter("@LocKy", BaseComboResponse.GetKeyValue(item.OrderLineLocation));
                    dbCommand.CreateAndAddParameter("@BUKy", item.BussinessUnitKey);
                    dbCommand.CreateAndAddParameter("@AdrKy", item.AddressKey);
                    dbCommand.CreateAndAddParameter("@AccKy", item.AccountKey);
                    dbCommand.CreateAndAddParameter("@LiNo", item.LineNumber);
                    dbCommand.CreateAndAddParameter("@ItmKy", item.ItemKey);
                    dbCommand.CreateAndAddParameter("@OrdItmNm", item.OrderItemName);
                    dbCommand.CreateAndAddParameter("@TrnQty", item.TransactionQuantity);
                    dbCommand.CreateAndAddParameter("@TrnUnitKy", item.UnitKey);
                    dbCommand.CreateAndAddParameter("@Rate", item.Rate);
                    dbCommand.CreateAndAddParameter("@TrnRate", item.TransactionRate);
                    dbCommand.CreateAndAddParameter("@DisPer", item.DiscountPercentage);
                    dbCommand.CreateAndAddParameter("@DOHPer", item.DohPercentage);
                    dbCommand.CreateAndAddParameter("@GOHPer", item.GohPercentage);
                    dbCommand.CreateAndAddParameter("@ProfitPer", item.ProfitProcentage);
                    dbCommand.CreateAndAddParameter("@DisAmt", item.DisocuntAmount);
                    dbCommand.CreateAndAddParameter("@TrnDisAmt", item.TransactionDiscountAmount);
                    dbCommand.CreateAndAddParameter("@ReqDt", item.RequiredDate);
                    dbCommand.CreateAndAddParameter("@ItmPrpKy", item.ItemPropertyKey);
                    dbCommand.CreateAndAddParameter("@IsSetOff", item.IsSetOff);
                    dbCommand.CreateAndAddParameter("@Des", item.Description);
                    dbCommand.CreateAndAddParameter("@Amt1", item.Amount1);
                    dbCommand.CreateAndAddParameter("@DlvAdrKy", item.DeliveryAddressKey);
                    dbCommand.CreateAndAddParameter("@ResrAdrKy", item.ReserveAddressKey);
                    dbCommand.CreateAndAddParameter("@ObjKy", ObjKy);
                    dbCommand.CreateAndAddParameter("@isTransfer", item.IsTransfer);
                    dbCommand.CreateAndAddParameter("@isConfirmed", item.IsConfirmed);
                    dbCommand.CreateAndAddParameter("@FrmOrdDetKy", item.FromOrderDetailKey);
                    dbCommand.CreateAndAddParameter("@TrnPri", item.TransactionPrice);
                    dbCommand.CreateAndAddParameter("@OrgQty", item.OriginalQuantity);
                    dbCommand.CreateAndAddParameter("@PrjKy", item.ProjectKey);

                    response.ExecutionStarted = DateTime.UtcNow;
                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        item.OrderLineItemKey = reader.GetColumn<int>("ReturnVal");
                    }

                    response.ExecutionStarted = DateTime.UtcNow;
                    response.Value = item;
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
                    reader.Dispose();
                    dbCommand.Dispose();
                    dbConnection.Dispose();
                }


            } // End of OrderItem Update
        }
        public BaseServerResponse<IList<OrderFindResults>> GenericFindOrders(OrderFindDto dto, Company company, User user)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                IList<OrderFindResults> results = new List<OrderFindResults>();
                BaseServerResponse<IList<OrderFindResults>> response = new BaseServerResponse<IList<OrderFindResults>>();
                string SPName = "Order_FindWeb";
                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    dbCommand.CreateAndAddParameter("FrmDt", dto.FromDate);
                    dbCommand.CreateAndAddParameter("ToDt", dto.ToDate);
                    dbCommand.CreateAndAddParameter("OrdTypKy", dto.OrderTypeKey);
                    dbCommand.CreateAndAddParameter("PreFixKy", BaseComboResponse.GetKeyValue(dto.Prefix));
                    dbCommand.CreateAndAddParameter("FrmOrdNo", dto.FromOrderNumber);
                    dbCommand.CreateAndAddParameter("ToOrdNo", dto.ToOrderNumber);
                    dbCommand.CreateAndAddParameter("YurRef", dto.YourReference);
                    dbCommand.CreateAndAddParameter("LocKy", BaseComboResponse.GetKeyValue(dto.Location));
                    dbCommand.CreateAndAddParameter("PrjKy", dto.ProjectKey);
                    dbCommand.CreateAndAddParameter("AccKy", dto.AccountKey);
                    dbCommand.CreateAndAddParameter("AdrKy", dto.AddressKey);
                    dbCommand.CreateAndAddParameter("AprStsKy", dto.ApproveStatusKey);
                    dbCommand.CreateAndAddParameter("IsPrinted", dto.IsPrinterd);
                    dbCommand.CreateAndAddParameter("UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("CKy", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("ObjKy", dto.ObjectKey);
                    dbCommand.CreateAndAddParameter("DocNo", dto.DocumentNumber);

                    response.ExecutionStarted = DateTime.UtcNow;
                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        OrderFindResults findResults = new OrderFindResults();

                        findResults.OrderKey = reader.GetColumn<int>("OrdKy");
                        findResults.OrderDate = reader.GetColumn<DateTime>("OrdDt");
                        findResults.Prefix = reader.GetColumn<string>("Prefix");
                        findResults.OrderNumber = reader.GetColumn<string>("OrdNo");
                        findResults.DocumentNumber = reader.GetColumn<string>("DocNo");
                        findResults.YourReference = reader.GetColumn<string>("YurRef");
                        findResults.Description = reader.GetColumn<string>("Des");
                        findResults.CusSupId = reader.GetColumn<string>("AdrId");
                        findResults.CusSupName = reader.GetColumn<string>("AdrNm");
                        results.Add(findResults);
                    }

                    response.ExecutionEnded = DateTime.UtcNow;
                    response.Value = results;

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
                    reader.Dispose();
                    dbCommand.Dispose();
                    dbConnection.Dispose();
                }

                return response;
            }
        }

        public BaseServerResponse<OrderHeaderEditDTO> GetGenericOrderByOrderKey(long Orderkey, Company company, User user)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                OrderHeaderEditDTO oorderV3 = new OrderHeaderEditDTO();
                BaseServerResponse<OrderHeaderEditDTO> response = new BaseServerResponse<OrderHeaderEditDTO>();
                string SPName = "OrdHdr_SelectWeb";
                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    dbCommand.CreateAndAddParameter("CKy", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("OrdKy", Orderkey);

                    response.ExecutionStarted = DateTime.UtcNow;
                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        oorderV3 = new OrderHeaderEditDTO();
                        oorderV3.OrderKey = reader.GetColumn<int>("OrdKy");
                        oorderV3.OrderType = this.GetCdMasByCdKy(reader.GetColumn<int>("OrdTypKy"));
                        oorderV3.OrderNumber = reader.GetColumn<int>("OrdNo");
                        oorderV3.OrderDate = reader.GetColumn<DateTime>("OrdDt");
                        oorderV3.PayementTerm = this.GetCdMasByCdKy(reader.GetColumn<int>("PmtTrmKy"));
                        oorderV3.DocumentNumber = reader.GetColumn<string>("DocNo");
                        oorderV3.OrderLocation = new CodeBaseResponse();
                        oorderV3.OrderLocation.CodeKey = reader.GetColumn<int>("OrdHdrLocKy");
                        oorderV3.OrderLocation.CodeName = reader.GetColumn<string>("OrdHdrLocNm");
                        oorderV3.OrderAdress = new AddressResponse(reader.GetColumn<int>("AdrKy"), reader.GetColumn<string>("AdrId"), reader.GetColumn<string>("AdrNm"));
                        oorderV3.RepAdress = new AddressResponse(reader.GetColumn<int>("RepAdrKy"), "", reader.GetColumn<string>("RepAdrNm"));
                        oorderV3.ObjectKey = reader.GetColumn<int>("ObjKy");
                        oorderV3.TransactionDiscountAmount = reader.GetColumn<decimal>("TrnDisAmt");
                        oorderV3.DiscountPercentage = reader.GetColumn<decimal>("DisPer");
                        oorderV3.DiscountAmount = reader.GetColumn<decimal>("DisAmt");
                        //oorderV3.OrderPrefix = new CodeBaseResponse(reader.GetColumn<int>("OrdPrefixKy"));
                        oorderV3.YourReference = reader.GetColumn<string>("YurRef");
                        oorderV3.Amount = reader.GetColumn<decimal>("Amt");
                        oorderV3.TransactionAmount = reader.GetColumn<decimal>("TrnAmt");
                        oorderV3.OrderStatus = new CodeBaseResponse(reader.GetColumn<int>("OrdStsKy"));
                        oorderV3.IsActive = reader.GetColumn<int>("IsAct");//check isact or objky 
                        oorderV3.IsApproved = reader.GetColumn<int>("IsApr");
                        oorderV3.Description = reader.GetColumn<string>("Des");
                        oorderV3.OrderPrefix = new CodeBaseResponse() { CodeName = reader.GetColumn<string>("Prefix") ?? "" };
                        oorderV3.OrderCategory1= this.GetCdMasByCdKy(reader.GetColumn<int>("OrdCat1Ky"));
                        oorderV3.OrderCategory2 = this.GetCdMasByCdKy(reader.GetColumn<int>("OrdCat2Ky"));
                        oorderV3.ProjectKey = reader.GetColumn<int>("PrjKy");
                    }
                    response.ExecutionEnded = DateTime.UtcNow;
                    response.Value = oorderV3;

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
                    reader.Dispose();
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

        public BaseServerResponse<IList<OrderLineCreateDTO>> GetGenericOrderLineItemsByOrderKey(long Orderkey, long ObjKy, Company company, User user)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                IList<OrderLineCreateDTO> itemList = new List<OrderLineCreateDTO>();
                BaseServerResponse<IList<OrderLineCreateDTO>> response = new BaseServerResponse<IList<OrderLineCreateDTO>>();
                string SPName = "OrdDet_selectWeb";
                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    dbCommand.CreateAndAddParameter("CKy", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("OrdKy", Orderkey);
                    dbCommand.CreateAndAddParameter("ObjKy", ObjKy);

                    response.ExecutionStarted = DateTime.UtcNow;
                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        OrderLineCreateDTO oorderV3 = new OrderLineCreateDTO();
                        oorderV3.FromOrderDetailKey = reader.GetColumn<int>("OrdDetKy");
                        oorderV3.LineNumber = reader.GetColumn<int>("LiNo");
                        oorderV3.OrderKey = reader.GetColumn<int>("OrdKy");
                        oorderV3.TransactionQuantity = reader.GetColumn<decimal>("TrnQty");
                        oorderV3.Rate = reader.GetColumn<decimal>("Rate");
                        oorderV3.TransactionRate = reader.GetColumn<decimal>("TrnRate");
                        oorderV3.DiscountPercentage = reader.GetColumn<decimal>("DisPer");
                        oorderV3.DisocuntAmount = reader.GetColumn<decimal>("DisAmt");
                        oorderV3.OrderItemName = reader.GetColumn<string>("ItmNm");
                        oorderV3.ItemKey = reader.GetColumn<int>("ItmKy");
                        oorderV3.IsTransfer = reader.GetColumn<int>("isTransfer");
                        oorderV3.IsConfirmed = reader.GetColumn<int>("isConfirmed");
                        oorderV3.IsActive = reader.GetColumn<int>("isAct");
                        oorderV3.IsApproved = reader.GetColumn<int>("isApr");
                        oorderV3.OrderLineLocation = this.GetCdMasByCdKy(reader.GetColumn<int>("LocKy"));
                        oorderV3.OriginalQuantity = reader.GetColumn<decimal>("OrgQty");
                        oorderV3.ItemTaxType1Per = reader.GetColumn<decimal>("ItmTaxTyp1Per");//vat
                        oorderV3.ItemTaxType2Per = reader.GetColumn<decimal>("ItmTaxTyp2Per");
                        oorderV3.ItemTaxType3Per = reader.GetColumn<decimal>("ItmTaxTyp3Per");
                        oorderV3.ItemTaxType4Per = reader.GetColumn<decimal>("ItmTaxTyp4Per");//svat
                        oorderV3.ItemTaxType5Per = reader.GetColumn<decimal>("ItmTaxTyp5Per");
                        oorderV3.Remarks = reader.GetColumn<string>("Rem");
                        oorderV3.Description = reader.GetColumn<string>("Des");
                        oorderV3.ItemTypeKey= reader.GetColumn<int>("itmtypKy");
                        oorderV3.ItemTypeName = reader.GetColumn<string>("ItmTypNm");
                        oorderV3.ItemTypeOurCode = reader.GetColumn<string>("ItmTypOurCd");
                        //carmrt and principle values customer amount

                        itemList.Add(oorderV3);
                    }
                    response.ExecutionEnded = DateTime.UtcNow;
                    response.Value = itemList;

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
                    reader.Dispose();
                    dbCommand.Dispose();
                    dbConnection.Dispose();

                }
                return response;

            }
        }
        public CodeBaseResponse OrderStatusFindByOrdKy(Company company, User user, int objky = 1, int ordky = 1)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                CodeBaseResponse approveState = new CodeBaseResponse();
                string SPName = "OrdHdrApr_LatestState";
                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;

                    dbCommand.CreateAndAddParameter("@Cky", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("@UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("@ObjKy", objky);
                    dbCommand.CreateAndAddParameter("@OrdKy", ordky);

                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        approveState = new CodeBaseResponse();
                        approveState.CodeKey = reader.GetColumn<int>("AprStsKy");
                        approveState.CodeName = reader.GetColumn<string>("AprNm");
                        approveState.Code = reader.GetColumn<string>("AprCd");

                    }

                }
                catch (Exception exp)
                {

                }

                finally
                {
                    IDbConnection dbConnection = dbCommand.Connection;

                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }

                    dbCommand.Dispose();
                    dbConnection.Dispose();
                }

                return approveState;
            }
        }
        public BaseServerResponse<IList<GetFromQuotResults>> GenericRetrieveQuotation(GetFromQuoatationDTO dto, Company company, User user)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                IList<GetFromQuotResults> quaotationList = new List<GetFromQuotResults>();
                string SPName = "OrdTypPendingOrdTyp_SelectWeb";
                BaseServerResponse<IList<GetFromQuotResults>> response = new BaseServerResponse<IList<GetFromQuotResults>>();

                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    dbCommand.CreateAndAddParameter("@OrdTypKy", dto.OrdTypKy);
                    dbCommand.CreateAndAddParameter("@PreOrdTypKy", dto.PreOrdTypKy);
                    dbCommand.CreateAndAddParameter("@SupAccKy", BaseComboResponse.GetKeyValue(dto.Supplier));
                    dbCommand.CreateAndAddParameter("@AdrKy", BaseComboResponse.GetKeyValue(dto.AdvAnalysis));
                    dbCommand.CreateAndAddParameter("@LocKy", BaseComboResponse.GetKeyValue(dto.Location));
                    dbCommand.CreateAndAddParameter("@FrmDt", dto.FromDate);
                    dbCommand.CreateAndAddParameter("@ToDt", dto.ToDate);
                    dbCommand.CreateAndAddParameter("@PrjKy", BaseComboResponse.GetKeyValue(dto.Project));
                    dbCommand.CreateAndAddParameter("@OrdNo", null);
                    dbCommand.CreateAndAddParameter("@PreOrdPreFixKy", BaseComboResponse.GetKeyValue(null));
                    dbCommand.CreateAndAddParameter("@CKy", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("@UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("@ObjKy", dto.ObjKy);

                    response.ExecutionStarted = DateTime.UtcNow;
                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        GetFromQuotResults quote = new GetFromQuotResults();
                        quote.OrdKy = reader.GetColumn<int>("OrdKy");
                        quote.OrdNo = reader.GetColumn<string>("OrdNo");
                        quote.OrdDt = reader.GetColumn<DateTime>("OrdDt");
                        quote.DocNo = reader.GetColumn<string>("DocNo");
                        quote.PrjId = reader.GetColumn<int>("PrjId");
                        quote.PrjNm = reader.GetColumn<string>("PrjNm");
                        quote.SupAccCd = reader.GetColumn<string>("SupAccCd");
                        quote.SupAccNm = reader.GetColumn<string>("SupAccNm");
                        quote.Prefix = reader.GetColumn<string>("Prefix");
                        quote.LocCd = reader.GetColumn<string>("LocCd");
                        quaotationList.Add(quote);
                    }
                    response.ExecutionStarted = DateTime.UtcNow;
                    response.Value = quaotationList;

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
                    reader.Dispose();
                    dbCommand.Dispose();
                    dbConnection.Dispose();
                }

                return response;
            }
        }
        public BaseServerResponse<IList<QuotationDetails>> GenericOpenQuotation(OrderOpenRequest request, Company company, User user)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                IList<QuotationDetails> quotation = new List<QuotationDetails>();
                BaseServerResponse<IList<QuotationDetails>> response = new BaseServerResponse<IList<QuotationDetails>>();
                string SPName= "OrdTypPendingOrdTypDetails_SelectWeb";
                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    dbCommand.CreateAndAddParameter("CKy", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("OrdKy", request.OrderKey);
                    dbCommand.CreateAndAddParameter("BaseTypKy", request.BaseTypKy);
                    dbCommand.CreateAndAddParameter("PrjKy", request.PrjKy);
                    dbCommand.CreateAndAddParameter("ObjKy", request.ObjKy);

                    response.ExecutionStarted = DateTime.UtcNow;
                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        QuotationDetails oorderV3 = new QuotationDetails();
                        oorderV3.LineNumber = reader.GetColumn<int>("LiNo");
                        oorderV3.OrderKey = reader.GetColumn<int>("OrdKy");
                        oorderV3.OrderType = this.GetCdMasByCdKy(reader.GetColumn<int>("OrdTypKy"));
                        oorderV3.OrdNo = reader.GetColumn<string>("OrdNo");
                        oorderV3.OrderDate = reader.GetColumn<DateTime>("OrdDt");
                        oorderV3.DocNo = reader.GetColumn<string>("DocNo");
                        oorderV3.YourReference = reader.GetColumn<string>("YurRef");
                        oorderV3.FromOrderDetailKey = reader.GetColumn<int>("OrdDetKy");
                        oorderV3.TransactionQuantity = reader.GetColumn<decimal>("Qty");
                        oorderV3.AvailableQuantity = reader.GetColumn<decimal>("AvlQty");
                        oorderV3.ItemKey = reader.GetColumn<int>("ItmKy");
                        oorderV3.OrderItemName = reader.GetColumn<string>("ItmNm");
                        oorderV3.TransactionUnitKey = reader.GetColumn<int>("TrnUnitKy");
                        oorderV3.Unit = new UnitResponse(reader.GetColumn<long>("UnitKy"), reader.GetColumn<string>("Unit"));
                        oorderV3.Rate = reader.GetColumn<decimal>("Rate");
                        oorderV3.OrderLineLocation = this.GetCdMasByCdKy(reader.GetColumn<int>("LocKy"));
                        oorderV3.AccountKey = reader.GetColumn<int>("AccKy");
                        oorderV3.OrderAdress = new AddressResponse(reader.GetColumn<int>("AdrKy"), reader.GetColumn<string>("AdrID"), reader.GetColumn<string>("AdrNm"));
                        oorderV3.RepAdress = new AddressResponse(reader.GetColumn<int>("RepAdrKy"), "", reader.GetColumn<string>("RepAdrNm"));
                        oorderV3.DistributeAddress = new AddressResponse(reader.GetColumn<int>("DistAdrKy"));
                        oorderV3.ProjectKey = reader.GetColumn<int>("PrjKy");
                        oorderV3.BussinessUnitKey = reader.GetColumn<int>("BUKy");
                        oorderV3.Description = reader.GetColumn<string>("Des");
                        oorderV3.Remarks = reader.GetColumn<string>("Rem");
                        oorderV3.TransactionRate = reader.GetColumn<decimal>("TrnRate");
                        oorderV3.RequiredDate = reader.GetColumn<DateTime>("ReqDt");
                        oorderV3.IsActive = reader.GetColumn<int>("isAct");
                        oorderV3.VAT = reader.GetColumn<decimal>("VAT");
                        oorderV3.SVAT = reader.GetColumn<decimal>("SVAT");
                        oorderV3.DiscountAmount = reader.GetColumn<decimal>("DisAmt");
                        oorderV3.DiscountPercentage = reader.GetColumn<decimal>("DisPer");//line 
                        oorderV3.AvlStk = reader.GetColumn<decimal>("AvlStk");
                        oorderV3.HdrLocation = this.GetCdMasByCdKy(reader.GetColumn<int>("LocKy"));
                        oorderV3.PaymentTerm = new CodeBaseResponse();
                        oorderV3.PaymentTerm.CodeKey = reader.GetColumn<int>("PmtTrmKy ");
                        oorderV3.PaymentTerm.CodeName = reader.GetColumn<string>("PmtTrmNm");

                        quotation.Add(oorderV3);
                    }
                    response.ExecutionEnded = DateTime.UtcNow;
                    response.Value = quotation;

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
                    reader.Dispose();
                    dbCommand.Dispose();
                    dbConnection.Dispose();

                }

                return response;
            }
        }

        public BaseServerResponse<OrderHeaderEditDTO> GetGenericOrderByOrderKeyV2(long Orderkey, Company company, User user)
        {
            using (IDbCommand dbCommand = _dataLayer.GetCommandAccess())
            {
                IDataReader reader = null;
                OrderHeaderEditDTO oorderV3 = new OrderHeaderEditDTO();
                BaseServerResponse<OrderHeaderEditDTO> response = new BaseServerResponse<OrderHeaderEditDTO>();
                string SPName = "OrdHdrV2_SelectWeb";
                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.CommandText = SPName;
                    dbCommand.CreateAndAddParameter("CKy", company.CompanyKey);
                    dbCommand.CreateAndAddParameter("UsrKy", user.UserKey);
                    dbCommand.CreateAndAddParameter("OrdKy", Orderkey);

                    response.ExecutionStarted = DateTime.UtcNow;
                    dbCommand.Connection.Open();
                    reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        oorderV3 = new OrderHeaderEditDTO();
                        oorderV3.OrderKey = reader.GetColumn<int>("OrdKy");
                        oorderV3.OrderType = this.GetCdMasByCdKy(reader.GetColumn<int>("OrdTypKy"));
                        oorderV3.OrderNumber = reader.GetColumn<int>("OrdNo");
                        oorderV3.OrderDate = reader.GetColumn<DateTime>("OrdDt");
                        oorderV3.PayementTerm = this.GetCdMasByCdKy(reader.GetColumn<int>("PmtTrmKy"));
                        oorderV3.DocumentNumber = reader.GetColumn<string>("DocNo");
                        oorderV3.OrderLocation = new CodeBaseResponse();
                        oorderV3.OrderLocation.CodeKey = reader.GetColumn<int>("OrdHdrLocKy");
                        oorderV3.OrderLocation.CodeName = reader.GetColumn<string>("OrdHdrLocNm");
                        oorderV3.OrderAdress = new AddressResponse(reader.GetColumn<int>("AdrKy"), reader.GetColumn<string>("AdrId"), reader.GetColumn<string>("AdrNm"));
                        oorderV3.RepAdress = new AddressResponse(reader.GetColumn<int>("RepAdrKy"), "", reader.GetColumn<string>("RepAdrNm"));
                        oorderV3.ObjectKey = reader.GetColumn<int>("ObjKy");
                        oorderV3.TransactionDiscountAmount = reader.GetColumn<decimal>("TrnDisAmt");
                        oorderV3.DiscountPercentage = reader.GetColumn<decimal>("DisPer");
                        oorderV3.DiscountAmount = reader.GetColumn<decimal>("DisAmt");
                        //oorderV3.OrderPrefix = new CodeBaseResponse(reader.GetColumn<int>("OrdPrefixKy"));
                        oorderV3.YourReference = reader.GetColumn<string>("YurRef");
                        oorderV3.Amount = reader.GetColumn<decimal>("Amt");
                        oorderV3.TransactionAmount = reader.GetColumn<decimal>("TrnAmt");
                        oorderV3.OrderStatus = new CodeBaseResponse(reader.GetColumn<int>("OrdStsKy"));
                        oorderV3.IsActive = reader.GetColumn<int>("IsAct");//check isact or objky 
                        oorderV3.IsApproved = reader.GetColumn<int>("IsApr");
                        oorderV3.Description = reader.GetColumn<string>("Des");
                        oorderV3.OrderPrefix = new CodeBaseResponse() { CodeName = reader.GetColumn<string>("Prefix") ?? "" };
                        oorderV3.OrderCategory1 = this.GetCdMasByCdKy(reader.GetColumn<int>("OrdCat1Ky"));
                        oorderV3.OrderCategory2 = this.GetCdMasByCdKy(reader.GetColumn<int>("OrdCat2Ky"));
                        oorderV3.ProjectKey = reader.GetColumn<int>("PrjKy");
                    }
                    response.ExecutionEnded = DateTime.UtcNow;
                    response.Value = oorderV3;

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
                    reader.Dispose();
                    dbCommand.Dispose();
                    dbConnection.Dispose();

                }

                return response;
            }
        }
    }
}
