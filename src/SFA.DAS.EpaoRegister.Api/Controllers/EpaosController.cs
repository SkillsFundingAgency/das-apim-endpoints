using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.EpaoRegister.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class EpaosController : ControllerBase
    {

        public EpaosController()
        {
            
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            throw new NotImplementedException();
            /*try
            {
                var queryResult = await _mediator.Send(new GetCourseListQuery());
                
                var model = new GetCourseListResponse
                {
                    Courses = queryResult.Courses.Select(c=>(GetCourseListItem)c).ToList()
                };

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to get list of Epaos");
                return BadRequest();
            }*/
        }
    }
}
