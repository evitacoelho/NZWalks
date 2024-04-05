using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace NZWalks.API.Controllers
{
    //https://localhost:portnumber/api/students/
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        //GET :  //https://localhost:portnumber/api/students/
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            //returns all students 
            string[] studentNames = new string[] { "Luna", "Kaos", "Ryan", "Rachel", "Monica" };
            //return status code and student names to the caller
            return Ok(studentNames);
        }
    }
}
