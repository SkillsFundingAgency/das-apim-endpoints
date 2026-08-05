using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Roatp.Application.Providers.Commands;
using SFA.DAS.Roatp.Domain.Models;
using SFA.DAS.Roatp.Infrastructure;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Roatp;

namespace SFA.DAS.Roatp.UnitTests.Providers.Commands.UpdateProviderNames;

public class UpdateProviderNamesCommandHandlerTests
{
    [Test, AutoData]
    public async Task WhenExecutingUpdateNames_GetsAllTheUkprnsFromRoatpApi(
        GetOrganisationsResponse organisationsResponse,
        UkrlpProvidersResponse ukrlpResponse)
    {
        var cancellationToken = CancellationToken.None;
        var roatpApiClient = new Mock<IRoatpApiClient>();
        roatpApiClient.Setup(x => x.GetOrganisations(cancellationToken)).ReturnsAsync(organisationsResponse);
        roatpApiClient.Setup(x => x.GetProvidersDataFromUkrlp(null, It.IsAny<int[]>(), It.IsAny<CancellationToken>())).ReturnsAsync(ukrlpResponse);
        var updateProviderNamesCommand = new UpdateProviderNamesCommand();
        var sut = new UpdateProviderNamesCommandHandler(roatpApiClient.Object, Mock.Of<ILogger<UpdateProviderNamesCommandHandler>>());

        await sut.Handle(updateProviderNamesCommand, cancellationToken);

        roatpApiClient.Verify(x => x.GetOrganisations(cancellationToken), Times.Once);
    }

    [Test, AutoData]
    public async Task WhenExecutingUpdateNames_BatchesCallsToUkrlpEndpoint(UkrlpProvidersResponse ukrlpResponse)
    {
        Fixture fixture = new();
        GetOrganisationsResponse organisationsResponse = new() { Organisations = [.. fixture.CreateMany<OrganisationResponse>(150)] };
        var cancellationToken = CancellationToken.None;
        var roatpApiClient = new Mock<IRoatpApiClient>();
        roatpApiClient.Setup(x => x.GetOrganisations(cancellationToken)).ReturnsAsync(organisationsResponse);
        roatpApiClient.Setup(x => x.GetProvidersDataFromUkrlp(null, It.IsAny<int[]>(), It.IsAny<CancellationToken>())).ReturnsAsync(ukrlpResponse);
        var updateProviderNamesCommand = new UpdateProviderNamesCommand();
        var sut = new UpdateProviderNamesCommandHandler(roatpApiClient.Object, Mock.Of<ILogger<UpdateProviderNamesCommandHandler>>());

        await sut.Handle(updateProviderNamesCommand, cancellationToken);

        roatpApiClient.Verify(x => x.GetProvidersDataFromUkrlp(null, It.IsAny<int[]>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        roatpApiClient.Verify(x => x.GetProvidersDataFromUkrlp(null, It.Is<int[]>(i => i.Length == 100), It.IsAny<CancellationToken>()), Times.Once);
        roatpApiClient.Verify(x => x.GetProvidersDataFromUkrlp(null, It.Is<int[]>(i => i.Length == 50), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task WhenExecutingUpdateNames_InvokesPutEndpointForEachProviderThatNeedsUpdating()
    {
        GetOrganisationsResponse organisationsResponse = new()
        {
            Organisations = [
                new OrganisationResponse { Ukprn = 10000001, LegalName = "unchanged", TradingName = "unchanged"},
                new OrganisationResponse { Ukprn = 10000002, LegalName = "changed", TradingName = "unchanged"},
                new OrganisationResponse { Ukprn = 10000003, LegalName = "unchanged", TradingName = "changed"},
                new OrganisationResponse { Ukprn = 10000004, LegalName = "changed", TradingName = "changed"}
            ]
        };

        UkrlpProvidersResponse ukrlpResponse = new(
        [
            new UkrlpProviderModel { Ukprn = 10000001, LegalName = "unchanged", TradingName = "unchanged"},
            new UkrlpProviderModel { Ukprn = 10000002, LegalName = "new", TradingName = "unchanged"},
            new UkrlpProviderModel { Ukprn = 10000003, LegalName = "unchanged", TradingName = "new"},
            new UkrlpProviderModel { Ukprn = 10000004, LegalName = "new", TradingName = "new"}
        ]);

        var cancellationToken = CancellationToken.None;
        var roatpApiClientMock = new Mock<IRoatpApiClient>();
        roatpApiClientMock.Setup(x => x.GetOrganisations(cancellationToken)).ReturnsAsync(organisationsResponse);
        roatpApiClientMock.Setup(x => x.GetProvidersDataFromUkrlp(null, It.IsAny<int[]>(), It.IsAny<CancellationToken>())).ReturnsAsync(ukrlpResponse);
        var updateProviderNamesCommand = new UpdateProviderNamesCommand();
        var sut = new UpdateProviderNamesCommandHandler(roatpApiClientMock.Object, Mock.Of<ILogger<UpdateProviderNamesCommandHandler>>());

        await sut.Handle(updateProviderNamesCommand, cancellationToken);

        roatpApiClientMock.Verify(x => x.PutOrganisation(10000001, It.IsAny<UpdateOrganisationModel>(), It.IsAny<CancellationToken>()), Times.Never);
        roatpApiClientMock.Verify(x => x.PutOrganisation(10000002, It.IsAny<UpdateOrganisationModel>(), It.IsAny<CancellationToken>()), Times.Once);
        roatpApiClientMock.Verify(x => x.PutOrganisation(10000003, It.IsAny<UpdateOrganisationModel>(), It.IsAny<CancellationToken>()), Times.Once);
        roatpApiClientMock.Verify(x => x.PutOrganisation(10000004, It.IsAny<UpdateOrganisationModel>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
