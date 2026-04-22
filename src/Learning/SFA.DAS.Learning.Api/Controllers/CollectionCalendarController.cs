using Microsoft.AspNetCore.Mvc;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.CollectionCalendar;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Learning.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CollectionCalendarController : ControllerBase
{
    private readonly ICollectionCalendarApiClient<CollectionCalendarApiConfiguration> _collectionCalendarApiClient;

    public CollectionCalendarController(ICollectionCalendarApiClient<CollectionCalendarApiConfiguration> collectionCalendarApiClient)
    {
        _collectionCalendarApiClient = collectionCalendarApiClient;
    }

    [HttpGet]
    [Route("academicYear/{searchDate}")]
    public async Task<IActionResult> GetAcademicYear(DateTime searchDate)
    {
        return Ok(await _collectionCalendarApiClient.Get<GetAcademicYearsResponse>(new GetAcademicYearByDateRequest(searchDate)));
    }
}
