using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Forecasting.Api.Models;
using SFA.DAS.Forecasting.Application.Pledges.Queries.GetApplications;

namespace SFA.DAS.Forecasting.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApplicationsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ApplicationsController> _logger;

        public ApplicationsController(IMediator mediator, ILogger<ApplicationsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetApplications(int page, int pageSize)
        {
            var queryResult = await _mediator.Send(new GetApplicationsQuery { Page = page, PageSize = pageSize });
            var result = (GetApplicationsResponse)queryResult;
            return Ok(result);
        }
    }

}
