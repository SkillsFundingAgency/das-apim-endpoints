using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Controllers;
using SFA.DAS.ApprenticeAan.Api.Models;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.InnerApi.MemberProfiles;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Members;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Controllers.MembersControllerTests;

public class PutMembersTests
{
    [Test, RecursiveMoqAutoData]
    public async Task UpdateMemberProfileAndPreferences_InvokesPatchMemberAndPutMember(
        UpdateMemberProfileModel request,
        [Frozen] Mock<IAanHubRestApiClient> aanHubRestApiClientMock,
        [Greedy] MembersController sut,
        Guid memberId,
        CancellationToken cancellationToken)
    {
        var patchDocument = new JsonPatchDocument<PatchMemberRequest>();
        request.patchMemberRequest.RegionId = 5;
        request.patchMemberRequest.OrganisationName = "OrganisationName";

        if (request.patchMemberRequest.RegionId > 0)
        {
            patchDocument.Replace(x => x.RegionId, request.patchMemberRequest.RegionId);
        }
        patchDocument.Replace(x => x.OrganisationName, request.patchMemberRequest.OrganisationName);
        patchDocument.ApplyTo(request.patchMemberRequest);

        await sut.UpdateMemberProfileAndPreferences(memberId, request, cancellationToken);

        aanHubRestApiClientMock.Verify(x => x.PatchMember(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<JsonPatchDocument<PatchMemberRequest>>(), It.IsAny<CancellationToken>()), Times.Once());
        aanHubRestApiClientMock.Verify(x => x.PutMemberProfile(memberId, request.updateMemberProfileRequest, cancellationToken), Times.Once());
    }

    [Test]
    [MoqInlineAutoData(true, true, 1)]
    [MoqInlineAutoData(true, false, 5)]
    [MoqInlineAutoData(false, true, 1)]
    [MoqInlineAutoData(false, false, 0)]
    public async Task UpdateMemberProfileAndPreferences_ReturnsNoContent(
        bool isMemberProfileAvailable,
        bool isMemberPreferenceAvailable,
        int regionId,
        [Greedy] MembersController sut,
        Guid memberId,
        UpdateMemberProfileModel request,
        CancellationToken cancellationToken)
    {
        if (!isMemberProfileAvailable)
        {
            request.updateMemberProfileRequest.MemberProfiles = new List<UpdateProfileModel>();
        }
        if (!isMemberPreferenceAvailable)
        {
            request.updateMemberProfileRequest.MemberPreferences = new List<UpdatePreferenceModel>();
        }
        request.patchMemberRequest.RegionId = regionId;
        var result = await sut.UpdateMemberProfileAndPreferences(memberId, request, cancellationToken);

        result.Should().BeOfType<NoContentResult>();
    }
}