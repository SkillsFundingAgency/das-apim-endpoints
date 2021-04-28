using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Assessors.Api.Models;
using SFA.DAS.Assessors.Application.Queries.GetStandardDetails;
using SFA.DAS.Assessors.Application.Queries.GetTrainingCourses;

namespace SFA.DAS.Assessors.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class TrainingCoursesController : ControllerBase
    {
        private readonly ILogger<TrainingCoursesController> _logger;
        private readonly IMediator _mediator;

        public TrainingCoursesController (ILogger<TrainingCoursesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var queryResult = await _mediator.Send(new GetTrainingCoursesQuery());
                
            var model = new GetCourseListResponse
            {
                Courses = queryResult.TrainingCourses.Select(c=>(GetCourseListItem)c).ToList()
            };

            return Ok(model);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetStandardById(string id)
        {
            // Id can be standardUId, LarsCode or IfateReferenceNumber
            // It will return one record, either uniquely via the StandardUid
            // Or the latest active via LarsCode / IFateReferenceNumber
            var queryResponse = await _mediator.Send(new GetStandardDetailsQuery(id));

            if (queryResponse.StandardDetails == null) return NotFound();

            return Ok((GetStandardDetailsResponse)queryResponse.StandardDetails);
        }
    }
}