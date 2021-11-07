using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Configuration;
using Microsoft.Ajax.Utilities;

namespace Core.Models.DBClasses
{
    public class CoreEntity : DbContext
    {
        public CoreEntity() : base("CoreDB")
        {
        }
        public DbSet<UsersTbl> Users { get; set; }
        public DbSet<UserDevicesTbl> UserDevices { get; set; }
        public DbSet<ItemsTbl> Items { get; set; }
        public DbSet<Items_LocTbl> Items_Loc { get; set; }
        public DbSet<OrdersTbl> Orders { get; set; }
        public DbSet<OrderInvoicesTbl> OrderInvoices { get; set; }
    }
}