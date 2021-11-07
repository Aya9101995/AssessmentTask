using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Core.Models.DBClasses
{
    public class OrderInvoicesTbl
    {
        [Key]
        public long ID { get; set; }
        public long ItemID { get; set; }
        public long OrderID { get; set; }
        public int Quantity { get; set; }

        // i used here item price because original item price may be changed by admin
        // so we have to record which price has been applied for each invoice
        public decimal ItemPrice { get; set; }
        public decimal TotalPrice { get; set; }

        [ForeignKey("ItemID")]
        public ItemsTbl Items { get; set; }

        [ForeignKey("OrderID")]
        public OrdersTbl Invoices { get; set; }
    }
}