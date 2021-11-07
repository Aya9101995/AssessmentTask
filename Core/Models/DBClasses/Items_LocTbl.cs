using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Core.Models.DBClasses
{
    public class Items_LocTbl
    {
        [Key]
        public long ID { get; set; }
        public long ItemID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int LanguageID { get; set; }

        [ForeignKey("ItemID")]
        public ItemsTbl Items { get; set; }
    }
}