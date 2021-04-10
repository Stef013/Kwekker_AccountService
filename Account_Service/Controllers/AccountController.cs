using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Account_Service.DataAccess;
using Account_Service.Models;
using Account_Service.Entities;
using Account_Service.Services;
using Account_Service.Services.Interfaces;
using Account_Service.Helpers;

namespace Account_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private IAccountService _accService;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accService = accountService;
            _logger = logger;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticationRequest request)
        {
            var response = _accService.Authenticate(request);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [HttpPost]
        public ResponseMessage create([FromBody]Account account)
        {
            ResponseMessage response = new ResponseMessage();

            if(String.IsNullOrEmpty(account.email) || String.IsNullOrEmpty(account.password))
            {
                response.Success = false;
                response.Message = "Email and Password cannot be empty";
            }
            else
            {
                int accountID = _accService.create(account);

                if ( accountID > 0)
                {
                    response.Success = true;
                    response.Message = "Success!";
                    response.AccountID = accountID;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Database Error!";
                }
            }

            return response;
        }

        
        [HttpGet]
        public string get()
        {
            return "accountservice";
        }

        [Authorize]
        [HttpGet("authorizetest")]
        public string authorizeTest()
        {
            return "You are Authorized!";
        }

        [HttpGet("email")]
        public bool checkEmail(string email)
        {
            return _accService.checkEmail(email);
        }

        [Authorize]
        [HttpPut]
        public bool update([FromBody] Account account)
        {
            return _accService.update(account);
        }

        [Authorize]
        [HttpDelete]
        public bool delete([FromBody] Account account)
        {
            return _accService.delete(account);
        }

    }
}
