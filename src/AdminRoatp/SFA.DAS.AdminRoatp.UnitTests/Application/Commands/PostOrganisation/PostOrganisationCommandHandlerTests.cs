using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminRoatp.Application.Commands.PostOrganisation;
using SFA.DAS.AdminRoatp.Infrastructure;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Commands.PostOrganisation;
public class PostOrganisationCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_CallsPostOrganisation(
        [Frozen] Mock<IRoatpServiceRestApiClient> roatpServiceRestApiClientMock,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> roatpV2ApiClientMock,
        [Greedy] PostOrganisatonCommandHandler sut,
        PostOrganisationCommand command,
        CancellationToken cancellationToken
        )
    {
        command.ProviderType = ProviderType.Employer;
        HttpResponseMessage response = new HttpResponseMessage { StatusCode = HttpStatusCode.Created };

        roatpServiceRestApiClientMock
            .Setup(x => x.PostOrganisation(It.IsAny<PostOrganisationRequest>(), cancellationToken)).ReturnsAsync(response);

        var actualResponse = await sut.Handle(command, cancellationToken);

        actualResponse.Should().Be(response.StatusCode);
        roatpServiceRestApiClientMock.Verify(api => api.PostOrganisation(It.Is<PostOrganisationRequest>(
            c => c.Ukprn == command.Ukprn
            && c.LegalName == command.LegalName
            && c.TradingName == command.TradingName
            && c.CompanyNumber == command.CompanyNumber
            && c.CharityNumber == command.CharityNumber
            && c.ProviderType == command.ProviderType
            && c.OrganisationTypeId == command.OrganisationTypeId
            && c.RequestingUserId == command.RequestingUserDisplayName
            ), cancellationToken), Times.Once);
    }

    [Test, RecursiveMoqInlineAutoData]
    public async Task Handle_CallsPostOrganisation_Unsuccessful(
        [Frozen] Mock<IRoatpServiceRestApiClient> roatpServiceRestApiClientMock,
        [Greedy] PostOrganisatonCommandHandler sut,
        PostOrganisationCommand command,
        CancellationToken cancellationToken
    )
    {
        HttpResponseMessage response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest };

        roatpServiceRestApiClientMock
            .Setup(x => x.PostOrganisation(It.IsAny<PostOrganisationRequest>(), cancellationToken)).ReturnsAsync(response);

        var actualResponse = await sut.Handle(command, cancellationToken);

        actualResponse.Should().Be(HttpStatusCode.BadRequest);
        roatpServiceRestApiClientMock.Verify(api => api.PostOrganisation(It.Is<PostOrganisationRequest>(
            c => c.Ukprn == command.Ukprn
                 && c.LegalName == command.LegalName
                 && c.TradingName == command.TradingName
                 && c.CompanyNumber == command.CompanyNumber
                 && c.CharityNumber == command.CharityNumber
                 && c.ProviderType == command.ProviderType
                 && c.OrganisationTypeId == command.OrganisationTypeId
                 && c.RequestingUserId == command.RequestingUserDisplayName
        ), cancellationToken), Times.Once);
    }

    [Test]
    [RecursiveMoqInlineAutoData(HttpStatusCode.Created, true, true, 1)]
    [RecursiveMoqInlineAutoData(HttpStatusCode.Created, true, false, 1)]
    [RecursiveMoqInlineAutoData(HttpStatusCode.Created, false, true, 1)]
    [RecursiveMoqInlineAutoData(HttpStatusCode.Created, false, false, 1)]
    [RecursiveMoqInlineAutoData(HttpStatusCode.BadRequest, true, true, 0)]
    public async Task Handle_CallsPutCourseTypes(
        HttpStatusCode postOrganisationStatus,
        bool deliversApprenticeships,
        bool deliversApprenticeshipUnits,
        int callsPutCourseTypesCount,
        [Frozen] Mock<IRoatpServiceRestApiClient> roatpServiceRestApiClientMock,
        [Greedy] PostOrganisatonCommandHandler sut,
        PostOrganisationCommand command,
        int ukprn,
        CancellationToken cancellationToken
    )
    {
        command.Ukprn = ukprn;
        command.DeliversApprenticeships = deliversApprenticeships;
        command.DeliversApprenticeshipUnits = deliversApprenticeshipUnits;
        command.ProviderType = ProviderType.Employer;

        var courseTypes = new List<int>();

        if (deliversApprenticeships) courseTypes.Add((int)CourseType.Apprenticeship);
        if (deliversApprenticeshipUnits) courseTypes.Add((int)CourseType.ApprenticeshipUnit);


        HttpResponseMessage response = new HttpResponseMessage { StatusCode = postOrganisationStatus };

        roatpServiceRestApiClientMock
            .Setup(x => x.PostOrganisation(It.IsAny<PostOrganisationRequest>(), cancellationToken)).ReturnsAsync(response);

        var actualResponse = await sut.Handle(command, cancellationToken);

        actualResponse.Should().Be(response.StatusCode);
        roatpServiceRestApiClientMock.Verify(api => api.PostOrganisation(It.Is<PostOrganisationRequest>(
            c => c.Ukprn == command.Ukprn
                 && c.Ukprn == ukprn
                 && c.LegalName == command.LegalName
                 && c.TradingName == command.TradingName
                 && c.CompanyNumber == command.CompanyNumber
                 && c.CharityNumber == command.CharityNumber
                 && c.ProviderType == command.ProviderType
                 && c.OrganisationTypeId == command.OrganisationTypeId
                 && c.RequestingUserId == command.RequestingUserDisplayName
        ), cancellationToken), Times.Once);

        roatpServiceRestApiClientMock.Verify(api => api.PutCourseTypes(ukprn, It.Is<UpdateCourseTypesModel>(
            c => c.UserId == command.RequestingUserDisplayName
            && c.CourseTypeIds.Count() == courseTypes.Count
            ), cancellationToken), Times.Exactly(callsPutCourseTypesCount));
    }


    [Test]
    [RecursiveMoqInlineAutoData(HttpStatusCode.Created, ProviderType.Main, 1)]
    [RecursiveMoqInlineAutoData(HttpStatusCode.BadRequest, ProviderType.Main, 0)]
    [RecursiveMoqInlineAutoData(HttpStatusCode.Created, ProviderType.Employer, 0)]
    [RecursiveMoqInlineAutoData(HttpStatusCode.Created, ProviderType.Supporting, 0)]
    public async Task Handle_CallsCreateProvider(
        HttpStatusCode postOrganisationStatus,
        ProviderType providerType,
        int callsPostProvidersCount,
        [Frozen] Mock<IRoatpServiceRestApiClient> roatpServiceRestApiClientMock,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> roatpV2ApiClientMock,
        [Greedy] PostOrganisatonCommandHandler sut,
        PostOrganisationCommand command,
        int ukprn,
        CancellationToken cancellationToken
    )
    {
        command.Ukprn = ukprn;
        command.ProviderType = providerType;

        HttpResponseMessage response = new HttpResponseMessage { StatusCode = postOrganisationStatus };

        roatpServiceRestApiClientMock
            .Setup(x => x.PostOrganisation(It.IsAny<PostOrganisationRequest>(), cancellationToken)).ReturnsAsync(response);

        var apiResponse = new ApiResponse<int>(1, HttpStatusCode.Created, string.Empty);

        roatpV2ApiClientMock.Setup(c => c.PostWithResponseCode<int>(It.IsAny<PostProviderRequest>(
        ), true)).ReturnsAsync(apiResponse);

        var actualResponse = await sut.Handle(command, cancellationToken);

        actualResponse.Should().Be(response.StatusCode);
        roatpServiceRestApiClientMock.Verify(api => api.PostOrganisation(It.Is<PostOrganisationRequest>(
            c => c.Ukprn == command.Ukprn
                 && c.Ukprn == ukprn
                 && c.LegalName == command.LegalName
                 && c.TradingName == command.TradingName
                 && c.CompanyNumber == command.CompanyNumber
                 && c.CharityNumber == command.CharityNumber
                 && c.ProviderType == command.ProviderType
                 && c.OrganisationTypeId == command.OrganisationTypeId
                 && c.RequestingUserId == command.RequestingUserDisplayName
        ), cancellationToken), Times.Once);

        roatpV2ApiClientMock.Verify(c => c.PostWithResponseCode<int>(It.Is<PostProviderRequest>(
            c => c.UserId == command.RequestingUserId
            && c.UserDisplayName == command.RequestingUserDisplayName
            && c.PostUrl == $"providers?userId={command.RequestingUserId}&userDisplayName={command.RequestingUserDisplayName}"
        ), true), Times.Exactly(callsPostProvidersCount));

    }
}
