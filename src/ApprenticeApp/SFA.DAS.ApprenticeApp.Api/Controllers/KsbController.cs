using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using SFA.DAS.ApprenticeApp.Application.Queries.CourseOptionKsbs;

namespace SFA.DAS.ApprenticeApp.Api.Controllers
{
    [ApiController]
    public class KsbController : ControllerBase
    {
        private readonly IMediator _mediator;

        public KsbController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("/Ksb/{id}/options/{option}/ksbs")]

        public async Task<IActionResult> GetKsbs(string id, string option)
        {
            var queryResult = await _mediator.Send(new GetStandardOptionKsbsQuery
            {
                Id = id,
                Option = option
            });

            return Ok(queryResult.Ksbs);
        }
    }
}
