using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.RecruitJobs.Domain;
using SFA.DAS.RecruitJobs.InnerApi.Responses.Location;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Recruit;
using GeoCodeMethod = SFA.DAS.SharedOuterApi.Types.Domain.Recruit.GeoCodeMethod;

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

        var mappedAddresses = employerLocations.Select(x => new SharedOuterApi.Types.Models.Address
        {
            AddressLine1 = x.AddressLine1,
            AddressLine2 = x.AddressLine2,
            AddressLine3 = x.AddressLine3,
            AddressLine4 = x.AddressLine4,
            Latitude = x.Latitude,
            Longitude = x.Longitude,
            Postcode = x.Postcode,
        });
        
        var patchDocument = new JsonPatchDocument<PatchableVacancyDto>();
        patchDocument.Replace(x => x.EmployerLocations, mappedAddresses);
        patchDocument.Replace(x => x.GeoCodeMethod, GeoCodeMethod.OuterApi);
        var patchRequest = new PatchVacancyRequest(vacancyId, patchDocument);
        var patchResponse = await recruitApiClient.PatchWithResponseCode<JsonPatchDocument<PatchableVacancyDto>, NullResponse>(patchRequest, false);
        patchResponse.EnsureSuccessStatusCode();
        return TypedResults.NoContent();
    }
}

