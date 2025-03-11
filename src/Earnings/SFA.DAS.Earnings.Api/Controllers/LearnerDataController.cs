using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Earnings.Application.LearnerRecord;

namespace SFA.DAS.Earnings.Api.Controllers;

[ApiController]
[Route("learnerData")]
public class LearnerDataController : Controller
{
    [HttpPut]
    [Route("/providers/{ukprn}/academicyears/{academicYear}/apprenticeships")]
    public async Task<IActionResult> Search([FromRoute]long ukprn, [FromRoute]int academicYear, [ModelBinder(BinderType = typeof(LearnerDataArrayBinder))]LearnerRecord[] records)
    {
        if (ModelState.IsValid)
        {
            return Accepted( new { correlationId = 1231333 });
        }

        return BadRequest();
    }   
}