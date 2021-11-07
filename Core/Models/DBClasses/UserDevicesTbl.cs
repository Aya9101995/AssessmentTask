using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Core.Models.DBClasses
{
    public class UserDevicesTbl
    {
        [Key]
        public long ID { get; set; }
        public long UserID { get; set; }
        public string ApiToken { get; set; }
        public string DeviceToken { get; set; }

        [ForeignKey("UserID")]
        public UsersTbl UsersTbl { get; set; }
    }
}