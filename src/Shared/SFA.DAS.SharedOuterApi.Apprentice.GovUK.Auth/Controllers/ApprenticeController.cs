using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.SharedOuterApi.Apprentice.GovUK.Auth.Application.Commands;

namespace SFA.DAS.SharedOuterApi.Apprentice.GovUK.Auth.Controllers;

public abstract class ApprenticeControllerBase(IMediator mediator) : ControllerBase
{
    [HttpPut]
    [Route("/apprentices")]
    public async Task<IActionResult> PutApprentice([FromBody] UpsertApprenticeRequest request)
    {
        var queryResult = await mediator.Send(new UpsertApprenticeCommand
        {
            GovUkIdentifier = request.GovUkIdentifier,
            Email = request.Email
        });

            
        return Ok(queryResult.Apprentice);
    }
}


public class UpsertApprenticeRequest
{
    public string GovUkIdentifier { get; set; }
    public string Email { get; set; }
}