using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Services;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService courseService;

        public CourseController(ICourseService courseService)
        {
            this.courseService = courseService;
        }
        
        [HttpPost]
        [Route("getPagination")]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetPagination([FromBody] CourseParameters courseParameters)
        {
            var courses = await courseService.GetPagination(courseParameters,GetIdFromClaims());
            return Ok(courses);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(UpdateCourseDto dto,int id)
        {
            var course = await courseService.Update(dto, id);
            return Ok(course);
        }
        
        [Authorize]
        [HttpPost]
        [Route("getPaginationSubscription")]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetPaginationSubscription([FromBody] CourseParameters courseParameters)
        {

            var courses = await courseService.GetPaginationSubscription(courseParameters,GetIdFromClaims());
            return Ok(courses);
        }

        [Authorize]
        [HttpPost]
        [Route("add")]
        public IActionResult AddCourse([FromBody] CourseDto dto)
        {
            courseService.AddCourse(dto);
            return Ok();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult RemoveCourse(int id)
        {
            courseService.Remove(id);
            return Ok();
        }
        
       //helper
        public string GetIdFromClaims()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                return identity.FindFirst("Id")?.Value;
            }

            return null;
        }
    }
}