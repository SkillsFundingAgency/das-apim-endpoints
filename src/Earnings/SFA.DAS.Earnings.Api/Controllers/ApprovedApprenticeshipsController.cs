using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Earnings.Api.LearnerData;
using SFA.DAS.Earnings.Application.ApprovedApprenticeships.GetApprovedApprenticeships;

namespace SFA.DAS.Earnings.Api.Controllers;

[ApiController]
[Route("approvedApprenticeships")]
public class ApprovedApprenticeshipsController(
    IMediator mediator) : Controller
{
    [HttpGet]
    [Route("/providers/{ukprn}/academicyears/{academicyear}/apprenticeships")]
    public async  Task<IActionResult> Search(long ukprn, int academicyear, [FromQuery] int page, [FromQuery] int pageSize)
    {
        if (pageSize is < 1 or > 100)
        {
            return BadRequest("Page size must be between 1 and 100");
        }
        
        if (page < 1)
        {
            return BadRequest("Invalid page requested");
        }
        
        var result = await mediator.Send(new GetApprovedApprenticeshipsQuery(ukprn, academicyear, page, pageSize));
        var response = new PagedResult
        {
            Apprenticeships = result.Apprenticeships.ConvertAll(app => new Apprenticeship { Uln = app.Uln }),
            TotalRecords = result.TotalRecords,
            Page = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling((double)result.TotalRecords / pageSize)
        };
                
        // Add Link headers for pagination
        var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}";
        var nextPage = page * pageSize < result.TotalRecords ? page + 1 : page;
        var prevPage = page > 1 ? page - 1 : 1;

        var header = "";
        
        if (prevPage != page)
        {
            header += $"<{baseUrl}?page={prevPage}&pageSize={pageSize}>; rel=\"prev\", ";
        } 
        
        if (nextPage != page)
        {
            if (header.Length > 0)
            {
                header += ", ";
            }
            
            header += $"<{baseUrl}?page={nextPage}&pageSize={pageSize}>; rel=\"next\"";
        }

        if (header.Length > 0)
        {
            Response.Headers.Link = header;
        }
        
        return Ok(response);
    }   
}