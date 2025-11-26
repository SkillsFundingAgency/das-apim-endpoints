using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminRoatp.Application.Commands.CreateProvider;
using SFA.DAS.AdminRoatp.Application.Commands.PostOrganisation;
using SFA.DAS.AdminRoatp.Infrastructure;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Commands.PostOrganisation;
public class PostOrganisationCommandHandlerTests
{
    [Test, RecursiveMoqInlineAutoData]
    public async Task Handle_CallsPostOrganisation(
        [Frozen] Mock<IRoatpServiceRestApiClient> roatpServiceRestApiClientMock,
        [Greedy] PostOrganisatonCommandHandler sut,
        PostOrganisationCommand command,
        CancellationToken cancellationToken
        )
    {
        HttpResponseMessage response = new HttpResponseMessage { StatusCode = HttpStatusCode.Created };

        roatpServiceRestApiClientMock
            .Setup(x => x.PostOrganisation(It.IsAny<PostOrganisationModel>(), cancellationToken)).ReturnsAsync(response);

        var actualResponse = await sut.Handle(command, cancellationToken);

        actualResponse.Should().Be(response.StatusCode);
        roatpServiceRestApiClientMock.Verify(api => api.PostOrganisation(It.Is<PostOrganisationModel>(
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
            .Setup(x => x.PostOrganisation(It.IsAny<PostOrganisationModel>(), cancellationToken)).ReturnsAsync(response);

        var actualResponse = await sut.Handle(command, cancellationToken);

        actualResponse.Should().Be(HttpStatusCode.BadRequest);
        roatpServiceRestApiClientMock.Verify(api => api.PostOrganisation(It.Is<PostOrganisationModel>(
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
    [RecursiveMoqInlineAutoData(HttpStatusCode.Created, false, false, 0)]
    [RecursiveMoqInlineAutoData(HttpStatusCode.BadRequest, true, true, 0)]
    public async Task Handle_CallsPutCourseTypes(
        HttpStatusCode postOrganisationStatus,
        bool deliversApprenticeships,
        bool deliversApprenticeshipUnits,
        int callsPutCourseTypesCount,
        [Frozen] Mock<IRoatpServiceRestApiClient> roatpServiceRestApiClientMock,
        [Greedy] PostOrganisatonCommandHandler sut,
        int ukprn,
        CancellationToken cancellationToken
    )
    {
        var courseTypes = new List<int>();

        if (deliversApprenticeships) courseTypes.Add((int)CourseType.Apprenticeships);
        if (deliversApprenticeshipUnits) courseTypes.Add((int)CourseType.ApprenticeshipUnits);

        PostOrganisationCommand command = new PostOrganisationCommand(ukprn, "legal name", "", "", "", ProviderType.Main,
            1, "mark", "marky", deliversApprenticeships, deliversApprenticeshipUnits);
        HttpResponseMessage response = new HttpResponseMessage { StatusCode = postOrganisationStatus };

        roatpServiceRestApiClientMock
            .Setup(x => x.PostOrganisation(It.IsAny<PostOrganisationModel>(), cancellationToken)).ReturnsAsync(response);

        var actualResponse = await sut.Handle(command, cancellationToken);

        actualResponse.Should().Be(response.StatusCode);
        roatpServiceRestApiClientMock.Verify(api => api.PostOrganisation(It.Is<PostOrganisationModel>(
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

        roatpServiceRestApiClientMock.Verify(api => api.PutCourseTypes(ukprn, It.Is<PutCourseTypesModel>(
            c => c.UserId == command.RequestingUserDisplayName
            && c.CourseTypeIds.Count == courseTypes.Count
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
        [Frozen] Mock<IRoatpV2ApiClient> roatpV2ApiClientMock,
        [Greedy] PostOrganisatonCommandHandler sut,
        int ukprn,
        CancellationToken cancellationToken
    )
    {
        PostOrganisationCommand command = new PostOrganisationCommand(ukprn, "legal name", "", "", "", providerType,
            1, "mark", "marky", true, true);
        HttpResponseMessage response = new HttpResponseMessage { StatusCode = postOrganisationStatus };

        roatpServiceRestApiClientMock
            .Setup(x => x.PostOrganisation(It.IsAny<PostOrganisationModel>(), cancellationToken)).ReturnsAsync(response);

        HttpResponseMessage roatpV2Response = new HttpResponseMessage { StatusCode = HttpStatusCode.Created };
        roatpV2ApiClientMock
            .Setup(x => x.CreateProvider(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CreateProviderModel>(), cancellationToken)).ReturnsAsync(roatpV2Response);


        var actualResponse = await sut.Handle(command, cancellationToken);

        actualResponse.Should().Be(response.StatusCode);
        roatpServiceRestApiClientMock.Verify(api => api.PostOrganisation(It.Is<PostOrganisationModel>(
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

        roatpV2ApiClientMock.Verify(api => api.CreateProvider(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CreateProviderModel>(), cancellationToken), Times.Exactly(callsPostProvidersCount));
    }
}
