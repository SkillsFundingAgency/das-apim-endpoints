using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeshipsManage.Api.Models;
using SFA.DAS.ApprenticeshipsManage.Application.Queries.GetApprenticeships;
using SFA.DAS.ApprenticeshipsManage.InnerApi.Services;

namespace SFA.DAS.ApprenticeshipsManage.Api.Controllers;

[ApiController]
public class ApprenticeshipsController(IMediator mediator, 
    ILogger<ApprenticeshipsController> logger,
    IPagedLinkHeaderService pagedLinkHeaderService) : ControllerBase
{
    [HttpGet("providers/{ukprn}/academicyears/{academicyear}/apprenticeships")]
    public async Task<IActionResult> GetApprenticeships([FromRoute] string ukprn, [FromRoute] int academicyear, [FromQuery] int page = 1, [FromQuery] int? pagesize = 20)
    {
        logger.LogInformation("GetApprenticeships for ukprn {Ukprn}, year {Year}", ukprn, academicyear);

        pagesize = pagesize.HasValue ? Math.Clamp(pagesize.Value, 1, 100) : pagesize;

        var query = new GetApprenticeshipsQuery()
        {
            Ukprn = ukprn,
            AcademicYear = academicyear,
            Page = page,
            PageSize = pagesize
        };

        var response = await mediator.Send(query);

        var pageLinks = pagedLinkHeaderService.GetPageLinks(query, response);
        Response.Headers.Add(pageLinks);

        return Ok((GetApprenticeshipsResponse)response);
    }
}