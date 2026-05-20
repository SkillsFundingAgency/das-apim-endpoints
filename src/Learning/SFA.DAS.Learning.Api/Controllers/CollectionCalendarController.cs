using Microsoft.AspNetCore.Mvc;
using SFA.DAS.CollectionCalendar.Contracts.ApiRequests;
using SFA.DAS.CollectionCalendar.Contracts.ApiResponses;
using SFA.DAS.CollectionCalendar.Contracts.Client;

namespace SFA.DAS.Learning.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CollectionCalendarController : ControllerBase
{
    private readonly ICollectionCalendarClient<CollectionCalendarConfiguration> _collectionCalendarClient;

    public CollectionCalendarController(ICollectionCalendarClient<CollectionCalendarConfiguration> collectionCalendarClient)
    {
        _collectionCalendarClient = collectionCalendarClient;
    }

    [HttpGet]
    [Route("academicYear/{searchDate}")]
    public async Task<IActionResult> GetAcademicYear(DateTime searchDate)
    {
        return Ok(await _collectionCalendarClient.Get<AcademicYearDetails>(new GetAcademicyearsApiRequest(searchDate)));
    }
}
