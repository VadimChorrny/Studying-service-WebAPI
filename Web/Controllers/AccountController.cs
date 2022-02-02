using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Services;
using AutoMapper;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        
        [Authorize]
        [HttpPost]
        [Route("getPagination")]
        public  ActionResult<IEnumerable<UserDto>> GetPagination([FromBody]  StudentParameters studentParameters)
        {
            var accounts =   accountService.GetPagination(studentParameters);

            return Ok(accounts);
        }
        
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveUser(string id)
        {
            await accountService.Delete(id);
            return Ok();
        }


        [Authorize]
        [HttpPost]
        [Route("subscribe")]
        public IActionResult Subscribe(SubscribeDto request)
        {
            accountService.Subscribe(GetIdFromClaims(), request.CourseId, request.StartStudyDate);
            return Ok();
        }

        [HttpPost]
        [Route("unsubscribe")]
        public IActionResult UnSubscribe(SubscribeDto request)
        {
            accountService.UnSubscribe(GetIdFromClaims(), request.CourseId);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(string id)
        {
            var account = await  accountService.GetById(id);
            return Ok(account);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async  Task<ActionResult<UserDto>> Update( UpdateDto model,string id)
        {
            UserDto account;
            if (id == null)
            {
                 account = await accountService.Update(GetIdFromClaims(), model);
            }
            else
            {
                 account = await accountService.Update(id, model);
            }

            return Ok(account);
        }

        public string GetIdFromClaims()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null && identity.Claims.Count() != 0)
            {
                return identity.FindFirst("Id")?.Value;
            }

            return null;
        }

    }
}