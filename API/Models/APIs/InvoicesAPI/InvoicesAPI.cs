using API.Models.APIBaseModel;
using Core.Models.Enums;
using Core.Models.Modules.Invoices;
using Core.Models.Modules.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.APIs.InvoicesAPI
{
    public class InvoicesAPI
    {
        public InvoiceResponseAPI PlaceOrder(InvoiceRequestAPI oInvoiceRequestAPI)
        {
            InvoicesCOM oInvoicesCOM = new InvoicesCOM();
            InvoiceResponseAPI oInvoiceResponseAPI = new InvoiceResponseAPI();
            long UserID = UsersCOM.GetUserIDByToken(oInvoiceRequestAPI.APIToken);
            oInvoiceResponseAPI.oInvoice = oInvoicesCOM.PlaceOrder(oInvoiceRequestAPI.lstInvoiceItems, UserID, oInvoiceRequestAPI.LanguageID);
            if (oInvoiceResponseAPI.oInvoice.InvoiceID > 0)
            {
                oInvoiceResponseAPI.APIStatus = 1;
                oInvoiceResponseAPI.APIMessage = "Success";
            }
            else
            {
                oInvoiceResponseAPI.APIStatus = -1;
                oInvoiceResponseAPI.APIMessage = "Something went wrong";
            }
            return oInvoiceResponseAPI;
        }
        public InvoicesResponseAPI GetUserInvoices(InvoiceRequestAPI oInvoiceRequestAPI)
        {
            InvoicesCOM oInvoicesCOM = new InvoicesCOM();
            InvoicesResponseAPI oInvoiceResponseAPI = new InvoicesResponseAPI();
            long UserID = UsersCOM.GetUserIDByToken(oInvoiceRequestAPI.APIToken);
            oInvoiceResponseAPI.lstInvoices = oInvoicesCOM.GetUserInvoices(UserID, oInvoiceRequestAPI.LanguageID);
            if (oInvoiceResponseAPI.lstInvoices.Count >= 0)
            {
                oInvoiceResponseAPI.APIStatus = 1;
                oInvoiceResponseAPI.APIMessage = "Success";
            }
            else
            {
                oInvoiceResponseAPI.APIStatus = -1;
                oInvoiceResponseAPI.APIMessage = "Something went wrong";
            }
            return oInvoiceResponseAPI;
        }
        public InvoiceResponseAPI GetUserInvoiceItems(InvoiceRequestAPI oInvoiceRequestAPI)
        {
            InvoicesCOM oInvoicesCOM = new InvoicesCOM();
            InvoiceResponseAPI oInvoiceResponseAPI = new InvoiceResponseAPI();
            long UserID = UsersCOM.GetUserIDByToken(oInvoiceRequestAPI.APIToken);
            oInvoiceResponseAPI.oInvoice = oInvoicesCOM.GetUserInvoiceItems(oInvoiceRequestAPI.OrderID, UserID, oInvoiceRequestAPI.LanguageID);
            if (oInvoiceResponseAPI.oInvoice != null)
            {
                oInvoiceResponseAPI.APIStatus = 1;
                oInvoiceResponseAPI.APIMessage = "Success";
            }
            else
            {
                oInvoiceResponseAPI.APIStatus = -1;
                oInvoiceResponseAPI.APIMessage = "Something went wrong";
            }
            return oInvoiceResponseAPI;
        }
        public InvoiceResponseAPI ChangeOrderStatus(InvoiceRequestAPI oInvoiceRequestAPI)
        {
            InvoicesCOM oInvoicesCOM = new InvoicesCOM();
            InvoiceResponseAPI oInvoiceResponseAPI = new InvoiceResponseAPI();
            long UserID = UsersCOM.GetUserIDByToken(oInvoiceRequestAPI.APIToken);
            bool IsStatusChanged = oInvoicesCOM.ChangeOrderStatus(oInvoiceRequestAPI.OrderID, (EnumInvoiceStatus)oInvoiceRequestAPI.StatusID);
            if (IsStatusChanged == true)
            {
                oInvoiceResponseAPI.APIStatus = 1;
                oInvoiceResponseAPI.APIMessage = "Success";
            }
            else
            {
                oInvoiceResponseAPI.APIStatus = -1;
                oInvoiceResponseAPI.APIMessage = "Something went wrong";
            }
            return oInvoiceResponseAPI;
        }
        public InvoiceResponseAPI CancelOrder(InvoiceRequestAPI oInvoiceRequestAPI)
        {
            InvoicesCOM oInvoicesCOM = new InvoicesCOM();
            InvoiceResponseAPI oInvoiceResponseAPI = new InvoiceResponseAPI();
            long UserID = UsersCOM.GetUserIDByToken(oInvoiceRequestAPI.APIToken);
            bool IsOrderCancelled = oInvoicesCOM.CancelOrder(oInvoiceRequestAPI.OrderID, UserID);
            if (IsOrderCancelled == true)
            {
                oInvoiceResponseAPI.APIStatus = 1;
                oInvoiceResponseAPI.APIMessage = "Success";
            }
            else
            {
                oInvoiceResponseAPI.APIStatus = -1;
                oInvoiceResponseAPI.APIMessage = "Something went wrong";
            }
            return oInvoiceResponseAPI;
        }
    }
    public class InvoiceRequestAPI : APIModel
    {
        public List<InvoiceItemsModel> lstInvoiceItems { get; set; }
        public long OrderID { get; set; }
        public int StatusID { get; set; }

    }
    public class InvoiceResponseAPI : APIModel
    {
        public InvoicesModel oInvoice { get; set; }
    }
    public class InvoicesResponseAPI : APIModel
    {
        public List<InvoicesModel> lstInvoices { get; set; }
    }
}