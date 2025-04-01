using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace NZWalks.API.Controllers
{
    //http://localhost:5075/api/Students
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            //GET: http://localhost:5075/api/Students
            String[] studentsName = new string[] { "a", "b", "c", "d" };
            return Ok(studentsName);
        }
    }
}
