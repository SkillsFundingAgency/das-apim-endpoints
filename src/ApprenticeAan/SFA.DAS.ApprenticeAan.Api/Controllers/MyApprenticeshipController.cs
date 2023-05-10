using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Application.MyApprenticeship.Queries.GetMyApprenticeship;

namespace SFA.DAS.ApprenticeAan.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MyApprenticeshipController : ControllerBase
{
    private readonly IMediator _mediator;

    public MyApprenticeshipController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{apprenticeId}")]
    [ProducesResponseType(typeof(GetMyApprenticeshipQueryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyApprenticeship(Guid apprenticeId, CancellationToken cancellationToken)
    {
        var myApprenticeshipDetails = await _mediator.Send(new GetMyApprenticeshipQuery { ApprenticeId = apprenticeId }, cancellationToken);
            
        return myApprenticeshipDetails.StatusCode switch
        {
            HttpStatusCode.BadRequest => BadRequest(),
            HttpStatusCode.NotFound => NotFound(),
            _ => Ok(myApprenticeshipDetails.MyApprenticeship)
        };
    }
}