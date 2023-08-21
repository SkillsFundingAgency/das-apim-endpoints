using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminAan.Application.Admins.Commands.Create;
using SFA.DAS.AdminAan.Application.Admins.Queries.Lookup;
using SFA.DAS.AdminAan.Infrastructure.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;

namespace SFA.DAS.AdminAan.Api.Controllers;

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
        var result = await _mediator.Send((LookupAdminMemberRequestModel)request, cancellationToken);

        if (result != null) return Ok(result);

        var createAdminResult = await _mediator.Send((CreateAdminMemberCommand)request, cancellationToken);

        return Ok(new LookupAdminMemberResult { MemberId = createAdminResult.MemberId, Status = Constants.Status.Live.GetDescription() });
    }
}
