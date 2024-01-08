using System.Net;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerAan.Application.Members.Queries.GetMembers;
using SFA.DAS.EmployerAan.Application.Models;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.EmployerAan.InnerApi.Members;
using SFA.DAS.EmployerAan.InnerApi.Members.PostMemberLeaving;
using SFA.DAS.EmployerAan.Models;

namespace SFA.DAS.EmployerAan.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MembersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAanHubRestApiClient _apiClient;

    public MembersController(IMediator mediator, IAanHubRestApiClient apiClient)
    {
        _mediator = mediator;
        _apiClient = apiClient;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetMembersQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMembers([FromQuery] GetMembersRequestModel requestModel, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send((GetMembersQuery)requestModel, cancellationToken);
        return Ok(response);
    }

    [HttpPut("{memberId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateMemberProfileAndPreferences(
        [FromRoute] Guid memberId,
        [FromBody] UpdateMemberProfileModel request,
        CancellationToken cancellationToken)
    {
        if (request.patchMemberRequest.RegionId > 0 || !string.IsNullOrEmpty(request.patchMemberRequest.OrganisationName))
        {
            PatchMemberRequest patchMemberRequest = request.patchMemberRequest;
            JsonPatchDocument<PatchMemberRequest> jsonPatchDocument = new JsonPatchDocument<PatchMemberRequest>();
            if (patchMemberRequest.RegionId > 0)
            {
                jsonPatchDocument.Replace(x => x.RegionId, patchMemberRequest.RegionId);
            }
            if (!string.IsNullOrEmpty(patchMemberRequest.OrganisationName))
            {
                jsonPatchDocument.Replace(x => x.OrganisationName, patchMemberRequest.OrganisationName);
            }
            jsonPatchDocument.ApplyTo(patchMemberRequest);
            await _apiClient.PatchMember(memberId, memberId, jsonPatchDocument, cancellationToken);
        }

        if (request.updateMemberProfileRequest.MemberProfiles.Any() || request.updateMemberProfileRequest.MemberPreferences.Any())
        {
            await _apiClient.PutMemberProfile(memberId, request.updateMemberProfileRequest, cancellationToken);
        }

        return NoContent();
    }

    [HttpPost("{memberId}/Leaving")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PostMemberLeavingReasons([FromRoute] Guid memberId, [FromBody] PostMemberLeavingModel model, CancellationToken cancellationToken)
    {
        var response = await _apiClient.PostMembersLeaving(memberId, model, cancellationToken);

        return response.StatusCode switch
        {
            HttpStatusCode.NoContent => NoContent(),
            HttpStatusCode.NotFound => NotFound(),
            _ => throw new InvalidOperationException("Post member leaving didn't come back with a successful response")
        };
    }
}
