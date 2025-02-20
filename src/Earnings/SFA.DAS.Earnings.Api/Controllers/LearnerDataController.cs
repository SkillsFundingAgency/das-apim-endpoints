using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Earnings.Api.Learnerdata;

namespace SFA.DAS.Earnings.Api.Controllers;

[ApiController]
[Route("leanerdata")]
public class LearnerDataController : Controller
{
    private readonly ILearnerDataSearchService _service;

    public LearnerDataController(ILearnerDataSearchService service)
    {
        _service = service;
    }
    
    [HttpGet]
    [Route("/providers/{ukprn}/academicyears/{academicyear}/apprenticeships")]
    public IActionResult Search(uint ukprn, uint academicyear, [FromQuery] uint page, [FromQuery] uint pageSize)
    {
        if (pageSize < 1 || pageSize > 100)
        {
            return BadRequest("Page size must be between 1 and 100");
        }
        
        if (page < 1)
        {
            return BadRequest("Invalid page requested");
        }
        
        var result = _service.Search(ukprn, academicyear, page==0?1:page, pageSize==0?20:pageSize);
                
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
            Response.Headers["Link"] = header;
        }
        
        return Ok(result);
    }   
}