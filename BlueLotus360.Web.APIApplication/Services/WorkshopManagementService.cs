using BlueLotus360.Core.Domain.Definitions.DataLayer;
using BlueLotus360.Core.Domain.DTOs;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.BookingModule;
using BlueLotus360.Core.Domain.Entity.MastrerData;
using BlueLotus360.Core.Domain.Entity.Order;
using BlueLotus360.Core.Domain.Entity.Transaction;
using BlueLotus360.Core.Domain.Entity.WorkOrder;
using BlueLotus360.Core.Domain.Responses;
using BlueLotus360.Web.APIApplication.Definitions.ServiceDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Web.APIApplication.Services
{
    public class WorkshopManagementService:IWorkshopManagementService
    {
        public readonly IUnitOfWork _unitOfWork;

        public WorkshopManagementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IList<Vehicle> GetVehicleDetails(VehicleSearch request,Company company,User user)
        {
            var response =_unitOfWork.WorkShopManagementRepository.SelectVehicle(request, company, user);
            foreach (var itm in response.Value)
            {
                var acc = _unitOfWork.AccountRepository.GetAccountByAddress(itm.RegisteredCustomer, company, user);
                itm.RegisteredAccount = acc.Value;
            }
            
            return response.Value;
        }

        public IList<WorkOrder> GetJobHistoryDetails(Vehicle request, Company company, User user)
        {
            var response = _unitOfWork.WorkShopManagementRepository.SelectJobhistory(request, company, user);

            return response.Value;
        }

        public IList<ProjectResponse> GetProgressingProjectDetails(Vehicle request, Company company, User user)
        {
            var response = _unitOfWork.WorkShopManagementRepository.SelectOngoingProjectDetails(request, company, user);
            return response.Value;
        }

        public BaseServerResponse<OrderSaveResponse> SaveWorkOrder(Company company, User user, GenericOrder orderDetails)
        {
            OrderHeaderCreateDTO OH = new OrderHeaderCreateDTO();
            OH.DocumentNumber = orderDetails.OrderDocumentNumber;
            OH.AddressKey = orderDetails.OrderCustomer.AddressKey;
            OH.ObjectKey = orderDetails.FormObjectKey;
            OH.AprroceStatusKey = 1;
            // OH.OrderStatusKey = OrderStatus.CodeKey;
            //OH.DeliveryDate = orderDetails.DeliveryDate;
            OH.OrderDate = DateTime.Now;
            OH.OrderType = new CodeBaseResponse();
            OH.OrderType.CodeKey = orderDetails.OrderType.CodeKey;
            OH.AccountKey = 1;
            OH.Description = orderDetails.HeaderDescription;
            OH.PayementTerm = new CodeBaseResponse();
            OH.PayementTerm.CodeKey = orderDetails.OrderPaymentTerm.CodeKey;
            OH.Location2Key = orderDetails.OrderLocation.CodeKey;
            OH.OrderLocation = new CodeBaseResponse();
            OH.OrderLocation.CodeKey = orderDetails.OrderLocation.CodeKey;
            OH.RepAddessKey = orderDetails.OrderRepAddress.AddressKey;
            OH.DiscountPercentage = orderDetails.HeaderLevelDisountPrecentage;
            OH.IsActive = 1;
            OH.IsApproved = 1;
            OH.OrderCategory1Key = (int)orderDetails.OrderCategory1.CodeKey;
            OH.OrderCategory2Key = (int)orderDetails.OrderCategory2.CodeKey;
            OH.OrderCategory3Key = ((int)orderDetails.OrderCategory3.CodeKey);
            OH.ProjectKey = (int)orderDetails.OrderProject.ProjectKey;
            OH.Code1Key = orderDetails.Cd1Ky;
            OH.OrderStatusKey = (int)orderDetails.OrderStatus.CodeKey;
            OH.MeterReading = orderDetails.MeterReading;
            OH.DeliveryDate = orderDetails.DeliveryDate;
            OH.FromOrderKey = orderDetails.FromOrderKey;

            if (!BaseComboResponse.IsEntityWithDefaultValue(orderDetails.OrderAccount))
            {
                OH.AccountKey = orderDetails.OrderAccount.AccountKey;
            }
            else
            {
                var acc = _unitOfWork.AccountRepository.GetAccountByAddress(orderDetails.OrderCustomer, company, user);
                OH.AccountKey = acc.Value.AccountKey;

            }

            IList<OrderLineCreateDTO> orderLineItems = new List<OrderLineCreateDTO>();
            _unitOfWork.OrderRepository.CreateOrder(OH, company, user);

            if (orderDetails.BaringHeaderCompanyAccount.AccountKey > 11)
            {
                WorkOrderAmountByAccount company_accdet = new WorkOrderAmountByAccount()
                {
                    OrderKey = OH.OrderKey,
                    ObjectKey = orderDetails.FormObjectKey,
                    Account = orderDetails.BaringHeaderCompanyAccount,
                    Address = orderDetails.OrderCustomer,
                    ControlConKey = orderDetails.OrderControlCondition.CodeKey,
                    LineNumber = 1,
                    Value = orderDetails.CompanyPercentage,
                    Amount = orderDetails.CompanyAmount
                };
                _unitOfWork.OrderRepository.OrderHeaderAccountInsertUpdate(company, user, company_accdet);
            }

            if (orderDetails.BaringHeaderPrincipleAccount.AccountKey > 11)
            {
                WorkOrderAmountByAccount principle_accdet = new WorkOrderAmountByAccount()
                {
                    OrderKey = OH.OrderKey,
                    ObjectKey = orderDetails.FormObjectKey,
                    Account = orderDetails.BaringHeaderPrincipleAccount,
                    Address = orderDetails.OrderCustomer,
                    ControlConKey = orderDetails.OrderControlCondition.CodeKey,
                    LineNumber = 2,
                    Value = orderDetails.PrincipalPercentage,
                    Amount = orderDetails.PrincipalAmount
                };
                _unitOfWork.OrderRepository.OrderHeaderAccountInsertUpdate(company, user, principle_accdet);
            }
            if (orderDetails.OrderAccount.AccountKey > 11)
            {
                WorkOrderAmountByAccount cus_accdet = new WorkOrderAmountByAccount()
                {
                    OrderKey = OH.OrderKey,
                    ObjectKey = orderDetails.FormObjectKey,
                    Account = orderDetails.OrderAccount,
                    Address = orderDetails.OrderCustomer,
                    ControlConKey = orderDetails.OrderControlCondition.CodeKey,
                    LineNumber = 3,
                    Value = orderDetails.CustomerPrecentage,
                    Amount = orderDetails.CustomerAmount
                };
                _unitOfWork.OrderRepository.OrderHeaderAccountInsertUpdate(company, user, cus_accdet);
            }

            if (orderDetails.OrderItems.Count() > 0)
            {
                foreach (GenericOrderItem item in orderDetails.OrderItems)
                {
                    OrderLineCreateDTO lineItem = new OrderLineCreateDTO();
                    lineItem.ItemKey = item.TransactionItem.ItemKey;
                    lineItem.OrderKey = OH.OrderKey;
                    lineItem.TransactionQuantity = item.TransactionQuantity;
                    lineItem.Rate = _unitOfWork.ItemRepository.GetCostPriceByLocAndItmKy(company, new CodeBaseResponse((int)orderDetails.OrderLocation.CodeKey), DateTime.Now, item.TransactionItem.ItemKey);
                    lineItem.TransactionRate = item.TransactionRate;
                    lineItem.AddressKey = OH.AddressKey;
                    lineItem.OrderDate = OH.OrderDate;
                    lineItem.ObjectKey = orderDetails.FormObjectKey;
                    lineItem.OrderLineLocation = new CodeBaseResponse();
                    lineItem.OrderLineLocation.CodeKey = orderDetails.OrderLocation.CodeKey;
                    lineItem.CompanyKey = company.CompanyKey;
                    lineItem.UserKey = user.UserKey;
                    lineItem.DiscountPercentage = item.DiscountPercentage;
                    lineItem.AccountKey = OH.AccountKey;
                    //  lineItem.TransactionDiscountAmount = Math.Abs(item.GetLineDiscount());
                    lineItem.LineNumber = item.LineNumber;
                    lineItem.IsActive = item.IsActive;
                    lineItem.OriginalQuantity = item.RequestedQuantity;


                    lineItem.IsApproved = item.IsApproved;
                    lineItem.OrderType = new CodeBaseResponse();
                    lineItem.OrderType.CodeKey = orderDetails.OrderType.CodeKey;
                    lineItem.IsTransfer = item.IsTransfer;
                    lineItem.IsConfirmed = item.IsTransferConfirmed;

                    lineItem.DisocuntAmount = item.DiscountAmount;
                    lineItem.TransactionDiscountAmount = item.DiscountAmount;
                    lineItem.ItemTaxType1 = item.ItemTaxType1;
                    lineItem.ItemTaxType2 = item.ItemTaxType2;
                    lineItem.ItemTaxType3 = item.ItemTaxType3;
                    lineItem.ItemTaxType4 = item.ItemTaxType4;
                    lineItem.ItemTaxType5 = item.ItemTaxType5;

                    lineItem.ItemTaxType1Per = item.ItemTaxType1Per;
                    lineItem.ItemTaxType2Per = item.ItemTaxType2Per;
                    lineItem.ItemTaxType3Per = item.ItemTaxType3Per;
                    lineItem.ItemTaxType4Per = item.ItemTaxType4Per;
                    lineItem.ItemTaxType5Per = item.ItemTaxType5Per;
                    lineItem.Remarks = item.Remark;
                    lineItem.Description = item.Description;
                    lineItem.ReserveAddressKey = (int)item.ResourceAddress.AddressKey;
                    lineItem.FromOrderDetailKey = item.FromOrderDetailKey;
                    //lineItem.FrmOrdDetKy = item.FromOrderDetKy;

                    //   TotalDiscount += Math.Abs(item.GetLineDiscount()));
                    _unitOfWork.OrderRepository.CreateOrderLineItem(lineItem, company, user, new UIObject() { ObjectId = orderDetails.FormObjectKey });

                    
                    if (item.BaringCompany.AccountKey>11)
                    {
                        WorkOrderAmountByAccount company_accdet = new WorkOrderAmountByAccount()
                        {
                            FromOrderDetailKey = lineItem.OrderLineItemKey,
                            ObjectKey = lineItem.ObjectKey,
                            Account = item.BaringCompany,
                            Address = new AddressResponse() { AddressKey = lineItem.AddressKey },
                            ControlConKey = orderDetails.OrderControlCondition.CodeKey,
                            LineNumber = 1,
                            Value = item.CompanyPrecentage,
                            Amount = item.CompanyAmount
                        };
                        _unitOfWork.OrderRepository.OrderDetailAccountInsertUpdate(company, user, company_accdet);
                    }

                    if (item.BaringPrinciple.AccountKey > 11)
                    {
                        WorkOrderAmountByAccount principle_accdet = new WorkOrderAmountByAccount()
                        {
                            FromOrderDetailKey = lineItem.OrderLineItemKey,
                            ObjectKey = lineItem.ObjectKey,
                            Account = item.BaringPrinciple,
                            Address = new AddressResponse() { AddressKey = lineItem.AddressKey },
                            ControlConKey = orderDetails.OrderControlCondition.CodeKey,
                            LineNumber = 2,
                            Value = item.PrinciplePrecentage,
                            Amount = item.PrincipleAmount
                        };
                        _unitOfWork.OrderRepository.OrderDetailAccountInsertUpdate(company, user, principle_accdet);
                    }
                    if (item.BaringCustomer.AccountKey > 11)
                    {
                        WorkOrderAmountByAccount cus_accdet = new WorkOrderAmountByAccount()
                        {
                            FromOrderDetailKey = lineItem.OrderLineItemKey,
                            ObjectKey = lineItem.ObjectKey,
                            Account = item.BaringCustomer,
                            Address = new AddressResponse() { AddressKey = lineItem.AddressKey },
                            ControlConKey = orderDetails.OrderControlCondition.CodeKey,
                            LineNumber = 3,
                            Value = item.CustomerPrecentage,
                            Amount = item.CustomerAmount
                        };
                        _unitOfWork.OrderRepository.OrderDetailAccountInsertUpdate(company, user, cus_accdet);
                    }
                }
            }


            _unitOfWork.OrderRepository.PostInterLocationTransfers(company, user, OH.OrderKey, orderDetails.FormObjectKey);


            var ordb = _unitOfWork.OrderRepository.GetGenericOrderByOrderKeyV2(OH.OrderKey, company, user); 
            OrderSaveResponse orderServerResponse = new OrderSaveResponse();
            orderServerResponse.OrderKey = ordb.Value.OrderKey;
            orderServerResponse.OrderNumber = ordb.Value.OrderNumber.ToString();
            orderServerResponse.Prefix = ordb.Value.OrderPrefix.CodeName;

            BaseServerResponse<OrderSaveResponse> orderSaveResponse = new BaseServerResponse<OrderSaveResponse>();
            orderSaveResponse.Value = orderServerResponse;

            return orderSaveResponse;
        }

        public BaseServerResponse<OrderSaveResponse> SaveIRNOrder(Company company, User user, GenericOrder order) 
        {
            OrderHeaderCreateDTO OH = new OrderHeaderCreateDTO();
            OH.DocumentNumber = order.OrderDocumentNumber;
            OH.AddressKey = order.OrderCustomer.AddressKey;
            OH.ObjectKey = order.FormObjectKey;
            OH.AprroceStatusKey = 1;
            OH.OrderDate = DateTime.Now;
            OH.OrderType = new CodeBaseResponse();
            OH.OrderType.CodeKey = order.OrderType.CodeKey;
            OH.AccountKey = 1;
            OH.Description = order.HeaderDescription;
            OH.PayementTerm = new CodeBaseResponse();
            OH.PayementTerm.CodeKey = order.OrderPaymentTerm.CodeKey;
            OH.Location2Key = order.OrderLocation.CodeKey;
            OH.OrderLocation = new CodeBaseResponse();
            OH.OrderLocation.CodeKey = order.OrderLocation.CodeKey;
            OH.RepAddessKey = order.OrderRepAddress.AddressKey;
            OH.DiscountPercentage = order.HeaderLevelDisountPrecentage;
            OH.IsActive = order.IsActive;
            OH.IsApproved = 1;
            OH.OrderCategory1Key = (int)order.OrderCategory1.CodeKey;
            OH.OrderCategory2Key = (int)order   .OrderCategory2.CodeKey;
            OH.ProjectKey = (int)order.OrderProject.ProjectKey;
            OH.Code1Key = order.Cd1Ky;
            OH.OrderStatusKey = (int)order.OrderStatus.CodeKey;
            OH.MeterReading = order.MeterReading;
            OH.DeliveryDate = order.DeliveryDate;
            OH.Insurance = new AccountResponse();
            OH.AccountKey = order.OrderAccount.AccountKey;
            //OH.Insurance.AccountKey = order.Insurance.AccountKey;

            //if (!BaseComboResponse.IsEntityWithDefaultValue(order.OrderAccount))
            //{
            //    OH.AccountKey = order.OrderAccount.AccountKey;
            //}
            //else
            //{
            //    var acc = _unitOfWork.AccountRepository.GetAccountByAddress(order.OrderCustomer, company, user);
            //    OH.AccountKey = acc.Value.AccountKey;

            //}

            _unitOfWork.OrderRepository.CreateOrder(OH, company, user);

            if (order.OrderItems.Count() > 0)
            {
                foreach (GenericOrderItem item in order.OrderItems)
                {
                    OrderLineCreateDTO lineItem = new OrderLineCreateDTO();
                    lineItem.ItemKey = item.TransactionItem.ItemKey;
                    lineItem.OrderKey = OH.OrderKey;
                    lineItem.TransactionQuantity = item.TransactionQuantity;
                    lineItem.Rate = _unitOfWork.ItemRepository.GetCostPriceByLocAndItmKy(company, new CodeBaseResponse((int)order.OrderLocation.CodeKey), DateTime.Now, item.TransactionItem.ItemKey);
                    lineItem.TransactionRate = item.TransactionRate;
                    lineItem.AddressKey = OH.AddressKey;

                    lineItem.ObjectKey = order.FormObjectKey;
                    lineItem.OrderLineLocation = new CodeBaseResponse();
                    lineItem.OrderLineLocation.CodeKey = item.OrderLineLocation.CodeKey;
                    lineItem.CompanyKey = company.CompanyKey;
                    lineItem.UserKey = user.UserKey;
                    lineItem.DiscountPercentage = item.DiscountPercentage;
                    lineItem.AccountKey = OH.AccountKey;
                    //  lineItem.TransactionDiscountAmount = Math.Abs(item.GetLineDiscount());
                    lineItem.LineNumber = item.LineNumber;
                    lineItem.IsActive = item.IsActive;
                    lineItem.OriginalQuantity = item.RequestedQuantity;


                    lineItem.IsApproved = item.IsApproved;
                    lineItem.OrderType = new CodeBaseResponse();
                    lineItem.OrderType.CodeKey = order.OrderType.CodeKey;
                    lineItem.IsTransfer = item.IsTransfer;
                    lineItem.IsConfirmed = item.IsTransferConfirmed;

                    lineItem.DisocuntAmount = item.DiscountAmount;
                    lineItem.TransactionDiscountAmount = item.DiscountAmount;
                    lineItem.ItemTaxType1 = item.ItemTaxType1;
                    lineItem.ItemTaxType2 = item.ItemTaxType2;
                    lineItem.ItemTaxType3 = item.ItemTaxType3;
                    lineItem.ItemTaxType4 = item.ItemTaxType4;
                    lineItem.ItemTaxType5 = item.ItemTaxType5;

                    lineItem.ItemTaxType1Per = item.ItemTaxType1Per;
                    lineItem.ItemTaxType2Per = item.ItemTaxType2Per;
                    lineItem.ItemTaxType3Per = item.ItemTaxType3Per;
                    lineItem.ItemTaxType4Per = item.ItemTaxType4Per;
                    lineItem.ItemTaxType5Per = item.ItemTaxType5Per;
                    lineItem.Remarks = item.Remark;
                    lineItem.Description = item.Description;
					lineItem.ReserveAddressKey = (int)item.ResourceAddress.AddressKey;
                    lineItem.AnalysisType2.CodeKey = item.AnalysisType2.CodeKey;
                    lineItem.AnalysisType4 = item.AnalysisType4;

					//   TotalDiscount += Math.Abs(item.GetLineDiscount()));
					_unitOfWork.OrderRepository.CreateOrderLineItem(lineItem, company, user, new UIObject() { ObjectId = order.FormObjectKey });


                    if (item.BaringCompany.AccountKey > 11)
                    {
                        WorkOrderAmountByAccount company_accdet = new WorkOrderAmountByAccount()
                        {
                            FromOrderDetailKey = lineItem.FromOrderDetailKey,
                            ObjectKey = lineItem.ObjectKey,
                            Account = item.BaringCompany,
                            Address = new AddressResponse() { AddressKey = lineItem.AddressKey },
                            ControlConKey = order.OrderControlCondition.CodeKey,
                            LineNumber = (int)lineItem.LineNumber,
                            Value = item.CompanyPrecentage,
                            Amount = item.CompanyAmount
                        };
                        _unitOfWork.OrderRepository.OrderDetailAccountInsertUpdate(company, user, company_accdet);
                    }

                    if (item.BaringPrinciple.AccountKey > 11)
                    {
                        WorkOrderAmountByAccount principle_accdet = new WorkOrderAmountByAccount()
                        {
                            FromOrderDetailKey = lineItem.FromOrderDetailKey,
                            ObjectKey = lineItem.ObjectKey,
                            Account = item.BaringPrinciple,
                            Address = new AddressResponse() { AddressKey = lineItem.AddressKey },
                            ControlConKey = order.OrderControlCondition.CodeKey,
                            LineNumber = (int)(lineItem.LineNumber + 1),
                            Value = item.PrinciplePrecentage,
                            Amount = item.PrincipleAmount
                        };
                        _unitOfWork.OrderRepository.OrderDetailAccountInsertUpdate(company, user, principle_accdet);
                    }
                }
            }

            _unitOfWork.OrderRepository.PostInterLocationTransfers(company, user, OH.OrderKey, order.FormObjectKey);


            var ordb = _unitOfWork.OrderRepository.GetGenericOrderByOrderKeyV2(OH.OrderKey, company, user);
            OrderSaveResponse orderServerResponse = new OrderSaveResponse();
            orderServerResponse.OrderKey = ordb.Value.OrderKey;
            orderServerResponse.OrderNumber = ordb.Value.OrderNumber.ToString();
            orderServerResponse.Prefix = ordb.Value.OrderPrefix.CodeName;

            BaseServerResponse<OrderSaveResponse> orderSaveResponse = new BaseServerResponse<OrderSaveResponse>();
            orderSaveResponse.Value = orderServerResponse;

            return orderSaveResponse;
        }

        public OrderSaveResponse UpdateWorkOrder(Company company, User user, GenericOrder orderDetails)
        {
            OrderHeaderEditDTO OH = new OrderHeaderEditDTO();
            OH.OrderKey = orderDetails.OrderKey;
            OH.OrderLocation = new CodeBaseResponse();
            OH.OrderLocation.CodeKey = orderDetails.OrderLocation.CodeKey;
            OH.OrderDate = orderDetails.OrderDate;
            OH.OrderType = new CodeBaseResponse();
            OH.OrderType.CodeKey = orderDetails.OrderType.CodeKey;
            OH.PayementTerm = new CodeBaseResponse();
            OH.PayementTerm.CodeKey = orderDetails.OrderPaymentTerm.CodeKey;
            OH.DocumentNumber = orderDetails.OrderDocumentNumber;
            OH.DiscountPercentage = orderDetails.HeaderLevelDisountPrecentage;
            OH.OrderAdress = orderDetails.OrderCustomer;
            OH.RepAdress = orderDetails.OrderRepAddress;
            OH.IsActive = orderDetails.IsActive;
            OH.IsApproved = orderDetails.IsApproved;
            OH.OrderNumber = Convert.ToInt32(orderDetails.OrderNumber);
            OH.BussinessUnit = orderDetails.BussinessUnit;
            OH.ObjectKey = orderDetails.FormObjectKey;
            OH.ApproveStatus = orderDetails.OrderApproveState;
            OH.AccountKey = 1;
            OH.Description = orderDetails.HeaderDescription;
            OH.OrderCategory1 = new CodeBaseResponse() { CodeKey= (int)orderDetails.OrderCategory1.CodeKey };
            OH.OrderCategory2 = new CodeBaseResponse() { CodeKey = (int)orderDetails.OrderCategory2.CodeKey };
            OH.OrderCategory3 = new CodeBaseResponse() { CodeKey = (int)orderDetails.OrderCategory3.CodeKey };
            OH.ProjectKey = (int)orderDetails.OrderProject.ProjectKey;
            OH.Code1Key = orderDetails.Cd1Ky;
            OH.OrderStatus = orderDetails.OrderStatus;
            OH.MeterReading = orderDetails.MeterReading;
            OH.DeliveryDate = orderDetails.DeliveryDate;
            OH.FromOrderKey = orderDetails.FromOrderKey;

            if (!BaseComboResponse.IsEntityWithDefaultValue(orderDetails.OrderAccount))
            {
                OH.AccountKey = orderDetails.OrderAccount.AccountKey;
            }
            else
            {
                var acc = _unitOfWork.AccountRepository.GetAccountByAddress(orderDetails.OrderCustomer, company, user);
                OH.AccountKey = acc.Value.AccountKey;
            }

            _unitOfWork.OrderRepository.UpdateGenericOrderHeader(OH, company, user);

            if (orderDetails.BaringHeaderCompanyAccount.AccountKey > 11)
            {
                WorkOrderAmountByAccount company_accdet = new WorkOrderAmountByAccount()
                {
                    OrderHeaderAccountKey=orderDetails.OrderHeaderAccountKey,
                    OrderKey = OH.OrderKey,
                    ObjectKey = orderDetails.FormObjectKey,
                    Account = orderDetails.BaringHeaderCompanyAccount,
                    Address = orderDetails.OrderCustomer,
                    ControlConKey = orderDetails.OrderControlCondition.CodeKey,
                    LineNumber = 1,
                    Value = orderDetails.CompanyPercentage,
                    Amount = orderDetails.CompanyAmount
                };
                _unitOfWork.OrderRepository.OrderHeaderAccountInsertUpdate(company, user, company_accdet);
            }

            if (orderDetails.BaringHeaderPrincipleAccount.AccountKey > 11)
            {
                WorkOrderAmountByAccount principle_accdet = new WorkOrderAmountByAccount()
                {
                    OrderHeaderAccountKey = orderDetails.OrderHeaderAccountKey,
                    OrderKey = OH.OrderKey,
                    ObjectKey = orderDetails.FormObjectKey,
                    Account = orderDetails.BaringHeaderPrincipleAccount,
                    Address = orderDetails.OrderCustomer,
                    ControlConKey = orderDetails.OrderControlCondition.CodeKey,
                    LineNumber = 2,
                    Value = orderDetails.PrincipalPercentage,
                    Amount = orderDetails.PrincipalAmount
                };
                _unitOfWork.OrderRepository.OrderHeaderAccountInsertUpdate(company, user, principle_accdet);
            }
            if (orderDetails.OrderAccount.AccountKey > 11)
            {
                WorkOrderAmountByAccount cus_accdet = new WorkOrderAmountByAccount()
                {
                    OrderHeaderAccountKey = orderDetails.OrderHeaderAccountKey,
                    OrderKey = OH.OrderKey,
                    ObjectKey = orderDetails.FormObjectKey,
                    Account = orderDetails.OrderAccount,
                    Address = orderDetails.OrderCustomer,
                    ControlConKey = orderDetails.OrderControlCondition.CodeKey,
                    LineNumber = 3,
                    Value = orderDetails.CustomerPrecentage,
                    Amount = orderDetails.CustomerAmount
                };
                _unitOfWork.OrderRepository.OrderHeaderAccountInsertUpdate(company, user, cus_accdet);
            }


            foreach (GenericOrderItem item in orderDetails.OrderItems)
            {
                OrderLineCreateDTO lineItem = new OrderLineCreateDTO();

                if (item.FromOrderDetailKey > 1)
                {

                    lineItem.OrderKey = OH.OrderKey;
                    lineItem.OrderDate = OH.OrderDate;
                    lineItem.FromOrderDetailKey = item.FromOrderDetailKey;
                    lineItem.OrderType = new CodeBaseResponse();
                    lineItem.OrderType.CodeKey = OH.OrderType.CodeKey;// item.OrderType.CodeKey;
                    lineItem.TransactionQuantity = item.TransactionQuantity;
                    lineItem.Rate = _unitOfWork.ItemRepository.GetCostPriceByLocAndItmKy(company, new CodeBaseResponse((int)orderDetails.OrderLocation.CodeKey), DateTime.Now, item.TransactionItem.ItemKey);
                    lineItem.TransactionRate = item.TransactionRate;
                    lineItem.AddressKey = OH.AddressKey;
                    lineItem.IsActive = item.IsActive;
                    lineItem.IsApproved = item.IsApproved;
                    lineItem.OrderLineLocation = new CodeBaseResponse();
                    lineItem.OrderLineLocation.CodeKey = orderDetails.OrderLocation.CodeKey;
                    lineItem.BussinessUnitKey = (int)item.BussinessUnit.CodeKey;
                    lineItem.AccountKey = OH.AccountKey;
                    lineItem.LineNumber = item.LineNumber;
                    lineItem.ItemKey = item.TransactionItem != null ? item.TransactionItem.ItemKey : 1;
                    lineItem.OrderItemName = item.TransactionItem != null ? item.TransactionItem.ItemName : "";
                    lineItem.Rate = item.Rate;
                    lineItem.TransactionRate = item.TransactionRate;
                    lineItem.DiscountPercentage = item.DiscountPercentage;
                    lineItem.DisocuntAmount = item.DiscountAmount;
                    lineItem.ObjectKey = orderDetails.FormObjectKey;
                    lineItem.CompanyKey = company.CompanyKey;
                    lineItem.UserKey = user.UserKey;
                    lineItem.OriginalQuantity = item.RequestedQuantity;
                    lineItem.IsTransfer = item.IsTransfer;
                    lineItem.IsConfirmed = item.IsTransferConfirmed;

                    lineItem.ItemTaxType1 = item.ItemTaxType1;
                    lineItem.ItemTaxType2 = item.ItemTaxType2;
                    lineItem.ItemTaxType3 = item.ItemTaxType3;
                    lineItem.ItemTaxType4 = item.ItemTaxType4;
                    lineItem.ItemTaxType5 = item.ItemTaxType5;

                    lineItem.ItemTaxType1Per = item.ItemTaxType1Per;
                    lineItem.ItemTaxType2Per = item.ItemTaxType2Per;
                    lineItem.ItemTaxType3Per = item.ItemTaxType3Per;
                    lineItem.ItemTaxType4Per = item.ItemTaxType4Per;
                    lineItem.ItemTaxType5Per = item.ItemTaxType5Per;
                    lineItem.ProjectKey= (int)orderDetails.OrderProject.ProjectKey;
                    lineItem.Description = item.Description;
                    lineItem.ReserveAddressKey = (int)item.ResourceAddress.AddressKey;
                    lineItem.OrderDetailsAccountKey = item.OrderDetailsAccountKey;
                   // lineItem.FrmOrdDetKy = item.FromOrderDetKy;



                    _unitOfWork.OrderRepository.UpdateGenericOrderLineItem(lineItem, orderDetails.FormObjectKey, company, user);
                }
                else
                {
                    lineItem.ItemKey = item.TransactionItem.ItemKey;
                    lineItem.OrderKey = OH.OrderKey;
                    lineItem.TransactionQuantity = item.TransactionQuantity;
                    lineItem.Rate = _unitOfWork.ItemRepository.GetCostPriceByLocAndItmKy(company, new CodeBaseResponse((int)orderDetails.OrderLocation.CodeKey), DateTime.Now, item.TransactionItem.ItemKey);
                    lineItem.TransactionRate = item.TransactionRate;
                    lineItem.AddressKey = OH.AddressKey;
                    lineItem.ObjectKey = orderDetails.FormObjectKey;
                    lineItem.OrderLineLocation = new CodeBaseResponse();
                    lineItem.OrderLineLocation.CodeKey = item.OrderLineLocation.CodeKey;
                    lineItem.CompanyKey = company.CompanyKey;
                    lineItem.UserKey = user.UserKey;
                    lineItem.DiscountPercentage = item.DiscountPercentage;
                    lineItem.AccountKey = OH.AccountKey;
                    lineItem.LineNumber = item.LineNumber;
                    lineItem.IsActive = item.IsActive;
                    lineItem.OriginalQuantity = item.RequestedQuantity;
                    lineItem.IsApproved = 1;
                    lineItem.OrderType = new CodeBaseResponse();
                    lineItem.OrderType.CodeKey = orderDetails.OrderType.CodeKey;
                    lineItem.IsTransfer = item.IsTransfer;
                    lineItem.IsConfirmed = item.IsTransferConfirmed;
                    lineItem.DisocuntAmount = item.DiscountAmount;
                    lineItem.TransactionDiscountAmount = item.DiscountAmount;
                    lineItem.ItemTaxType1 = item.ItemTaxType1;
                    lineItem.ItemTaxType2 = item.ItemTaxType2;
                    lineItem.ItemTaxType3 = item.ItemTaxType3;
                    lineItem.ItemTaxType4 = item.ItemTaxType4;
                    lineItem.ItemTaxType5 = item.ItemTaxType5;

                    lineItem.ItemTaxType1Per = item.ItemTaxType1Per;
                    lineItem.ItemTaxType2Per = item.ItemTaxType2Per;
                    lineItem.ItemTaxType3Per = item.ItemTaxType3Per;
                    lineItem.ItemTaxType4Per = item.ItemTaxType4Per;
                    lineItem.ItemTaxType5Per = item.ItemTaxType5Per;
                    lineItem.ProjectKey = (int)orderDetails.OrderProject.ProjectKey;
                    lineItem.BussinessUnitKey = (int)item.BussinessUnit.CodeKey;
                    lineItem.Description = item.Description;
                    lineItem.ReserveAddressKey = (int)item.ResourceAddress.AddressKey;
                    lineItem.OrderDetailsAccountKey = item.OrderDetailsAccountKey;
                    //lineItem.FrmOrdDetKy = item.FromOrderDetKy;

                    _unitOfWork.OrderRepository.CreateOrderLineItem(lineItem, company, user, new UIObject() { ObjectId = orderDetails.FormObjectKey });
                }

                if (item.BaringCompany.AccountKey > 11)
                {
                    WorkOrderAmountByAccount company_accdet = new WorkOrderAmountByAccount()
                    {
                        OrderDetailsAccountKey=lineItem.OrderDetailsAccountKey,
                        FromOrderDetailKey = lineItem.OrderLineItemKey,
                        ObjectKey = lineItem.ObjectKey,
                        Account = item.BaringCompany,
                        Address = new AddressResponse() { AddressKey = lineItem.AddressKey },
                        ControlConKey = orderDetails.OrderControlCondition.CodeKey,
                        LineNumber = 1,
                        Value = item.CompanyPrecentage,
                        Amount = item.CompanyAmount
                    };
                    _unitOfWork.OrderRepository.OrderDetailAccountInsertUpdate(company, user, company_accdet);
                }

                if (item.BaringPrinciple.AccountKey > 11)
                {
                    WorkOrderAmountByAccount principle_accdet = new WorkOrderAmountByAccount()
                    {
                        OrderDetailsAccountKey = lineItem.OrderDetailsAccountKey,
                        FromOrderDetailKey = lineItem.OrderLineItemKey,
                        ObjectKey = lineItem.ObjectKey,
                        Account = item.BaringPrinciple,
                        Address = new AddressResponse() { AddressKey = lineItem.AddressKey },
                        ControlConKey = orderDetails.OrderControlCondition.CodeKey,
                        LineNumber = 2,
                        Value = item.PrinciplePrecentage,
                        Amount = item.PrincipleAmount
                    };
                    _unitOfWork.OrderRepository.OrderDetailAccountInsertUpdate(company, user, principle_accdet);
                }
                if (item.BaringCustomer.AccountKey > 11)
                {
                    WorkOrderAmountByAccount cus_accdet = new WorkOrderAmountByAccount()
                    {
                        OrderDetailsAccountKey = lineItem.OrderDetailsAccountKey,
                        FromOrderDetailKey = lineItem.OrderLineItemKey,
                        ObjectKey = lineItem.ObjectKey,
                        Account = item.BaringCustomer,
                        Address = new AddressResponse() { AddressKey = lineItem.AddressKey },
                        ControlConKey = orderDetails.OrderControlCondition.CodeKey,
                        LineNumber =3,
                        Value = item.CustomerPrecentage,
                        Amount = item.CustomerAmount
                    };
                    _unitOfWork.OrderRepository.OrderDetailAccountInsertUpdate(company, user, cus_accdet);
                }
            }

            _unitOfWork.OrderRepository.PostInterLocationTransfers(company, user, OH.OrderKey, orderDetails.FormObjectKey);

            var ordb = _unitOfWork.OrderRepository.GetGenericOrderByOrderKeyV2(OH.OrderKey, company, user);
            OrderSaveResponse orderServerResponse = new OrderSaveResponse();
            orderServerResponse.OrderKey = ordb.Value.OrderKey;
            orderServerResponse.OrderNumber = ordb.Value.OrderNumber.ToString();
            orderServerResponse.Prefix = ordb.Value.OrderPrefix.CodeName;



            return orderServerResponse;
        }

        public BaseServerResponse<OrderSaveResponse> UpdateIRNOrder(Company company, User user, GenericOrder orderDetails) 
        {
            OrderHeaderEditDTO OH = new OrderHeaderEditDTO();
            OH.OrderKey = orderDetails.OrderKey;
            OH.OrderLocation = new CodeBaseResponse();
            OH.OrderLocation.CodeKey = orderDetails.OrderLocation.CodeKey;
            OH.Location2 = new CodeBaseResponse();
            OH.Location2.CodeKey = orderDetails.OrderLocation.CodeKey;
            OH.OrderDate = orderDetails.OrderDate;
            OH.OrderType = new CodeBaseResponse();
            OH.OrderType.CodeKey = orderDetails.OrderType.CodeKey;
            OH.PayementTerm = new CodeBaseResponse();
            OH.PayementTerm.CodeKey = orderDetails.OrderPaymentTerm.CodeKey;
            OH.DocumentNumber = orderDetails.OrderDocumentNumber;
            OH.DiscountPercentage = orderDetails.HeaderLevelDisountPrecentage;
            OH.OrderAdress = orderDetails.OrderCustomer;
            OH.RepAdress = orderDetails.OrderRepAddress;
            OH.IsActive = orderDetails.IsActive;
            OH.IsApproved = orderDetails.IsApproved;
            OH.OrderNumber = Convert.ToInt32(orderDetails.OrderNumber);
            OH.BussinessUnit = orderDetails.BussinessUnit;
            OH.ObjectKey = orderDetails.FormObjectKey;
            OH.ApproveStatus = orderDetails.OrderApproveState;
            OH.AccountKey = 1;
            OH.Description = orderDetails.HeaderDescription;
            OH.OrderCategory1 = new CodeBaseResponse() { CodeKey = (int)orderDetails.OrderCategory1.CodeKey };
            OH.OrderCategory2 = new CodeBaseResponse() { CodeKey = (int)orderDetails.OrderCategory2.CodeKey };
            OH.ProjectKey = (int)orderDetails.OrderProject.ProjectKey;
            OH.Code1Key = orderDetails.Cd1Ky;
            OH.OrderStatus = orderDetails.OrderStatus;
            OH.MeterReading = orderDetails.MeterReading;
            OH.DeliveryDate = orderDetails.DeliveryDate;
            OH.Insurance = new AccountResponse();
            OH.AccountKey = orderDetails.OrderAccount.AccountKey;

            //if (!BaseComboResponse.IsEntityWithDefaultValue(orderDetails.OrderAccount))
            //{
            //    OH.AccountKey = orderDetails.OrderAccount.AccountKey;
            //}
            //else
            //{
            //    var acc = _unitOfWork.AccountRepository.GetAccountByAddress(orderDetails.OrderCustomer, company, user);
            //    OH.AccountKey = acc.Value.AccountKey;
            //}

            _unitOfWork.OrderRepository.UpdateGenericOrderHeader(OH, company, user);

            foreach (GenericOrderItem item in orderDetails.OrderItems)
            {
                OrderLineCreateDTO lineItem = new OrderLineCreateDTO();
                if (item.FromOrderDetailKey > 1)
                {

                    lineItem.OrderKey = OH.OrderKey;
                    lineItem.FromOrderDetailKey = item.FromOrderDetailKey;
                    lineItem.OrderType = new CodeBaseResponse();
                    lineItem.OrderType.CodeKey = item.OrderType.CodeKey;
                    lineItem.TransactionQuantity = item.TransactionQuantity;
                    lineItem.Rate = _unitOfWork.ItemRepository.GetCostPriceByLocAndItmKy(company, new CodeBaseResponse((int)orderDetails.OrderLocation.CodeKey), DateTime.Now, item.TransactionItem.ItemKey);
                    lineItem.TransactionRate = item.TransactionRate;
                    lineItem.AddressKey = OH.AddressKey;
                    lineItem.IsActive = item.IsActive;
                    lineItem.IsApproved = item.IsApproved;
                    lineItem.OrderLineLocation = new CodeBaseResponse();
                    lineItem.OrderLineLocation.CodeKey = item.OrderLineLocation.CodeKey;
                    lineItem.BussinessUnitKey = (int)item.BussinessUnit.CodeKey;
                    lineItem.AccountKey = OH.AccountKey;
                    lineItem.LineNumber = item.LineNumber;
                    lineItem.ItemKey = item.TransactionItem != null ? item.TransactionItem.ItemKey : 1;
                    lineItem.OrderItemName = item.TransactionItem != null ? item.TransactionItem.ItemName : "";
                    lineItem.Rate = item.Rate;
                    lineItem.TransactionRate = item.TransactionRate;
                    lineItem.DiscountPercentage = item.DiscountPercentage;
                    lineItem.DisocuntAmount = item.DiscountAmount;
                    lineItem.ObjectKey = orderDetails.FormObjectKey;
                    lineItem.CompanyKey = company.CompanyKey;
                    lineItem.UserKey = user.UserKey;
                    lineItem.OriginalQuantity = item.RequestedQuantity;
                    lineItem.IsTransfer = item.IsTransfer;
                    lineItem.IsConfirmed = item.IsTransferConfirmed;

                    lineItem.ItemTaxType1 = item.ItemTaxType1;
                    lineItem.ItemTaxType2 = item.ItemTaxType2;
                    lineItem.ItemTaxType3 = item.ItemTaxType3;
                    lineItem.ItemTaxType4 = item.ItemTaxType4;
                    lineItem.ItemTaxType5 = item.ItemTaxType5;

                    lineItem.ItemTaxType1Per = item.ItemTaxType1Per;
                    lineItem.ItemTaxType2Per = item.ItemTaxType2Per;
                    lineItem.ItemTaxType3Per = item.ItemTaxType3Per;
                    lineItem.ItemTaxType4Per = item.ItemTaxType4Per;
                    lineItem.ItemTaxType5Per = item.ItemTaxType5Per;
                    lineItem.ProjectKey = (int)orderDetails.OrderProject.ProjectKey;
                    lineItem.Description = item.Description;
					lineItem.ReserveAddressKey = (int)item.ResourceAddress.AddressKey;
					lineItem.AnalysisType2.CodeKey = item.AnalysisType2.CodeKey;
                    lineItem.AnalysisType4 = item.AnalysisType4;

					_unitOfWork.OrderRepository.UpdateGenericOrderLineItem(lineItem, orderDetails.FormObjectKey, company, user);
                }
                else
                {
                    lineItem.ItemKey = item.TransactionItem.ItemKey;
                    lineItem.OrderKey = OH.OrderKey;
                    lineItem.TransactionQuantity = item.TransactionQuantity;
                    lineItem.Rate = _unitOfWork.ItemRepository.GetCostPriceByLocAndItmKy(company, new CodeBaseResponse((int)orderDetails.OrderLocation.CodeKey), DateTime.Now, item.TransactionItem.ItemKey);
                    lineItem.TransactionRate = item.TransactionRate;
                    lineItem.AddressKey = OH.AddressKey;
                    lineItem.ObjectKey = orderDetails.FormObjectKey;
                    lineItem.OrderLineLocation = new CodeBaseResponse();
                    lineItem.OrderLineLocation.CodeKey = item.OrderLineLocation.CodeKey;
                    lineItem.CompanyKey = company.CompanyKey;
                    lineItem.UserKey = user.UserKey;
                    lineItem.DiscountPercentage = item.DiscountPercentage;
                    lineItem.AccountKey = OH.AccountKey;
                    lineItem.LineNumber = item.LineNumber;
                    lineItem.IsActive = 1;
                    lineItem.OriginalQuantity = item.RequestedQuantity;
                    lineItem.IsApproved = 1;
                    lineItem.OrderType = new CodeBaseResponse();
                    lineItem.OrderType.CodeKey = orderDetails.OrderType.CodeKey;
                    lineItem.IsTransfer = item.IsTransfer;
                    lineItem.IsConfirmed = item.IsTransferConfirmed;
                    lineItem.DisocuntAmount = item.DiscountAmount;
                    lineItem.TransactionDiscountAmount = item.DiscountAmount;
                    lineItem.ItemTaxType1 = item.ItemTaxType1;
                    lineItem.ItemTaxType2 = item.ItemTaxType2;
                    lineItem.ItemTaxType3 = item.ItemTaxType3;
                    lineItem.ItemTaxType4 = item.ItemTaxType4;
                    lineItem.ItemTaxType5 = item.ItemTaxType5;

                    lineItem.ItemTaxType1Per = item.ItemTaxType1Per;
                    lineItem.ItemTaxType2Per = item.ItemTaxType2Per;
                    lineItem.ItemTaxType3Per = item.ItemTaxType3Per;
                    lineItem.ItemTaxType4Per = item.ItemTaxType4Per;
                    lineItem.ItemTaxType5Per = item.ItemTaxType5Per;
                    lineItem.ProjectKey = (int)orderDetails.OrderProject.ProjectKey;
                    lineItem.BussinessUnitKey = (int)item.BussinessUnit.CodeKey;
                    lineItem.Description = item.Description;
					lineItem.ReserveAddressKey = (int)item.ResourceAddress.AddressKey;
                    lineItem.AnalysisType2.CodeKey = item.AnalysisType2.CodeKey;
                    lineItem.AnalysisType4 = item.AnalysisType4;

                    _unitOfWork.OrderRepository.CreateOrderLineItem(lineItem, company, user, new UIObject() { ObjectId = orderDetails.FormObjectKey });
                }

                if (item.BaringCompany.AccountKey > 11)
                {
                    WorkOrderAmountByAccount company_accdet = new WorkOrderAmountByAccount()
                    {
                        FromOrderDetailKey = lineItem.OrderLineItemKey,
                        ObjectKey = lineItem.ObjectKey,
                        Account = item.BaringCompany,
                        Address = new AddressResponse() { AddressKey = lineItem.AddressKey },
                        ControlConKey = orderDetails.OrderControlCondition.CodeKey,
                        LineNumber = (int)lineItem.LineNumber,
                        Value = item.CompanyPrecentage,
                        Amount = item.CompanyAmount
                    };
                    _unitOfWork.OrderRepository.OrderDetailAccountInsertUpdate(company, user, company_accdet);
                }

                if (item.BaringPrinciple.AccountKey > 11)
                {
                    WorkOrderAmountByAccount principle_accdet = new WorkOrderAmountByAccount()
                    {
                        FromOrderDetailKey = lineItem.OrderLineItemKey,
                        ObjectKey = lineItem.ObjectKey,
                        Account = item.BaringPrinciple,
                        Address = new AddressResponse() { AddressKey = lineItem.AddressKey },
                        ControlConKey = orderDetails.OrderControlCondition.CodeKey,
                        LineNumber = (int)(lineItem.LineNumber + 1),
                        Value = item.PrinciplePrecentage,
                        Amount = item.PrincipleAmount
                    };
                    _unitOfWork.OrderRepository.OrderDetailAccountInsertUpdate(company, user, principle_accdet);
                }
            }

            _unitOfWork.OrderRepository.PostInterLocationTransfers(company, user, OH.OrderKey, orderDetails.FormObjectKey);

            var ordb = _unitOfWork.OrderRepository.GetGenericOrderByOrderKeyV2(OH.OrderKey, company, user);
            OrderSaveResponse orderServerResponse = new OrderSaveResponse();
            orderServerResponse.OrderKey = ordb.Value.OrderKey;
            orderServerResponse.OrderNumber = ordb.Value.OrderNumber.ToString();
            orderServerResponse.Prefix = ordb.Value.OrderPrefix.CodeName;

            BaseServerResponse<OrderSaveResponse> orderSaveResponse = new BaseServerResponse<OrderSaveResponse>();
            orderSaveResponse.Value = orderServerResponse;

            return orderSaveResponse;

        }

        public BaseServerResponse<WorkOrder> OpenWorkOrder(Company company, User user, OrderOpenRequest request)
        {
            int controlConKy = 0;
            var ord = _unitOfWork.OrderRepository.GetGenericOrderByOrderKeyV2(request.OrderKey, company, user);
            OrderHeaderEditDTO responses = ord.Value;

            var ordDet = _unitOfWork.OrderRepository.GetGenericOrderLineItemsByOrderKey(request.OrderKey, responses.ObjectKey, company, user);
            IList<OrderLineCreateDTO> itemList = ordDet.Value;

            WorkOrder order = new WorkOrder();
            order.OrderLocation = responses.OrderLocation;
            order.OrderCustomer = responses.OrderAdress;
            order.OrderRepAddress = responses.RepAdress;
            order.OrderAccount = new AccountResponse() { AccountKey=responses.AccountKey};
            order.HeaderLevelDisountPrecentage = responses.DiscountPercentage;
            order.OrderKey = responses.OrderKey;
            order.OrderNumber = responses.OrderNumber.ToString();
            order.OrderDocumentNumber = responses.DocumentNumber;
            order.OrderDate = responses.OrderDate;
            order.OrderPaymentTerm = responses.PayementTerm;
            order.OrderType = responses.OrderType;
            order.FormObjectKey = responses.ObjectKey;
            order.IsActive = responses.IsActive;
            order.IsApproved = responses.IsApproved;
            order.HeaderDescription = responses.Description;
            order.OrderPrefix = responses.OrderPrefix;
            order.OrderApproveState = _unitOfWork.OrderRepository.OrderApproveStatusFindByOrdKy(company, user, order.FormObjectKey, order.OrderKey);
            order.OrderCategory1 = responses.OrderCategory1;    
            order.OrderCategory2 = responses.OrderCategory2;
            order.OrderCategory3= responses.OrderCategory3;
            order.OrderProject=new ProjectResponse() { ProjectKey=responses.ProjectKey};
            order.OrderStatus = responses.OrderStatus;
            order.MeterReading=responses.MeterReading;
            order.DeliveryDate=responses.DeliveryDate;

            var concode = _unitOfWork.CodeBaseRepository.GetControlConditionCode(company, user, (int)request.ObjKy, "OrdDetAcc");
            controlConKy = (int)concode.Value.CodeKey;

            WorkOrderAmountByAccount accdet = new WorkOrderAmountByAccount()
            {
                OrderKey = order.OrderKey,
                ObjectKey = (int)request.ObjKy,
                ControlConKey = controlConKy,

            };

            var hdr = _unitOfWork.OrderRepository.OrderHeaderAccountSelect(company, user, accdet);
            IList<WorkOrderAmountByAccount> hdr_list = hdr.Value;

            WorkOrderAmountByAccount? hdracc1 = hdr_list.Where(x => x.LineNumber == 1).FirstOrDefault();
            WorkOrderAmountByAccount? hdracc2 = hdr_list.Where(x => x.LineNumber == 2).FirstOrDefault();
            WorkOrderAmountByAccount? hdracc3 = hdr_list.Where(x => x.LineNumber == 3).FirstOrDefault();

            if (hdr_list.Count > 0)
            {
                order.OrderHeaderAccountKey = hdracc1 != null ? hdracc1.OrderHeaderAccountKey : 1;
                order.BaringHeaderCompanyAccount = new AccountResponse() { AccountKey = hdracc1 != null ? hdracc1.Account.AccountKey : 1 };
                order.CompanyPercentage = hdracc1 != null ? hdracc1.Value : 0;
                order.CompanyAmount = hdracc1 != null ? hdracc1.Amount : 0;
                order.BaringHeaderPrincipleAccount = new AccountResponse() { AccountKey = hdracc2 != null ? hdracc2.Account.AccountKey : 1 };
                order.PrincipalPercentage = hdracc2 != null ? hdracc2.Value : 0;
                order.PrincipalAmount = hdracc2 != null ? hdracc2.Amount : 0;
                order.CustomerPrecentage = hdracc3 != null ? hdracc3.Value : 0;
                order.CustomerAmount = hdracc3 != null ? hdracc3.Amount : 0;
            }

            foreach (OrderLineCreateDTO item in itemList)
            {
                    GenericOrderItem lineItem = new GenericOrderItem();
                    lineItem.TransactionItem = new ItemResponse() { 
                                                        ItemKey = item.ItemKey, 
                                                        ItemName = item.OrderItemName,
                                                        ItemType=new CodeBaseResponse() { CodeKey=item.ItemTypeKey,Code=item.ItemTypeOurCode,CodeName=item.ItemTypeName}
                                                };
                    lineItem.TransactionQuantity = item.TransactionQuantity;
                    lineItem.Rate = _unitOfWork.ItemRepository.GetCostPriceByLocAndItmKy(company, new CodeBaseResponse(item.OrderLineLocation != null ? (int)item.OrderLineLocation.CodeKey : 1), DateTime.Now, item.ItemKey);
                    lineItem.TransactionRate = item.TransactionRate;
                    lineItem.ObjectKey = item.ObjectKey;
                    lineItem.OrderLineLocation = item.OrderLineLocation;
                    lineItem.CompanyKey = company.CompanyKey;
                    lineItem.UserKey = user.UserKey;
                    lineItem.DiscountPercentage = item.DiscountPercentage;
                    lineItem.LineNumber = item.LineNumber;
                    lineItem.IsActive = item.IsActive;
                    lineItem.RequestedQuantity = item.OriginalQuantity;
                    lineItem.IsApproved = item.IsApproved;
                    lineItem.IsTransfer = item.IsTransfer;
                    lineItem.IsTransferConfirmed = item.IsConfirmed;
                    lineItem.DiscountAmount = item.DisocuntAmount;
                    lineItem.ItemTaxType1 = item.ItemTaxType1;
                    lineItem.ItemTaxType2 = item.ItemTaxType2;
                    lineItem.ItemTaxType3 = item.ItemTaxType3;
                    lineItem.ItemTaxType4 = item.ItemTaxType4;
                    lineItem.ItemTaxType5 = item.ItemTaxType5;
                    lineItem.ItemTaxType1Per = item.ItemTaxType1Per;
                    lineItem.ItemTaxType2Per = item.ItemTaxType2Per;
                    lineItem.ItemTaxType3Per = item.ItemTaxType3Per;
                    lineItem.ItemTaxType4Per = item.ItemTaxType4Per;
                    lineItem.ItemTaxType5Per = item.ItemTaxType5Per;
                    lineItem.FromOrderDetailKey = item.FromOrderDetailKey;
                    lineItem.Remark = item.Remarks;
                    lineItem.Description = item.Description;
                    lineItem.TransactionUnit = new UnitResponse() { UnitKey=item.UnitKey,UnitName=item.TransactionUnitName};
                    lineItem.ResourceAddress = new AddressResponse() { AddressKey=item.ReserveAddressKey,AddressName=item.ReserveAddressID};
                    lineItem.InsertDate = item.InsertDate;
                    lineItem.UpdateDate = item.UpdateDate;
                    lineItem.AnalysisType2 = item.AnalysisType2;
				    lineItem.AnalysisType4 = item.AnalysisType4;

				    

                    WorkOrderAmountByAccount company_accdet = new WorkOrderAmountByAccount()
                    {
                        FromOrderDetailKey = lineItem.FromOrderDetailKey,
                        ObjectKey = lineItem.ObjectKey,
                        ControlConKey = controlConKy,
                        
                    };

                   var det=_unitOfWork.OrderRepository.OrderDetailAccountSelect(company,user, company_accdet);
                    IList<WorkOrderAmountByAccount> det_list = det.Value;

                    WorkOrderAmountByAccount? acc1 = det_list.Where(x => x.LineNumber == 1).FirstOrDefault();
                    WorkOrderAmountByAccount? acc2 = det_list.Where(x => x.LineNumber == 2).FirstOrDefault();
                    WorkOrderAmountByAccount? acc3 = det_list.Where(x => x.LineNumber == 3).FirstOrDefault();

                    if (det_list.Count>0)
                    {
                        lineItem.OrderDetailsAccountKey = acc1 != null? acc1.OrderDetailsAccountKey:1;
                        lineItem.BaringCompany = new AccountResponse() { AccountKey = acc1 != null ? acc1.Account.AccountKey:1 };
                        lineItem.CompanyPrecentage = acc1 != null ? acc1.Value:0;
                        lineItem.CompanyAmount = acc1 != null ? acc1.Amount:0;
                        lineItem.BaringPrinciple = new AccountResponse() { AccountKey = acc2 != null ? acc2.Account.AccountKey:1 };
                        lineItem.PrinciplePrecentage = acc2 != null ? acc2.Value:0;
                        lineItem.PrincipleAmount = acc2 != null ? acc2.Amount:0;
                        lineItem.BaringCustomer = new AccountResponse() { AccountKey = acc3 != null ? acc3.Account.AccountKey : 1 };
                        lineItem.CustomerPrecentage = acc3 != null ? acc3.Value : 0;
                        lineItem.CustomerAmount = acc3 != null ? acc3.Amount : 0;
                    }
                    

                    order.OrderItems.Add(lineItem);
                

            }

            BaseServerResponse<WorkOrder> response = new BaseServerResponse<WorkOrder>();
            response.Value = order;

            return response;
        }

        public IList<BookingDetails> GetRecentBooking(Vehicle request, Company company, User user)
        {
            var response = _unitOfWork.WorkShopManagementRepository.RecentBookingDetails(request, company, user);
            return response.Value;
        }

        public BaseServerResponse<BLTransaction> SaveWorkOrderTransaction(BLTransaction transaction, Company company, User user, UIObject uIObject)
        {
            BaseServerResponse<BLTransaction> response = new BaseServerResponse<BLTransaction>();
            BLTransaction TrnResponse = new BLTransaction();
            if (BaseComboResponse.GetKeyValue(transaction.Address) < 11)
            {
                var address = _unitOfWork.AccountRepository.GetAddressByAccount(company, user, transaction.Account.AccountKey);
                if (address != null)
                    transaction.Address = address.Value;
            }

            if (!transaction.IsPersisted)
            {
                response = _unitOfWork.TransactionRepository.SaveGenericTransaction(company, user, transaction);
                
            }
            else 
            {
                response=_unitOfWork.TransactionRepository.UpdateGenericTransaction(company, user, transaction);
            }

            if (transaction.BaringCompany.AccountKey > 11)
            {
                WorkOrderAmountByAccount company_accdet = new WorkOrderAmountByAccount()
                {
                    TransactionHeaderAccountKey = transaction.TransactionHeaderAccountKey,
                    TransactionKey = (int)transaction.TransactionKey,
                    ObjectKey = transaction.ElementKey,
                    Account = transaction.BaringCompany,
                    Address = transaction.Address,
                    ControlConKey = transaction.TransactionControlCondition.CodeKey,
                    LineNumber = 1,
                    Value = transaction.CompanyPrecentage,
                    Amount = transaction.CompanyAmount
                };
                _unitOfWork.TransactionRepository.TrnHeaderAccountInsertUpdate(company, user, company_accdet);
            }

            if (transaction.BaringPrinciple.AccountKey > 11)
            {
                WorkOrderAmountByAccount principle_accdet = new WorkOrderAmountByAccount()
                {
                    TransactionHeaderAccountKey = transaction.TransactionHeaderAccountKey,
                    TransactionKey = (int)transaction.TransactionKey,
                    ObjectKey = transaction.ElementKey,
                    Account = transaction.BaringPrinciple,
                    Address = transaction.Address,
                    ControlConKey = transaction.TransactionControlCondition.CodeKey,
                    LineNumber = 2,
                    Value = transaction.PrinciplePrecentage,
                    Amount = transaction.PrincipleAmount
                };
                _unitOfWork.TransactionRepository.TrnHeaderAccountInsertUpdate(company, user, principle_accdet);
            }
            if (transaction.BaringCustomer.AccountKey > 11)
            {
                WorkOrderAmountByAccount cus_accdet = new WorkOrderAmountByAccount()
                {
                    TransactionHeaderAccountKey = transaction.TransactionHeaderAccountKey,
                    TransactionKey = (int)transaction.TransactionKey,
                    ObjectKey = transaction.ElementKey,
                    Account = transaction.BaringCustomer,
                    Address = transaction.Address,
                    ControlConKey = transaction.TransactionControlCondition.CodeKey,
                    LineNumber = 3,
                    Value = transaction.CustomerPrecentage,
                    Amount = transaction.CustomerAmount
                };
                _unitOfWork.TransactionRepository.TrnHeaderAccountInsertUpdate(company, user, cus_accdet);
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

                if (line.SerialNumbers.Count>0)
                {
                    foreach (ItemSerialNumber serialNumber in line.SerialNumbers)
                    {
                        serialNumber.ItemTransactionKey = line.ItemTransactionKey;
                        serialNumber.ItemKey = line.TransactionItem.ItemKey;
                        serialNumber.PersistingElementKey = transaction.ElementKey;
                        _unitOfWork.TransactionRepository.SaveOrUpdateSerialNumber(company, user, serialNumber);

                        
                    }
                }

                if (line.BaringCompany.AccountKey > 11)
                {
                    WorkOrderAmountByAccount company_accdet = new WorkOrderAmountByAccount()
                    {
                        TransactionDetailsAccountKey=line.TransactionDetailsAccountKey,
                        FromItemTransactionKey = (int)line.ItemTransactionKey,
                        ObjectKey = line.ElementKey,
                        Account = line.BaringCompany,
                        Address = new AddressResponse() { AddressKey = line.Address.AddressKey },
                        ControlConKey = transaction.TransactionControlCondition.CodeKey,
                        LineNumber = 1,
                        Value = line.CompanyPrecentage,
                        Amount = line.CompanyAmount
                    };
                    _unitOfWork.TransactionRepository.TrnDetailAccountInsertUpdate(company, user, company_accdet);
                }

                if (line.BaringPrinciple.AccountKey > 11)
                {
                    WorkOrderAmountByAccount principle_accdet = new WorkOrderAmountByAccount()
                    {
                        TransactionDetailsAccountKey = line.TransactionDetailsAccountKey,
                        FromItemTransactionKey = (int)line.ItemTransactionKey,
                        ObjectKey = line.ElementKey,
                        Account = line.BaringPrinciple,
                        Address = new AddressResponse() { AddressKey = line.Address.AddressKey },
                        ControlConKey = transaction.TransactionControlCondition.CodeKey,
                        LineNumber = 2,
                        Value = line.PrinciplePrecentage,
                        Amount = line.PrincipleAmount
                    };
                    _unitOfWork.TransactionRepository.TrnDetailAccountInsertUpdate(company, user, principle_accdet);
                }
                if (line.BaringCustomer.AccountKey > 11)
                {
                    WorkOrderAmountByAccount cus_accdet = new WorkOrderAmountByAccount()
                    {
                        TransactionDetailsAccountKey = line.TransactionDetailsAccountKey,
                        FromItemTransactionKey = (int)line.ItemTransactionKey,
                        ObjectKey = line.ElementKey,
                        Account = line.BaringCustomer,
                        Address = new AddressResponse() { AddressKey = line.Address.AddressKey },
                        ControlConKey = transaction.TransactionControlCondition.CodeKey,
                        LineNumber = 3,
                        Value = line.CustomerPrecentage,
                        Amount = line.CustomerAmount
                    };
                    _unitOfWork.TransactionRepository.TrnDetailAccountInsertUpdate(company, user, cus_accdet);
                }



            }
            _unitOfWork.TransactionRepository.PostAfterTranSaveActions(company, user, transaction.TransactionKey, transaction.ElementKey);


            return response;
        }
        public BaseServerResponse<BLTransaction> OpenWorkOrderTransaction(Company company, User user, TransactionOpenRequest request)
        {
            var trn= _unitOfWork.TransactionRepository.GenericOpenTransactionV2(company, user, request);
            BLTransaction bltrn = trn.Value;
            CodeBaseResponse appr = _unitOfWork.TransactionRepository.TrnrApproveStatusFindByTrnKy(company,user, (int)request.ElementKey,(int)request.TransactionKey);
            bltrn.ApproveState = appr;

            var concode = _unitOfWork.CodeBaseRepository.GetControlConditionCode(company, user, (int)request.ElementKey, "ItmTrnAcc");
            int controlConKy = (int)concode.Value.CodeKey;

            WorkOrderAmountByAccount company_accdet = new WorkOrderAmountByAccount()
            {
                TransactionKey = (int)bltrn.TransactionKey,
                ObjectKey = request.ElementKey,
                ControlConKey = controlConKy,

            };

            var det = _unitOfWork.TransactionRepository.TransactionHeaderAccountSelect(company, user, company_accdet);
            IList<WorkOrderAmountByAccount> hdr_list = det.Value;

            WorkOrderAmountByAccount? acc1 = hdr_list.Where(x => x.LineNumber == 1).FirstOrDefault();
            WorkOrderAmountByAccount? acc2 = hdr_list.Where(x => x.LineNumber == 2).FirstOrDefault();
            WorkOrderAmountByAccount? acc3 = hdr_list.Where(x => x.LineNumber == 3).FirstOrDefault();

            if (hdr_list.Count > 0)
            {
                bltrn.TransactionHeaderAccountKey = acc1 != null ? acc1.TransactionHeaderAccountKey:1;
                bltrn.BaringCompany = new AccountResponse() { AccountKey = acc1 != null ? acc1.Account.AccountKey : 1 };
                bltrn.CompanyPrecentage = acc1 != null ? acc1.Value : 0;
                bltrn.CompanyAmount = acc1 != null ? acc1.Amount : 0;
                bltrn.BaringPrinciple = new AccountResponse() { AccountKey = acc2 != null ? acc2.Account.AccountKey : 1 };
                bltrn.PrinciplePrecentage = acc2 != null ? acc2.Value : 0;
                bltrn.PrincipleAmount = acc2 != null ? acc2.Amount : 0;
                bltrn.BaringCustomer = new AccountResponse() { AccountKey = acc3 != null ? acc3.Account.AccountKey : 1 };
                bltrn.CustomerPrecentage = acc3 != null ? acc3.Value : 0;
                bltrn.CustomerAmount = acc3 != null ? acc3.Amount : 0;
            }

            return new BaseServerResponse<BLTransaction>() { Value = bltrn, ExecutionStarted = trn.ExecutionStarted, ExecutionEnded = trn.ExecutionEnded, Messages = trn.Messages };
        }
        public BaseServerResponse<IList<GenericTransactionLineItem>> GetWorkOrderTransactionLineItems(Company company, User user, TransactionOpenRequest request)
        {
            var trnDet = _unitOfWork.TransactionRepository.GenericallyGetTransactionLineItemsV2(company, user, request);
            IList<GenericTransactionLineItem> itmlist = trnDet.Value;  

            var concode = _unitOfWork.CodeBaseRepository.GetControlConditionCode(company, user, (int)request.ElementKey, "ItmTrnAcc");
            int controlConKy = (int)concode.Value.CodeKey;

            foreach (var lineItem in itmlist)
            {
                WorkOrderAmountByAccount company_accdet = new WorkOrderAmountByAccount()
                {
                    FromItemTransactionKey = (int)lineItem.ItemTransactionKey,
                    ObjectKey = request.ElementKey,
                    ControlConKey = controlConKy,

                };

                var det = _unitOfWork.TransactionRepository.TransactionDetailAccountSelect(company, user, company_accdet);
                IList<WorkOrderAmountByAccount> det_list = det.Value;

                WorkOrderAmountByAccount? acc1 = det_list.Where(x => x.LineNumber == 1).FirstOrDefault();
                WorkOrderAmountByAccount? acc2 = det_list.Where(x => x.LineNumber == 2).FirstOrDefault();
                WorkOrderAmountByAccount? acc3 = det_list.Where(x => x.LineNumber == 3).FirstOrDefault();

                if (det_list.Count > 0)
                {
                    lineItem.TransactionDetailsAccountKey = acc1 != null ? acc1.TransactionDetailsAccountKey : 1;
                    lineItem.BaringCompany = new AccountResponse() { AccountKey = acc1 != null ? acc1.Account.AccountKey : 1 };
                    lineItem.CompanyPrecentage = acc1 != null ? acc1.Value : 0;
                    lineItem.CompanyAmount = acc1 != null ? acc1.Amount : 0;
                    lineItem.BaringPrinciple = new AccountResponse() { AccountKey = acc2 != null ? acc2.Account.AccountKey : 1 };
                    lineItem.PrinciplePrecentage = acc2 != null ? acc2.Value : 0;
                    lineItem.PrincipleAmount = acc2 != null ? acc2.Amount : 0;
                    lineItem.BaringCustomer = new AccountResponse() { AccountKey = acc3 != null ? acc3.Account.AccountKey : 1 };
                    lineItem.CustomerPrecentage = acc3 != null ? acc3.Value : 0;
                    lineItem.CustomerAmount = acc3 != null ? acc3.Amount : 0;
                }

               
            }
            return new BaseServerResponse<IList<GenericTransactionLineItem>>(){Value= itmlist ,Messages= trnDet.Messages};
        }
        public UserRequestValidation WorkorderValidation(WorkOrder dto, Company company, User user)
        {
            return _unitOfWork.WorkShopManagementRepository.WorkOrderValidation(dto,company,user);
        }

        public IList<WorkOrder> GetIRNBasedOnStatus(WorkOrder dto, Company company, User user)
        {
            var irnList = _unitOfWork.WorkShopManagementRepository.SelectIRNBasedOnStatus(dto,company,user); 
            IList<IRNResponse> IRNsResponseList = irnList.Value;
            IList<WorkOrder> IRNs=new List<WorkOrder>();

            foreach (var itm in IRNsResponseList.ToLookup(x=>x.OrderKey).ToList())
            {
                WorkOrder irn = new WorkOrder();
                irn.OrderKey = (int)(itm.FirstOrDefault()?.OrderKey);
                irn.OrderNumber = itm.FirstOrDefault()?.OrderNumber;
                irn.OrderDate = itm.FirstOrDefault().Insertdate;
                irn.OrderType = itm.FirstOrDefault()?.IRNType;
                irn.SelectedVehicle.VehicleRegistration.ItemCode= itm.FirstOrDefault()?.VehicleID;
                irn.OrderRepAddress = itm.FirstOrDefault()?.ServiceAdvisor;
                irn.BussinessUnit = itm.FirstOrDefault()?.BusinessUnit;
                irn.IsActive = (int)itm.FirstOrDefault()?.HederIsActive;
				irn.Insurance = itm.FirstOrDefault()?.Insurance;
                irn.OrderLocation = itm.FirstOrDefault()?.OrderLocation;
                
                irn.OrderItems=new List<GenericOrderItem>();
                foreach (var i in itm)
                {
                    GenericOrderItem goitm = new GenericOrderItem() {
                        TransactionItem = new ItemResponse()
                        {
                            ItemCode = i.Item.ItemCode,
                            ItemName = i.Item.ItemName,
                            ItemKey = i.Item.ItemKey
                        },
                        TransactionQuantity = i.Quantity,
                        TransactionRate = i.Rate,
                        IsActive = i.IsActive,
                        TransactionUnit=i.TransactionUnit,
                        //DiscountAmount = i.DisocuntAmount,
                        //DiscountPercentage = i.DiscountPercentage,
                        //AnalysisType1 = i.AnalysisType1
                        //amount??
                    };

                    irn.OrderItems.Add(goitm);  
                }

                IRNs.Add(irn);

            }

            return IRNs;
        }

        public OrderSaveResponse CarOrdToOrdPosting(CarOrdToOrdPostingRequest dto, Company company, User user)
        {
           return _unitOfWork.WorkShopManagementRepository.CarOrdToOrdPosting(dto,company,user);
        }

    }
}
