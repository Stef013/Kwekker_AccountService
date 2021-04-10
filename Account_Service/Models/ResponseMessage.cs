using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account_Service.Models
{
    public class ResponseMessage
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int AccountID { get; set; }
    }
}
