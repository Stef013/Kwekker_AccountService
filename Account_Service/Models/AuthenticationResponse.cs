using Account_Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account_Service.Models
{
    public class AuthenticationResponse
    {
        public int AccountID { get; set; }
        public string Token { get; set; }

        public AuthenticationResponse(Account account, string token)
        {
            AccountID = account.ID;
            Token = token;
        }

    }
}
