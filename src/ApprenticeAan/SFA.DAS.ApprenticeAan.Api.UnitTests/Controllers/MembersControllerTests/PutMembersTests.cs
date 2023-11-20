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
    [MoqInlineAutoData(true, true)]
    [MoqInlineAutoData(true, false)]
    [MoqInlineAutoData(false, true)]
    [MoqInlineAutoData(false, false)]
    public async Task UpdateMemberProfileAndPreferences_ReturnsNoContent(
    bool isMemberProfileAvailable,
    bool isMemberPreferenceAvailable,
    [Greedy] MembersController sut,
    Guid memberId,
    UpdateMemberProfileModel request,
    CancellationToken cancellationToken)
    {
        if (!isMemberProfileAvailable)
        {
            request.updateMemberProfileRequest.Profiles = Enumerable.Empty<UpdateProfileModel>();
        }
        if (!isMemberPreferenceAvailable)
        {
            request.updateMemberProfileRequest.Preferences = Enumerable.Empty<UpdatePreferenceModel>();
        }

        var result = await sut.UpdateMemberProfileAndPreferences(memberId, request, cancellationToken);

        result.Should().BeOfType<NoContentResult>();
    }
}