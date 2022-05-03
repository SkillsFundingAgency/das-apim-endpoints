using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Forecasting.Api.Models;
using SFA.DAS.Forecasting.Application.Pledges.Queries.GetApplications;

namespace SFA.DAS.Forecasting.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApplicationsController : Controller
    {
        private readonly IMediator _mediator;

        public ApplicationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetApplications(int pledgeId)
        {
            var queryResult = await _mediator.Send(new GetApplicationsQuery { PledgeId = pledgeId });
            var result = (GetApplicationsResponse)queryResult;
            return Ok(result);
        }
    }

}
