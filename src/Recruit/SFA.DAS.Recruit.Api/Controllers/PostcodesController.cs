using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.GetBulkPostcodeData;
using SFA.DAS.Recruit.Application.Queries.GetPostcodeData;

namespace SFA.DAS.Recruit.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class PostcodesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Route("{postcode}")]
    public async Task<IActionResult> GetPostcodeData(string postcode, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(postcode))
        {
            return BadRequest();
        }
        
        var result = await mediator.Send(new GetPostcodeDataQuery(postcode), cancellationToken);
        return result == GetPostcodeDataResult.None
            ? NotFound()
            : Ok(GetPostcodeDataResponse.From(postcode, result));
    }
    
    [HttpPost]
    [Route("bulk")]
    public async Task<IActionResult> GetBulkPostcodeData([Required] List<string> postcodes, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetBulkPostcodeDataQuery(postcodes), cancellationToken);
        return Ok(result.Results);
    }
}