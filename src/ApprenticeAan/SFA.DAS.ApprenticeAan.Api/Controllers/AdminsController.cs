using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Application.Admins.Commands.Create;
using SFA.DAS.ApprenticeAan.Application.Admins.Queries.Lookup;

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
    [ProducesResponseType(typeof(LookupAdminMemberResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup([FromBody] LookupAdminMemberRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        if (result != null)
            return Ok(result);

        var createAdminResult = await _mediator.Send(new CreateAdminMemberCommand { Email = request.Email, FirstName = request.FirstName, LastName = request.LastName }, cancellationToken);

        return Ok(new LookupAdminMemberResult { MemberId = createAdminResult.MemberId, Status = "Live" });
    }
}
