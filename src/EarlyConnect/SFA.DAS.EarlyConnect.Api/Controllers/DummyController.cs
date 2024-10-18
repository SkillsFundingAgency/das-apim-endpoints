using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace SFA.DAS.EarlyConnect.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DummyController : ControllerBase
    {
        //[HttpGet]
        //public IActionResult Get()
        //{
        //    var response = new
        //    {
        //        Message = "This is a sample API response.",
        //        Timestamp = DateTime.UtcNow
        //    };

        //    return Ok(response);
        //}

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult GetById(int id)
        {
            if (id <= 0)
            {
                return NotFound("ID must be greater than zero.");
            }

            var response = new
            {
                Id = id,
                Message = $"Data for item with ID {id}.",
                Timestamp = DateTime.UtcNow
            };

            return Ok(response);
        }
    }
}
