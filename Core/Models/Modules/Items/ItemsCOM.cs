using Core.Models.DBClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core.Models.Modules.Items
{
    public class ItemsCOM
    {
        #region Method :: CalculateItemStockAmount
        public int CalculateItemStockAmount(long ItemID)
        {
            using (CoreEntity db = new CoreEntity())
            {
                int orderedQty = db.OrderInvoices.Where(x => x.ItemID == ItemID).ToList().Sum(x => x.Quantity);
                int itemQty = db.Items.FirstOrDefault(x => x.ID == ItemID).Quantity;
                return itemQty - orderedQty;
            }
        }
        #endregion
        #region Method :: GetAvailableItems
        public List<ItemsModel> GetAvailableItems(int languageID, int PageID, int PageSize)
        {
            using (CoreEntity db = new CoreEntity())
            {
                return (from item in db.Items
                        join items_loc in db.Items_Loc on item.ID equals items_loc.ItemID
                        where items_loc.LanguageID == languageID && !item.IsDeleted
                        select new
                        {
                            Description = items_loc.Description,
                            Image = item.Image,
                            ItemID = item.ID,
                            Name = items_loc.Name,
                            Price = item.Price,
                            CreatedDate = item.CreatedDate,
                            LanguageID = languageID
                        }).AsEnumerable().Select(item => new ItemsModel()
                        {
                            Description = item.Description,
                            Image = item.Image,
                            ItemID = item.ItemID,
                            Name = item.Name,
                            Price = item.Price,
                            CreatedDate = item.CreatedDate,
                            StockAmount = CalculateItemStockAmount(item.ItemID),
                            LanguageID = languageID
                        }).Where(x => x.StockAmount > 0).OrderByDescending(x => x.CreatedDate).Skip((PageID - 1) * PageSize).Take(PageSize).ToList();
            }
        }
        #endregion
        #region Method :: DeleteItem
        public List<ItemsModel> DeleteItem(long ItemID, int LanguageID, int PageID, int PageSize)
        {
            using (CoreEntity db = new CoreEntity())
            {
                ItemsTbl item = db.Items.Where(x => x.ID == ItemID).FirstOrDefault();
                if (item != null)
                {
                    item.IsDeleted = true;
                    db.SaveChanges();
                }
                return GetAvailableItems(LanguageID, PageID, PageSize);
            }
        }
        #endregion
        #region Method :: GetItem
        public ItemsModel GetItem(long ItemID, int LanguageID)
        {
            using (CoreEntity db = new CoreEntity())
            {
                return (from item in db.Items
                        join items_loc in db.Items_Loc on item.ID equals items_loc.ItemID
                        where items_loc.LanguageID == LanguageID && !item.IsDeleted && item.ID == ItemID
                        select new ItemsModel()
                        {
                            Description = items_loc.Description,
                            Image = item.Image,
                            ItemID = item.ID,
                            Name = items_loc.Name,
                            Price = item.Price,
                            StockAmount = item.Quantity,
                            CreatedDate = item.CreatedDate,
                            LanguageID = LanguageID
                        }).FirstOrDefault();
            }
        }
        #endregion
        #region Method :: SaveItem
        public List<ItemsModel> SaveItem(ItemsModel oModel, int PageID, int PageSize)
        {
            using (CoreEntity db = new CoreEntity())
            {
                ItemsTbl oItem = new ItemsTbl();
                Items_LocTbl oItem_Loc = new Items_LocTbl();
                if (oModel.ItemID > 0)
                {
                    oItem_Loc = db.Items_Loc.FirstOrDefault(x => x.ItemID == oModel.ItemID && x.LanguageID == oModel.LanguageID);
                    oItem = db.Items.FirstOrDefault(x => x.ID == oItem_Loc.ItemID);
                }
                oItem.IsDeleted = false;
                oItem.Image = oModel.Image;
                oItem.Price = oModel.Price;
                oItem.Quantity = oModel.StockAmount;
                oItem.CreatedDate = DateTime.Now;
                oItem_Loc.Name = oModel.Name;
                oItem_Loc.Description = oModel.Description;
                oItem_Loc.LanguageID = oModel.LanguageID;
                if (oModel.ItemID == 0)
                {
                    db.Items.Add(oItem);
                    db.SaveChanges();
                    oItem_Loc.ItemID = oItem.ID;
                    db.Items_Loc.Add(oItem_Loc);
                }
                db.SaveChanges();
                if (oItem.ID > 0 && oItem_Loc.ID > 0)
                    return GetAvailableItems(oModel.LanguageID, PageID, PageSize);
                else
                    return new List<ItemsModel>();
            }
        }
        #endregion
    }
}