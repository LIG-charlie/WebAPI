using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIWeb.Models
{
    public class RequestParam
    {
        public data data { get; set; }
    }
    public class data
    {
        public String workspace { get; set; }
        public String name { get; set; }
        public String notes { get; set; }
    }

}