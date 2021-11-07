using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Core.Models.DBClasses
{
    public class OrdersTbl
    {
        [Key]
        public long ID { get; set; }
        public long UserID { get; set; }
        public int StatusID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        [ForeignKey("UserID")]
        public UsersTbl Users { get; set; }

        //** hint :
        // Total price will be calculated  
        // because Item price may be changed any time.
    }
}