using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeshipsManage.Api.Models;
using SFA.DAS.ApprenticeshipsManage.Application.Queries.GetApprenticeships;

namespace SFA.DAS.ApprenticeshipsManage.Api.Controllers;

[ApiController]
public class ApprenticeshipsController(IMediator mediator, ILogger<ApprenticeshipsController> logger) : ControllerBase
{
    [HttpGet("providers/{ukprn}/apprenticeships")]
    public async Task<IActionResult> GetApprenticeships([FromRoute] string ukprn, [FromQuery] string searchDate, [FromQuery] int? page = 1, [FromQuery] int? pagesize = 20)
    {
        logger.LogInformation("GetApprenticeships for ukprn {ukprn}, year {year}", ukprn, searchDate);

        var validDate = DateTime.TryParse(searchDate, out var searchDateValue);

        if (!validDate)
        {
            return new BadRequestResult();
        }

        page ??= 1;
        pagesize = pagesize.HasValue ? Math.Clamp(pagesize.Value, 10, 100) : 20;

        var queryResult = await mediator.Send(new GetApprenticeshipsQuery()
        {
            Ukprn = ukprn,
            AcademicYearDate = searchDateValue,
            Page = page.Value,
            PageSize = pagesize.Value
           
        });
         
        return Ok((GetApprenticeshipsResponse)queryResult);
    }
}
