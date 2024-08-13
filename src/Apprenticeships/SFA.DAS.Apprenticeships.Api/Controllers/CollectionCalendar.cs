﻿using Microsoft.AspNetCore.Mvc;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Apprenticeships.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CollectionCalendar : ControllerBase
{
    private readonly ICollectionCalendarApiClient<CollectionCalendarApiConfiguration> _collectionCalendarApiClient;

    public CollectionCalendar(ICollectionCalendarApiClient<CollectionCalendarApiConfiguration> collectionCalendarApiClient)
    {
        _collectionCalendarApiClient = collectionCalendarApiClient;
    }

    [HttpGet]
    [Route("academicYear/{searchDate}")]
    public async Task<IActionResult> GetAcademicYear(DateTime searchDate)
    {
        return Ok(await _collectionCalendarApiClient.Get<GetAcademicYearsResponse>(new GetAcademicYearsRequest(searchDate)));
    }
}
