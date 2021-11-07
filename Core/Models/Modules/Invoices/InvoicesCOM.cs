using Core.Models.DBClasses;
using Core.Models.Enums;
using Core.Models.Modules.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core.Models.Modules.Invoices
{
    public class InvoicesCOM
    {
        #region GetUserInvoices
        public List<InvoicesModel> GetUserInvoices(long UserID, int LanguageID)
        {
            using (CoreEntity db = new CoreEntity())
            {
                List<InvoicesModel> lstInvoiceModel = (from order in db.Orders
                                                       join invoice in db.OrderInvoices on order.ID equals invoice.OrderID
                                                       join item in db.Items on invoice.ItemID equals item.ID into items
                                                       from item_ in items.DefaultIfEmpty()
                                                       join itemloc in db.Items_Loc on invoice.ItemID equals itemloc.ItemID into itemsloc
                                                       from itemloc_ in itemsloc.DefaultIfEmpty()
                                                       where order.UserID == UserID
                                                       && (itemloc_ != null ? itemloc_.LanguageID == LanguageID : true)
                                                       select new
                                                       {
                                                           CreatedDate = order.CreatedDate,
                                                           InvoiceID = order.ID,
                                                           ModifiedDate = order.ModifiedDate,
                                                           StatusID = (EnumInvoiceStatus)order.StatusID,
                                                           UserID = order.UserID,
                                                           ItemID = invoice.ItemID,
                                                           ItemName = itemloc_ != null ? itemloc_.Name : "",
                                                           ItemDescription = itemloc_ != null ? itemloc_.Description : "",
                                                           ItemPrice = invoice.ItemPrice,
                                                           ItemQuantity = invoice.Quantity
                                                       }).AsEnumerable().GroupBy(x => x.InvoiceID).Select(oOrder => new InvoicesModel()
                                                       {
                                                           CreatedDate = oOrder.FirstOrDefault().CreatedDate,
                                                           InvoiceID = oOrder.FirstOrDefault().InvoiceID,
                                                           ModifiedDate = oOrder.FirstOrDefault().ModifiedDate,
                                                           StatusID = (EnumInvoiceStatus)oOrder.FirstOrDefault().StatusID,
                                                           UserID = oOrder.FirstOrDefault().UserID,
                                                           lstItems = (from item in oOrder
                                                                       select new InvoiceItemsModel()
                                                                       {
                                                                           ItemID = item.ItemID,
                                                                           Quantity = item.ItemQuantity,
                                                                           ItemUnitPrice = item.ItemPrice,
                                                                           TotalPrice = item.ItemPrice * item.ItemQuantity,
                                                                           ItemDescription = item.ItemDescription,
                                                                           ItemName = item.ItemName
                                                                       }).ToList(),
                                                           TotalPrice = GetOrderTotalPrice(oOrder.FirstOrDefault().InvoiceID)
                                                       }).ToList();
                return lstInvoiceModel;
            }
        }
        #endregion
        #region GetOrderTotalPrice
        public decimal GetOrderTotalPrice(long OrderID)
        {
            decimal Total = 0;
            using (CoreEntity db = new CoreEntity())
            {
                Total = db.OrderInvoices.AsNoTracking().Where(x => x.OrderID == OrderID).Sum(x => x.ItemPrice * x.Quantity);
            }
            return Total;
        }
        #endregion
        #region GetUserInvoiceItems
        public InvoicesModel GetUserInvoiceItems(long OrderID, long UserID, int LanguageID)
        {
            using (CoreEntity db = new CoreEntity())
            {
                InvoicesModel oInvoiceModel = (from order in db.Orders
                                               join invoice in db.OrderInvoices on order.ID equals invoice.OrderID
                                               join item in db.Items on invoice.ItemID equals item.ID into items
                                               from item_ in items.DefaultIfEmpty()
                                               join itemloc in db.Items_Loc on invoice.ItemID equals itemloc.ItemID into itemsloc
                                               from itemloc_ in itemsloc.DefaultIfEmpty()
                                               where order.ID == OrderID && order.UserID == UserID
                                               && (itemloc_ != null ? itemloc_.LanguageID == LanguageID : true)
                                               select new
                                               {
                                                   CreatedDate = order.CreatedDate,
                                                   InvoiceID = order.ID,
                                                   ModifiedDate = order.ModifiedDate,
                                                   StatusID = (EnumInvoiceStatus)order.StatusID,
                                                   UserID = order.UserID,
                                                   ItemID = invoice.ItemID,
                                                   ItemName = itemloc_ != null ? itemloc_.Name : "",
                                                   ItemDescription = itemloc_ != null ? itemloc_.Description : "",
                                                   ItemPrice = invoice.ItemPrice,
                                                   ItemQuantity = invoice.Quantity
                                               }).AsEnumerable().GroupBy(x => x.InvoiceID).Select(oOrder => new InvoicesModel()
                                               {
                                                   CreatedDate = oOrder.FirstOrDefault().CreatedDate,
                                                   InvoiceID = oOrder.FirstOrDefault().InvoiceID,
                                                   ModifiedDate = oOrder.FirstOrDefault().ModifiedDate,
                                                   StatusID = (EnumInvoiceStatus)oOrder.FirstOrDefault().StatusID,
                                                   UserID = oOrder.FirstOrDefault().UserID,
                                                   lstItems = (from item in oOrder
                                                               select new InvoiceItemsModel()
                                                               {
                                                                   ItemID = item.ItemID,
                                                                   Quantity = item.ItemQuantity,
                                                                   ItemUnitPrice = item.ItemPrice,
                                                                   ItemDescription = item.ItemDescription,
                                                                   TotalPrice = item.ItemPrice * item.ItemQuantity,
                                                                   ItemName = item.ItemName
                                                               }).ToList(),
                                                   TotalPrice = GetOrderTotalPrice(oOrder.FirstOrDefault().InvoiceID)
                                               }).FirstOrDefault();
                return oInvoiceModel;
            }
        }
        #endregion
        #region PlaceOrder
        public InvoicesModel PlaceOrder(List<InvoiceItemsModel> lstItems, long UserID, int LanguageID)
        {
            using (CoreEntity db = new CoreEntity())
            {
                OrdersTbl oOrder = new OrdersTbl();
                oOrder.UserID = UserID;
                oOrder.StatusID = (int)EnumInvoiceStatus.Placed;
                oOrder.CreatedDate = oOrder.ModifiedDate = DateTime.Now;
                db.Orders.Add(oOrder);
                db.SaveChanges();
                if (oOrder.ID > 0)
                {
                    // I used foreach instead of for loop because we only need to insert,
                    //no need to access index or make any update.
                    List<OrderInvoicesTbl> lstOrderInvoices = new List<OrderInvoicesTbl>();
                    foreach (var item in lstItems)
                    {
                        OrderInvoicesTbl oOrderInvoice = new OrderInvoicesTbl();
                        oOrderInvoice.OrderID = oOrder.ID;
                        oOrderInvoice.ItemID = item.ItemID;
                        oOrderInvoice.Quantity = item.Quantity;
                        oOrderInvoice.ItemPrice = item.ItemUnitPrice;
                        oOrderInvoice.TotalPrice = item.Quantity * item.ItemUnitPrice;
                        lstOrderInvoices.Add(oOrderInvoice);
                    }
                    db.OrderInvoices.AddRange(lstOrderInvoices);
                    db.SaveChanges();
                    return GetUserInvoiceItems(oOrder.ID, UserID, LanguageID);
                }
                else
                    return new InvoicesModel();
            }
        }
        #endregion
        #region ChangeOrderStatus
        public bool ChangeOrderStatus(long OrderID, EnumInvoiceStatus Status, long UserID = 0)
        {
            using (CoreEntity db = new CoreEntity())
            {
                OrdersTbl oOrder = db.Orders.FirstOrDefault(x => x.ID == OrderID && (UserID > 0 ? x.UserID == UserID : true));
                if (oOrder != null)
                {
                    oOrder.ModifiedDate = DateTime.Now;
                    oOrder.StatusID = (int)Status;
                    db.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }
        #endregion
        #region CancelOrder
        public bool CancelOrder(long OrderID, long UserID)
        {
            if (UserID > 0)
                return ChangeOrderStatus(OrderID, EnumInvoiceStatus.Cancelled, UserID);
            else
                return false;
        }
        #endregion
    }
}