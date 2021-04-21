using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Account_Service.Entities
{
    public class Account
    {
        [Key]
        public int ID { get; set; }

        public string email { get; set; }
 
 
        public string password { get; set; } 
    }
}
