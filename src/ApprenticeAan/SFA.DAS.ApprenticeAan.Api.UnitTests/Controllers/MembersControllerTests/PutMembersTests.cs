using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Controllers;
using SFA.DAS.ApprenticeAan.Api.Models;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Members;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Controllers.MembersControllerTests;

public class PutMembersTests
{
    Mock<IAanHubRestApiClient> aanHubRestApiClientMock = null!;

    [Test, RecursiveMoqAutoData]
    public async Task UpdateMemberProfileAndPreferences_InvokesPatchMemberAndPutMember(
        [Greedy] MembersController sut,
        UpdateMemberProfileModel request,
        Guid memberId,
        CancellationToken cancellationToken)
    {
        var patchDocument = new JsonPatchDocument<PatchMemberRequest>();
        aanHubRestApiClientMock = new();
        request.patchMemberRequest.RegionId = 5;
        request.patchMemberRequest.OrganisationName = "OrganisationName";

        if (request.patchMemberRequest.RegionId > 0)
        {
            patchDocument.Replace(x => x.RegionId, request.patchMemberRequest.RegionId);
        }
        patchDocument.Replace(x => x.OrganisationName, request.patchMemberRequest.OrganisationName);
        patchDocument.ApplyTo(request.patchMemberRequest);

        await sut.UpdateMemberProfileAndPreferences(memberId, request, cancellationToken);

        aanHubRestApiClientMock.Verify(x => x.PatchMember(memberId, memberId, patchDocument, cancellationToken), Times.Once());
        aanHubRestApiClientMock.Verify(x => x.PutMemberProfile(memberId, request.updateMemberProfileRequest, cancellationToken), Times.Once());
    }

    [Test, RecursiveMoqAutoData]
    public async Task UpdateMemberProfileAndPreferences_ReturnsNoContent(
    [Greedy] MembersController sut,
    Guid memberId,
    UpdateMemberProfileModel request,
    CancellationToken cancellationToken)
    {
        var result = await sut.UpdateMemberProfileAndPreferences(memberId, request, cancellationToken);

        result.Should().BeOfType<NoContentResult>();
    }
}