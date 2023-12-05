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
        // Arrange
        var patchDocument = new JsonPatchDocument<PatchMemberRequest>();
        request.patchMemberRequest.RegionId = 5;
        request.patchMemberRequest.OrganisationName = "OrganisationName";

        if (request.patchMemberRequest.RegionId > 0)
        {
            patchDocument.Replace(x => x.RegionId, request.patchMemberRequest.RegionId);
        }
        patchDocument.Replace(x => x.OrganisationName, request.patchMemberRequest.OrganisationName);
        patchDocument.ApplyTo(request.patchMemberRequest);

        // Act
        await sut.UpdateMemberProfileAndPreferences(memberId, request, cancellationToken);

        // Assert
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
        // Arrange
        if (!isMemberProfileAvailable)
        {
            request.updateMemberProfileRequest.MemberProfiles = new List<UpdateProfileModel>();
        }
        if (!isMemberPreferenceAvailable)
        {
            request.updateMemberProfileRequest.MemberPreferences = new List<UpdatePreferenceModel>();
        }

        // Act
        var result = await sut.UpdateMemberProfileAndPreferences(memberId, request, cancellationToken);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Test, MoqAutoData]
    public async Task UpdateMemberProfileAndPreferences_RegionIdIsValidAndOrganisationNameIsInvalid_ShouldInvokePatchMember(
        [Frozen] Mock<IAanHubRestApiClient> aanHubRestApiClientMock,
        [Greedy] MembersController sut,
        Guid memberId,
        UpdateMemberProfileModel request,
        int regionId,
        CancellationToken cancellationToken
    )
    {
        // Arrange
        request.patchMemberRequest.RegionId = regionId;
        request.patchMemberRequest.OrganisationName = string.Empty;

        // Act
        var result = await sut.UpdateMemberProfileAndPreferences(memberId, request, cancellationToken);

        // Assert
        aanHubRestApiClientMock.Verify(a => a.PatchMember(It.IsAny<Guid>(), It.IsAny<Guid>(), It.Is<JsonPatchDocument<PatchMemberRequest>>(j => j.Operations.Count == 1), It.IsAny<CancellationToken>()), Times.Once, "The Region Id is not added in the Operations array");

        aanHubRestApiClientMock.Verify(a => a.PatchMember(It.IsAny<Guid>(), It.IsAny<Guid>(), It.Is<JsonPatchDocument<PatchMemberRequest>>(j => j.Operations.First().path == "/RegionId"), It.IsAny<CancellationToken>()), Times.Once, "The Region Id path is not added to the Opearions array ");

        aanHubRestApiClientMock.Verify(a => a.PatchMember(It.IsAny<Guid>(), It.IsAny<Guid>(), It.Is<JsonPatchDocument<PatchMemberRequest>>(j => j.Operations.First().value.ToString() == regionId.ToString()), It.IsAny<CancellationToken>()), Times.Once, "The Region Id value is not added to the Opearions array ");
    }

    [Test, MoqAutoData]
    public async Task UpdateMemberProfileAndPreferences_OrganisationNameIsValidAndRegionIdIsInvalid_ShouldInvokePatchMember(
        [Frozen] Mock<IAanHubRestApiClient> aanHubRestApiClientMock,
        [Greedy] MembersController sut,
        Guid memberId,
        string organisationName,
        UpdateMemberProfileModel request,
        CancellationToken cancellationToken
    )
    {
        // Arrange
        request.patchMemberRequest.OrganisationName = organisationName;
        request.patchMemberRequest.RegionId = 0;

        //Act
        var result = await sut.UpdateMemberProfileAndPreferences(memberId, request, cancellationToken);

        //Assert
        aanHubRestApiClientMock.Verify(a => a.PatchMember(It.IsAny<Guid>(), It.IsAny<Guid>(), It.Is<JsonPatchDocument<PatchMemberRequest>>(j => j.Operations.Count == 1), It.IsAny<CancellationToken>()), Times.Once, "The Organisation Name is not added in the Operations array");

        aanHubRestApiClientMock.Verify(a => a.PatchMember(It.IsAny<Guid>(), It.IsAny<Guid>(), It.Is<JsonPatchDocument<PatchMemberRequest>>(j => j.Operations.First().path == "/OrganisationName"), It.IsAny<CancellationToken>()), Times.Once, "The Organisation Name path is not added to the Operations array ");

        aanHubRestApiClientMock.Verify(a => a.PatchMember(It.IsAny<Guid>(), It.IsAny<Guid>(), It.Is<JsonPatchDocument<PatchMemberRequest>>(j => j.Operations.First().value.ToString() == organisationName), It.IsAny<CancellationToken>()), Times.Once, "The Organisation Name value is not added to the Operations array ");
    }

    [Test, MoqAutoData]
    public async Task UpdateMemberProfileAndPreferences_OrganisationNameAndRegionIdIsValid_ShouldInvokePatchMember(
        [Frozen] Mock<IAanHubRestApiClient> aanHubRestApiClientMock,
        [Greedy] MembersController sut,
        Guid memberId,
        string organisationName,
        int regionId,
        UpdateMemberProfileModel request,
        CancellationToken cancellationToken
    )
    {
        // Arrange
        request.patchMemberRequest.RegionId = regionId;
        request.patchMemberRequest.OrganisationName = organisationName;

        // Act
        var result = await sut.UpdateMemberProfileAndPreferences(memberId, request, cancellationToken);

        // Assert
        aanHubRestApiClientMock.Verify(a => a.PatchMember(It.IsAny<Guid>(), It.IsAny<Guid>(), It.Is<JsonPatchDocument<PatchMemberRequest>>(j => j.Operations.Count == 2), It.IsAny<CancellationToken>()), Times.Once, "The Region Id and Organisation Name is not added in the Operations array");

        aanHubRestApiClientMock.Verify(a => a.PatchMember(It.IsAny<Guid>(), It.IsAny<Guid>(), It.Is<JsonPatchDocument<PatchMemberRequest>>(j => j.Operations.First().path == "/RegionId" && j.Operations.Last().path == "/OrganisationName"), It.IsAny<CancellationToken>()), Times.Once, "The Region Id and Organisation Name path is not added to the Operations array ");

        aanHubRestApiClientMock.Verify(a => a.PatchMember(It.IsAny<Guid>(), It.IsAny<Guid>(), It.Is<JsonPatchDocument<PatchMemberRequest>>(j => j.Operations.First().value.ToString() == regionId.ToString() && j.Operations.Last().value.ToString() == organisationName), It.IsAny<CancellationToken>()), Times.Once, "The Region Id and Organisation Name value is not added to the Operations array ");
    }

    [Test, MoqAutoData]
    public async Task UpdateMemberProfileAndPreferences_OrganisationNameAndRegionIdIsInValid_ShouldNotInvokePatchMember(
        [Frozen] Mock<IAanHubRestApiClient> aanHubRestApiClientMock,
        [Greedy] MembersController sut,
        Guid memberId,
        UpdateMemberProfileModel request,
        CancellationToken cancellationToken
    )
    {
        // Arrange
        request.patchMemberRequest.RegionId = 0;
        request.patchMemberRequest.OrganisationName = string.Empty;

        // Act
        var result = await sut.UpdateMemberProfileAndPreferences(memberId, request, cancellationToken);

        // Assert
        aanHubRestApiClientMock.Verify(a => a.PatchMember(It.IsAny<Guid>(), It.IsAny<Guid>(), It.Is<JsonPatchDocument<PatchMemberRequest>>(j => j.Operations.Count == 0), It.IsAny<CancellationToken>()), Times.Never, "The Region Id and Organisation Name is added in the operations array");
    }

    [Test, MoqAutoData]
    public async Task UpdateMemberProfileAndPreferences_MemberPreferenceIsEmptyAndMemberProfileHasValue_ShouldInvokePutMemberProfile(
        [Frozen] Mock<IAanHubRestApiClient> aanHubRestApiClientMock,
        [Greedy] MembersController sut,
        Guid memberId,
        UpdateMemberProfileModel request,
        CancellationToken cancellationToken
    )
    {
        // Arrange
        request.updateMemberProfileRequest.MemberPreferences = new List<UpdatePreferenceModel>();

        // Act
        var result = await sut.UpdateMemberProfileAndPreferences(memberId, request, cancellationToken);

        // Assert
        aanHubRestApiClientMock.Verify(x => x.PutMemberProfile(memberId, request.updateMemberProfileRequest, cancellationToken), Times.Once());
    }

    [Test, MoqAutoData]
    public async Task UpdateMemberProfileAndPreferences_MemberProfileIsEmptyAndMemberPreferenceHasValue_ShouldInvokePutMemberProfile(
        [Frozen] Mock<IAanHubRestApiClient> aanHubRestApiClientMock,
        [Greedy] MembersController sut,
        Guid memberId,
        UpdateMemberProfileModel request,
        CancellationToken cancellationToken
    )
    {
        // Arrange
        request.updateMemberProfileRequest.MemberProfiles = new List<UpdateProfileModel>();

        // Act
        var result = await sut.UpdateMemberProfileAndPreferences(memberId, request, cancellationToken);

        // Assert
        aanHubRestApiClientMock.Verify(x => x.PutMemberProfile(memberId, request.updateMemberProfileRequest, cancellationToken), Times.Once());
    }

    [Test, MoqAutoData]
    public async Task UpdateMemberProfileAndPreferences_MemberProfileAndMemberPreferenceAreEmpty_ShouldNotInvokePutMemberProfile(
        [Frozen] Mock<IAanHubRestApiClient> aanHubRestApiClientMock,
        [Greedy] MembersController sut,
        Guid memberId,
        UpdateMemberProfileModel request,
        CancellationToken cancellationToken
    )
    {
        // Arrange
        request.updateMemberProfileRequest.MemberProfiles = new List<UpdateProfileModel>();
        request.updateMemberProfileRequest.MemberPreferences = new List<UpdatePreferenceModel>();

        // Act
        var result = await sut.UpdateMemberProfileAndPreferences(memberId, request, cancellationToken);

        // Assert
        aanHubRestApiClientMock.Verify(x => x.PutMemberProfile(memberId, request.updateMemberProfileRequest, cancellationToken), Times.Never());
    }

    [Test, MoqAutoData]
    public async Task UpdateMemberProfileAndPreferences_MemberProfileAndMemberPreferenceHaveValue_ShouldInvokePutMemberProfile(
        [Frozen] Mock<IAanHubRestApiClient> aanHubRestApiClientMock,
        [Greedy] MembersController sut,
        Guid memberId,
        UpdateMemberProfileModel request,
        CancellationToken cancellationToken
    )
    {
        // Act
        var result = await sut.UpdateMemberProfileAndPreferences(memberId, request, cancellationToken);

        // Assert
        aanHubRestApiClientMock.Verify(x => x.PutMemberProfile(memberId, request.updateMemberProfileRequest, cancellationToken), Times.Once());
    }

    [Test]
    [MoqInlineAutoData("   test")]
    [MoqInlineAutoData("   test")]
    [MoqInlineAutoData("test  ")]
    [MoqInlineAutoData("   test  ")]
    [MoqInlineAutoData("")]
    public async Task UpdateMemberProfileAndPreferences_MemberProfileValueHasSpaces_ShouldTrimmedMemberProfileValue(
        string? profileValue,
        [Frozen] Mock<IAanHubRestApiClient> aanHubRestApiClientMock,
        [Greedy] MembersController sut,
        Guid memberId,
        UpdateMemberProfileModel request,
        CancellationToken cancellationToken
    )
    {
        // Arrange
        request.updateMemberProfileRequest.MemberProfiles = new List<UpdateProfileModel>() { new() { MemberProfileId = 1, Value = profileValue } };

        // Act
        var result = await sut.UpdateMemberProfileAndPreferences(memberId, request, cancellationToken);

        // Assert
        aanHubRestApiClientMock.Verify(a => a.PutMemberProfile(It.IsAny<Guid>(), It.Is<UpdateMemberProfileRequest>(j => j.MemberProfiles.Count == 1 && j.MemberProfiles.FirstOrDefault()!.Value == profileValue!.Trim()), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task UpdateMemberProfileAndPreferences_MemberProfileValueIsNull_ShouldTrimmedMemberProfileValue(
        [Frozen] Mock<IAanHubRestApiClient> aanHubRestApiClientMock,
        [Greedy] MembersController sut,
        Guid memberId,
        UpdateMemberProfileModel request,
        CancellationToken cancellationToken
    )
    {
        // Arrange
        string? profileValue = null;
        request.updateMemberProfileRequest.MemberProfiles = new List<UpdateProfileModel>() { new() { MemberProfileId = 1, Value = profileValue } };

        // Act
        var result = await sut.UpdateMemberProfileAndPreferences(memberId, request, cancellationToken);

        // Assert
        aanHubRestApiClientMock.Verify(a => a.PutMemberProfile(It.IsAny<Guid>(), It.Is<UpdateMemberProfileRequest>(j => j.MemberProfiles.Count == 1 && j.MemberProfiles.FirstOrDefault()!.Value == null), It.IsAny<CancellationToken>()), Times.Once);
    }

}