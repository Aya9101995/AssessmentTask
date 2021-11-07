using API.Models.APIBaseModel;
using Core.Models.Modules.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.APIs.ItemsAPI
{
    public class ItemsAPI
    {
        public ItemsResponseAPI SaveItem(ItemsRequestAPI oItemsRequestAPI)
        {
            ItemsCOM oItemsCOM = new ItemsCOM();

            ItemsResponseAPI oItemsResponseAPI = new ItemsResponseAPI();
            oItemsResponseAPI.lstItem = oItemsCOM.SaveItem(oItemsRequestAPI.oItem, oItemsRequestAPI.PageID, oItemsRequestAPI.PageSize);
            if (oItemsResponseAPI.lstItem.Count > 0)
            {
                oItemsResponseAPI.APIStatus = 1;
                oItemsResponseAPI.APIMessage = "Success";
            }
            else
            {
                oItemsResponseAPI.APIStatus = -3;
                oItemsResponseAPI.APIMessage = "Something went wrong";
            }

            return oItemsResponseAPI;
        }

        public ItemResponseAPI GetItem(ItemsRequestAPI oItemsRequestAPI)
        {
            ItemsCOM oItemsCOM = new ItemsCOM();

            ItemResponseAPI oItemsResponseAPI = new ItemResponseAPI();
            oItemsResponseAPI.oItem = oItemsCOM.GetItem(oItemsRequestAPI.ItemID, oItemsRequestAPI.LanguageID);
            if (oItemsResponseAPI.oItem != null)
            {
                oItemsResponseAPI.APIStatus = 1;
                oItemsResponseAPI.APIMessage = "Success";
            }
            else
            {
                oItemsResponseAPI.APIStatus = -3;
                oItemsResponseAPI.APIMessage = "Something went wrong";
            }

            return oItemsResponseAPI;
        }

        public ItemsResponseAPI DeleteItem(ItemsRequestAPI oItemsRequestAPI)
        {
            ItemsCOM oItemsCOM = new ItemsCOM();

            ItemsResponseAPI oItemsResponseAPI = new ItemsResponseAPI();
            oItemsResponseAPI.lstItem = oItemsCOM.DeleteItem(oItemsRequestAPI.ItemID, oItemsRequestAPI.LanguageID, oItemsRequestAPI.PageID, oItemsRequestAPI.PageSize);
            oItemsResponseAPI.APIStatus = 1;
            oItemsResponseAPI.APIMessage = "Success";
            return oItemsResponseAPI;
        }

        public ItemsResponseAPI GetAvailableItems(ItemsRequestAPI oItemsRequestAPI)
        {
            ItemsCOM oItemsCOM = new ItemsCOM();
            ItemsResponseAPI oItemsResponseAPI = new ItemsResponseAPI();
            oItemsResponseAPI.lstItem = oItemsCOM.GetAvailableItems(oItemsRequestAPI.LanguageID, oItemsRequestAPI.PageID, oItemsRequestAPI.PageSize);
            oItemsResponseAPI.APIStatus = 1;
            oItemsResponseAPI.APIMessage = "Success";
            return oItemsResponseAPI;
        }
    }
    public class ItemsRequestAPI : APIModel
    {
        public ItemsModel oItem { get; set; }
        public long ItemID { get; set; }
    }
    public class ItemResponseAPI : APIModel
    {
        public ItemsModel oItem { get; set; }
    }
    public class ItemsResponseAPI : APIModel
    {
        public List<ItemsModel> lstItem { get; set; }
    }
}