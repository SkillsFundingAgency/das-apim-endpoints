using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Api.Models;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Members;
using SFA.DAS.ApprenticeAan.Application.Members.Queries.GetMembers;

namespace SFA.DAS.ApprenticeAan.Api.Controllers;

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
            request.updateMemberProfileRequest.MemberProfiles.ForEach(obj => obj.Value = obj.Value?.Trim());
            await _apiClient.PutMemberProfile(memberId, request.updateMemberProfileRequest, cancellationToken);
        }

        return NoContent();
    }
}
