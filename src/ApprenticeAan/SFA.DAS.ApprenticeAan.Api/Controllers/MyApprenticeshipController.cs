using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Api.Models;
using SFA.DAS.ApprenticeAan.Application.Commitments.GetRecentCommitment;
using SFA.DAS.ApprenticeAan.Application.MyApprenticeships.Commands.CreateMyApprenticeship;
using SFA.DAS.ApprenticeAan.Application.MyApprenticeships.Queries.GetMyApprenticeship;
using SFA.DAS.ApprenticeAan.Application.StagedApprentices.Queries.GetStagedApprentice;

namespace SFA.DAS.ApprenticeAan.Api.Controllers;

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
        var myApprenticeship = await _mediator.Send(new GetMyApprenticeshipQuery { ApprenticeId = apprenticeId }, cancellationToken);

        if (myApprenticeship == null) return NotFound();

        return Ok(myApprenticeship);
    }

    [HttpPost]
    [ProducesResponseType(typeof(GetMyApprenticeshipQueryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> TryCreateMyApprenticeship([FromBody] TryCreateMyApprenticeshipRequestModel model, CancellationToken cancellationToken)
    {
        var myApprenticeship = await _mediator.Send(new GetMyApprenticeshipQuery { ApprenticeId = model.ApprenticeId }, cancellationToken);

        if (myApprenticeship != null) return Ok(myApprenticeship);

        GetRecentCommitmentQueryResult? commitment = null;
            
            //await _mediator.Send(new GetRecentCommitmentQuery(model.FirstName, model.LastName, model.DateOfBirth), cancellationToken);

        CreateMyApprenticeshipCommand? command = null;

        if (commitment != null)
        {
            command = commitment;
        }
        else
        {
            var stagedApprentice = await _mediator.Send(new GetStagedApprenticeQuery(model.LastName, model.DateOfBirth, model.Email), cancellationToken);
            if (stagedApprentice != null) command = stagedApprentice;
        }

        if (command != null)
        {
            command.ApprenticeId = model.ApprenticeId;
            await _mediator.Send(command, cancellationToken);

            myApprenticeship = await _mediator.Send(new GetMyApprenticeshipQuery { ApprenticeId = model.ApprenticeId }, cancellationToken);
            return Ok(myApprenticeship);
        }
        return NotFound();
    }
}