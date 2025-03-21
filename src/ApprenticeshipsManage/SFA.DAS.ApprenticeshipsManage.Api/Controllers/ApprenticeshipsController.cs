using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeshipsManage.Api.Models;
using SFA.DAS.ApprenticeshipsManage.Application.Queries.GetApprenticeships;

namespace SFA.DAS.ApprenticeshipsManage.Api.Controllers;

[ApiController]
public class ApprenticeshipsController(IMediator mediator, ILogger<ApprenticeshipsController> logger) : ControllerBase
{
    [HttpGet("providers/{ukprn}/academicyears/{academicyear}/apprenticeships")]
    public async Task<IActionResult> GetApprenticeships([FromRoute] string ukprn, [FromRoute] int academicyear, [FromQuery] int? page = 1, [FromQuery] int? pagesize = 20)
    {
        logger.LogInformation("GetApprenticeships for ukprn {ukprn}, year {year}", ukprn, academicyear);

        page ??= 1;
        pagesize = pagesize.HasValue ? Math.Clamp(pagesize.Value, 10, 100) : 20;

        var queryResult = await mediator.Send(new GetApprenticeshipsQuery()
        {
            Ukprn = ukprn,
            AcademicYear = academicyear,
            Page = page.Value,
            PageSize = pagesize.Value
           
        });
         
        return Ok((GetApprenticeshipsResponse)queryResult);
    }
}
