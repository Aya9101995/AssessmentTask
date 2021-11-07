using Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core.Models.Modules.Invoices
{
    public class InvoicesModel
    {
        public long InvoiceID { get; set; }
        public long UserID { get; set; }
        public EnumInvoiceStatus StatusID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public decimal TotalPrice { get; set; }
        public List<InvoiceItemsModel> lstItems { get; set; }
    }

    public class InvoiceItemsModel
    {
        public long InvoiceItemID { get; set; }
        public long ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string ItemImage { get; set; }
        public int Quantity { get; set; }
        public decimal ItemUnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}