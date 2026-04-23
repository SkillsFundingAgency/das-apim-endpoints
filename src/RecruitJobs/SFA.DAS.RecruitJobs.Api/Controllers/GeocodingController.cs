using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RecruitJobs.InnerApi.Responses.Location;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Domain.Recruit;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Recruit;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.RecruitJobs.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class GeocodingController(ILocationLookupService locationLookupService) : ControllerBase
{
    [HttpGet]
    [Route("postcode/{postcode}/geopoint")]
    public async Task<IResult> GetGeopoint([FromRoute] string postcode)
    {
        var response = await locationLookupService.GetLocationInformation(postcode, default, default);
        if (response is null)
        {
            return TypedResults.NotFound();
        }
        
        var result = new GetGeoPointResponse {
            GeoPoint = new GeoPoint
            {
                Latitude = response.GeoPoint[0],
                Longitude = response.GeoPoint[1]
            }
        };
        
        return TypedResults.Ok(result);
    }
    
    [HttpPost]
    [Route("vacancies/{vacancyId:guid}/geocoded")]
    public async Task<IResult> PostUpdateGeocodedLocations(
        [FromServices] IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
        [FromRoute] Guid vacancyId,
        [FromBody] List<Address> employerLocations,
        CancellationToken cancellationToken)
    {
        // Patch the Vacancy
        var patchDocument = new JsonPatchDocument<PatchableVacancyDto>();
        patchDocument.Replace(x => x.EmployerLocations, employerLocations);
        patchDocument.Replace(x => x.GeoCodeMethod, GeoCodeMethod.OuterApi);
        var patchRequest = new PatchVacancyRequest(vacancyId, patchDocument);
        var patchResponse = await recruitApiClient.PatchWithResponseCode<JsonPatchDocument<PatchableVacancyDto>, NullResponse>(patchRequest, false);
        patchResponse.EnsureSuccessStatusCode();
        return TypedResults.NoContent();
    }
}

