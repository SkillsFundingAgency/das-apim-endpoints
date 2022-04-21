using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Forecasting.Api.Models;
using SFA.DAS.Forecasting.Application.Pledges.Queries.GetPledges;

namespace SFA.DAS.Forecasting.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PledgesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PledgesController> _logger;

        public PledgesController(IMediator mediator, ILogger<PledgesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetPledges(int page, int pageSize)
        {
            var queryResult = await _mediator.Send(new GetPledgesQuery{ Page = page, PageSize = pageSize});
            var result = (GetPledgesResponse) queryResult;
            return Ok(result);
        }
    }
}
