using Account_Service.Entities;
using Account_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account_Service.Services.Interfaces
{
    public interface IAccountService
    {
        public AuthenticationResponse Authenticate(AuthenticationRequest request);
        public int create(Account account);
        public Account GetById(int id);
        public bool checkEmail(string email);
        public bool update(Account account);
        public bool delete(Account account);
        public string generateJwtToken(Account account);
    }
}
