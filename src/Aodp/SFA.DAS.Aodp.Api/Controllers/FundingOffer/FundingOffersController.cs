using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aodp.Application.Queries.FundingOffer;

namespace SFA.DAS.Aodp.Api.Controllers.FundingOffer;

[Route("api/[controller]")]
[ApiController]
public class FundingOffersController : BaseController
{
    public FundingOffersController(IMediator mediator, ILogger<FundingOffersController> logger) : base(mediator, logger)
    { }

    [HttpGet("/api/funding-offers")]
    [ProducesResponseType(typeof(GetFundingOffersQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFundingOffers()
    {
        return await SendRequestAsync(new GetFundingOffersQuery());
    }

}
