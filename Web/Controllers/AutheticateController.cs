using System;
using System.Threading.Tasks;
using BLL.Services;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]"
    )]
    public class AutheticateController :ControllerBase
    {
        private readonly IAutheticateService accountService;
        public AutheticateController( IAutheticateService accountService)
        {   
            this.accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] AutheticateDto model)
        {          
            var response = await accountService.Login(model,ipAddress());
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {           
            Console.WriteLine(Request.Host.Host);
            await accountService.Register(model);              
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("facebookAuthenticate")]
        public async Task<IActionResult> LoginWithFacebook(TokenDto token)
        {
            var response = await accountService.LoginWithFacebook(token.Token, ipAddress());
            return Ok(response);
        }
        

        [AllowAnonymous]
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(VerifiEmailDto model)
        {
            await accountService.VerifyEmail(model.Id, model.Token);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken(TokenDto token)
        {
            var response = await accountService.RefreshToken(token.Token, ipAddress());
            return Ok(response);
        }

        private string ipAddress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}