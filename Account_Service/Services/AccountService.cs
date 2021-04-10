using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account_Service.DataAccess;
using Account_Service.Models;
using Account_Service.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using Account_Service.Services.Interfaces;
using Account_Service.Helpers;

namespace Account_Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly AccountDbContext _context;
        private readonly AppSettings _appSettings;
        private Hashing hashing;

        public AccountService(AccountDbContext context, IOptions<AppSettings> appSettings )
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public AuthenticationResponse Authenticate(AuthenticationRequest request)
        {
            hashing = new Hashing();
            request.Password = hashing.ComputeSha256Hash(request.Password);

            var account = _context.Account.FirstOrDefault(x => x.email == request.Email && x.password == request.Password);

            // return null if user not found
            if (account == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(account);

            return new AuthenticationResponse(account, token);
        }

        public int create(Account account)
        {
            int accountID;

            try
            {
                hashing = new Hashing();
                account.password = hashing.ComputeSha256Hash(account.password);

                using (var context = _context)
                {
                    context.Account.Add(account);
                    context.SaveChanges();

                    accountID = account.ID;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                accountID = 0;
            }
            
            return accountID;
        }


        public Account GetById(int id)
        {
            return _context.Account.FirstOrDefault(x => x.ID == id);
        }


        public bool checkEmail(string email)
        {
            bool result;
            try
            {
                using (var context = _context)
                {
                    result = context.Account
                        .Any(a => a.email == email);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                result = false;
            }

            return result;
        }

        public bool update(Account account)
        {
            bool result;
            try
            {
                using (var context = _context)
                {
                    var acc = context.Account
                        .Single(a => a.ID == account.ID);
                    acc.email = account.email;
                    acc.password = account.password;
                    context.SaveChanges();
                }
                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                result = false;
            }

            return result;
        }

        public bool delete(Account account)
        {
            bool result;

            try
            {
                using (var context = _context)
                {
                    var acc = context.Account
                        .Single(a => a.ID == account.ID && a.email == account.email && a.password == account.password);

                    context.Account.Remove(acc);
                    context.SaveChanges();
                }
                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                result = false;
            }

            return result;
        }

        public string generateJwtToken(Account account)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", account.ID.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
