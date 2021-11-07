using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Core.Models.Modules.Users
{
    public class UsersModel
    {
        public long UserID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        public int DefaultLanguageID { get; set; }
        public string Password { get; set; }
        public bool IsDeleted { get; set; }
        public UserDevicesModel oDevice { get; set; }
    }
    public class UserDevicesModel
    {
        public long UserID { get; set; }
        public long UserDeviceID { get; set; }
        public string ApiToken { get; set; }
        public string DeviceToken { get; set; }
    }
}