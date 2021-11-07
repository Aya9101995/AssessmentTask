using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Core.Models.Modules.Items
{
    public class ItemsModel
    {

        public long ItemID { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        public decimal Price { get; set; }
        [Required]
        public int StockAmount { get; set; }
        public bool IsDeleted { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public int LanguageID { get; set; }
    }
}