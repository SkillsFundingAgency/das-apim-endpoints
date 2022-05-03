using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Forecasting.Api.Models;
using SFA.DAS.Forecasting.Application.Pledges.Queries.GetAccountsWithPledges;
using SFA.DAS.Forecasting.Application.Pledges.Queries.GetPledges;

namespace SFA.DAS.Forecasting.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PledgesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PledgesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetPledges(long accountId)
        {
            var queryResult = await _mediator.Send(new GetPledgesQuery{ AccountId = accountId });
            var result = (GetPledgesResponse) queryResult;
            return Ok(result);
        }

        [HttpGet]
        [Route("accountIds")]
        public async Task<IActionResult> GetAccountsWithPledges()
        {
            var queryResult = await _mediator.Send(new GetAccountsWithPledgesQuery());
            var result = (GetAccountsWithPledgesResponse)queryResult;
            return Ok(result);
        }
    }
}
