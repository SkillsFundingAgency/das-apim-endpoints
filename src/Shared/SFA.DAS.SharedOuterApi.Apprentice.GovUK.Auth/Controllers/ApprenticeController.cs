using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.SharedOuterApi.Apprentice.GovUK.Auth.Application.Commands;

namespace SFA.DAS.SharedOuterApi.Apprentice.GovUK.Auth.Controllers;

public abstract class ApprenticeControllerBase(ISender sender) : ControllerBase
{
    [HttpPut]
    [Route("/apprentices")]
    public async Task<IActionResult> PutApprentice([FromBody] UpsertApprenticeRequest request)
    {
        var result = await sender.Send(new UpsertApprenticeCommand
        {
            GovUkIdentifier = request.GovUkIdentifier,
            Email = request.Email
        });

        return Ok(result.Apprentice);
    }
}

public class UpsertApprenticeRequest
{
    public string GovUkIdentifier { get; set; }
    public string Email { get; set; }
}