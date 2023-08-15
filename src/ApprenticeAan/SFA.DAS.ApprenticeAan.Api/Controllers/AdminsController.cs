using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Application.Admins.Commands.Create;
using SFA.DAS.ApprenticeAan.Application.Admins.Commands.Lookup;

namespace SFA.DAS.ApprenticeAan.Api.Controllers;

[Route("[controller]")]
public class AdminsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(LookupAdminMemberCommandResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup([FromBody] LookupAdminMemberCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (result != null)
            return Ok(result);

        var createAdminResult = await _mediator.Send(new CreateAdminMemberCommand { Email = command.Email, FirstName = command.FirstName, LastName = command.LastName }, cancellationToken);

        return Ok(new LookupAdminMemberCommandResult { MemberId = createAdminResult.MemberId, Status = "Live" });
    }
}
