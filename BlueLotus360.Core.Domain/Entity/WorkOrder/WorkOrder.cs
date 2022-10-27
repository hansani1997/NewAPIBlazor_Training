﻿using BlueLotus360.Core.Domain.DTOs;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Core.Domain.Entity.WorkOrder
{
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

        public WorkOrder()
        {
            SelectedVehicle = new Vehicle();
            Department = new CodeBaseResponse();
            WorkOrderSimpleEstimation = new Estimation();
            CustomerComplains = new List<CustomerComplain>();
            WorkOrderMaterials = new List<GenericOrderItem>();
            WorkOrderServices = new List<GenericOrderItem>();
            JobHistory = new List<WorkOrder>();
        }
    }

    public class Vehicle
    {
        public long ObjectKey { get; set; }
        public ItemResponse VehicleRegistration { get; set; }
        public AddressResponse VehicleAddress { get; set; }
        public AddressMaster RegisteredCustomer { get; set; }
        public ItemSerialNumber SerialNumber { get; set; }
        public CodeBaseResponse Category { get; set; }
        public CodeBaseResponse SubCategory { get; set; }
        public string Brand { get; set; } = "";
        public string Model { get; set; } = "";
        public Warranty VehicleWarrannty { get; set; }
        public string MaintenancePackage { get; set; } = "";
        public decimal CurrentMilage { get; set; }
        public decimal PreviousMilage { get; set; }
        public string Fuel { get; set; } = "";
        public int IsActive { get; set; }
        public Vehicle()
        {
            VehicleWarrannty = new Warranty();
            VehicleRegistration = new ItemResponse();
            RegisteredCustomer = new AddressMaster();
            SerialNumber = new ItemSerialNumber();
            Category = new CodeBaseResponse();
            SubCategory = new CodeBaseResponse();
            VehicleAddress=new AddressResponse();
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
        public IList<GenericOrderItem> EstimatedMaterials { get; set; }
        public IList<GenericOrderItem> EstimatedServices { get; set; }
        public decimal TotalValue { get; set; }
        public Estimation()
        {
            EstimatedMaterials = new List<GenericOrderItem>();
            EstimatedServices = new List<GenericOrderItem>();
        }

    }
}