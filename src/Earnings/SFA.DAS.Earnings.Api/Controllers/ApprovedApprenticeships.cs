using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Earnings.Api.LearnerData;
using SFA.DAS.Earnings.Application.LearnerData.GetLearnerData;

namespace SFA.DAS.Earnings.Api.Controllers;

[ApiController]
[Route("approvedapprenticeships")]
public class ApprovedApprenticeshipsController(
    IMediator mediator) : Controller
{
    [HttpPost]
    [Route("/providers/{ukprn}/academicyears/{academicyear}/apprenticeships")]
    public async  Task<IActionResult> Search(long ukprn, int academicyear)
    {
        return Accepted();
    }   
}