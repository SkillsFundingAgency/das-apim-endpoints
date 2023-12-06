using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminAan.Api.Models.Admins;
using SFA.DAS.AdminAan.Application.Admins.Queries.Lookup;
using CreateAdminMemberCommand = SFA.DAS.AdminAan.Application.Admins.Commands.Create.CreateAdminMemberCommand;

namespace SFA.DAS.AdminAan.Api.Controllers;

[Route("[controller]")]
public class AdminsController : ControllerBase
{
    public const string LiveStatus = "Live";

    private readonly IMediator _mediator;

    public AdminsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(LookupAdminMemberResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup([FromBody] LookupAdminMemberRequestModel requestModel, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send((LookupAdminMemberRequest)requestModel, cancellationToken);

        if (result != null) return Ok(result);

        var createAdminResult = await _mediator.Send((CreateAdminMemberCommand)requestModel, cancellationToken);

        return Ok(new LookupAdminMemberResult { MemberId = createAdminResult.MemberId, Status = LiveStatus });
    }
}
