using BlueLotus360.Core.Domain.DTOs;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.Order;
using BlueLotus360.Core.Domain.Entity.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Core.Domain.Entity.WorkOrder
{
    public class WorkOrderAmountByAccount 
    {
        public int OrderDetailsAccountKey { get; set; }
        public int LineNumber { get; set; }
        public int FromOrderDetailKey { get; set; }
        public long ObjectKey { get; set; }
        public AccountResponse Account { get; set; }
        public long ControlConKey { get; set; }
        public AddressResponse Address { get; set; }
        public decimal Value { get; set; }
        public decimal Amount { get; set; }

        public WorkOrderAmountByAccount() 
        {
            Account = new AccountResponse();
            Address = new AddressResponse();
        }
    }
    public class VehicleSearch
    {
        public long ObjectKey { get; set; }
        public AddressResponse VehicleRegistration { get; set; }
        public ItemSerialNumber VehicleSerialNumber { get; set; }
        public AddressResponse RegisteredCustomer { get; set; }
        public AddressResponse RegisterNIC { get; set; }
        public VehicleSearch()
        {
            VehicleRegistration = new AddressResponse();
            RegisteredCustomer = new AddressResponse();
            RegisterNIC = new AddressResponse();
            VehicleSerialNumber = new ItemSerialNumber();
        }
    }
    public class WorkOrder : GenericOrder
    {
        public int TrnKy { get; set; }
        public Vehicle SelectedVehicle { get; set; }
        public DateTime DeliveryDate { get; set; }
        public decimal PrincipalPercentage { get; set; }
        public decimal PrincipalValue { get; set; }
        public decimal CarmartlPercentage { get; set; }
        public decimal CarmartValue { get; set; }
        public CodeBaseResponse Department { get; set; }
        public Estimation WorkOrderSimpleEstimation { get; set; }
        public IList<CustomerComplain> CustomerComplains { get; set; }
        public IList<GenericOrderItem> WorkOrderMaterials { get; set; }
        public IList<GenericOrderItem> WorkOrderServices { get; set; }
        public IList<WorkOrder> JobHistory { get; set; }
        public BLTransaction WorkOrderTransaction { get; set; }
        public int MaterialRequsitionKey { get; set; } = 1;
        public string? MaterialRequsitionNo { get; set; } = "";
        public WorkOrder()
        {
            SelectedVehicle = new Vehicle();
            Department = new CodeBaseResponse();
            WorkOrderSimpleEstimation = new Estimation();
            CustomerComplains = new List<CustomerComplain>();
            WorkOrderMaterials = new List<GenericOrderItem>();
            WorkOrderServices = new List<GenericOrderItem>();
            JobHistory = new List<WorkOrder>();
            WorkOrderTransaction = new BLTransaction();
        }
    }

    public class Vehicle
    {
        public long ObjectKey { get; set; }
        public DateTime VehicleRegisterDate { get; set; }
        public ItemResponse VehicleRegistration { get; set; }
        public AddressResponse VehicleAddress { get; set; }
        public AddressMaster RegisteredCustomer { get; set; }
        public AccountResponse RegisteredAccount { get; set; }
        public ItemSerialNumber SerialNumber { get; set; }
        public CodeBaseResponse Category { get; set; }
        public CodeBaseResponse SubCategory { get; set; }
        public CodeBaseResponse Brand { get; set; } 
        public CodeBaseResponse Model { get; set; } 
        public Warranty VehicleWarrannty { get; set; }
        public CodeBaseResponse MaintenancePackage { get; set; } 
        public decimal CurrentMilage { get; set; }
        public decimal PreviousMilage { get; set; }
        public string Fuel { get; set; } = "";
        public int IsActive { get; set; }
        public bool IsInsurence { get; set; } = false;
        public CodeBaseResponse Province { get; set; }
        public Vehicle()
        {
            VehicleWarrannty = new Warranty();
            VehicleRegistration = new ItemResponse();
            RegisteredCustomer = new AddressMaster();
            SerialNumber = new ItemSerialNumber();
            Category = new CodeBaseResponse();
            SubCategory = new CodeBaseResponse();
            VehicleAddress=new AddressResponse();
            RegisteredAccount=new AccountResponse();
            Province=new CodeBaseResponse();
            Brand=new CodeBaseResponse();  
            Model= new CodeBaseResponse();  
            MaintenancePackage= new CodeBaseResponse(); 
        }
    }

    public class Warranty
    {
        public string WarranrtyStatus { get; set; } = "";
        public DateTime WarrantyStartDate { get; set; } = DateTime.Now;
        public DateTime WarrantyEndDate { get; set; } = DateTime.Now;
    }

    public class CustomerComplain
    {
        public int ComplainID { get; set; }
        public string? ComplainName { get; set; }
        public int IsActive { get; set; }
        public DateTime EnteredDate { get; set; }
        public AddressResponse? EnteredBy { get; set; }
        public CustomerComplain()
        {
            EnteredBy = new AddressResponse();
        }

    }

    public class Estimation
    {
        public long EstimateKey { get; set; }
        public string? EstimationNumber { get; set; } = "";
        public IList<GenericOrderItem> EstimatedMaterials { get; set; }
        public IList<GenericOrderItem> EstimatedServices { get; set; }
        public decimal TotalValue { get; set; }
        public Estimation()
        {
            EstimatedMaterials = new List<GenericOrderItem>();
            EstimatedServices = new List<GenericOrderItem>();
        }

    }

    public class IRNResponse
    {
        public long OrderKey { get; set; }
        public string? OrderNumber { get; set; }
        public DateTime Insertdate { get; set; }
        public CodeBaseResponse IRNType { get; set; }
        public string? VehicleID { get; set; }
        public AddressResponse ServiceAdvisor { get; set; }
        public CodeBaseResponse BusinessUnit { get; set; }
        public ItemResponse Item { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public int IsActive { get; set; }
        public UnitResponse TransactionUnit { get; set; }
		public AccountResponse Insurance { get; set; }
        public int HederIsActive { get; set; }
		public decimal DiscountPercentage { get; set; }
		public decimal DisocuntAmount { get; set; }
		public CodeBaseResponse AnalysisType1 { get; set; }
		public CodeBaseResponse OrderLocation { get; set; }
		public IRNResponse() 
        {
            IRNType= new CodeBaseResponse();
            ServiceAdvisor = new AddressResponse();
            BusinessUnit= new CodeBaseResponse();
            Item= new ItemResponse();
            TransactionUnit = new UnitResponse();
            Insurance = new AccountResponse();
            AnalysisType1= new CodeBaseResponse();
			OrderLocation = new CodeBaseResponse();
		}
    }
}
