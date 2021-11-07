using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.APIBaseModel
{
    public class APIModel
    {
        public string APIKey { get; set; }
        public long UserID { get; set; }
        public string APIToken { get; set; }
        public int APIStatus { get; set; }
        public string APIMessage { get; set; }
        public int LanguageID { get; set; }
        public int PageID { get; set; }
        public int PageSize { get; set; }
    }
}