using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class SavedSearchesController(IMediator mediator) : ControllerBase
{
    
}