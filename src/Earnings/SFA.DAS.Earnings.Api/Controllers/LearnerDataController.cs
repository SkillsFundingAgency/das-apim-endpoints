using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Earnings.Application.LearnerRecord;

namespace SFA.DAS.Earnings.Api.Controllers;

[ApiController]
[Route("learnerData")]
public class LearnerDataController : Controller
{
    [HttpPut]
    [Route("/providers/{ukprn}/academicyears/{academicyear}/apprenticeships")]
    public async Task<IActionResult> Search(
        [FromRoute]long ukprn,
        [FromRoute]int academicyear,
        [FromBody] LearnerRecord[] records)
    {
        if (ModelState.IsValid)
        {
            return Accepted( new { correlationId = 1231333 });
        }

        return BadRequest();
    }   
}