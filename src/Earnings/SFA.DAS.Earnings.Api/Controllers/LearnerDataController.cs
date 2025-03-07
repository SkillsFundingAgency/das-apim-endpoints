using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.Earnings.Api.Controllers;

[ApiController]
[Route("learnerData")]
public class LearnerDataController : Controller
{
    [HttpPut]
    [Route("/providers/{ukprn}/academicyears/{academicyear}/apprenticeships")]
    public async Task<IActionResult> Search(long ukprn, int academicyear)
    {
        return Accepted( new { correlationId = 1231333 });
    }   
}