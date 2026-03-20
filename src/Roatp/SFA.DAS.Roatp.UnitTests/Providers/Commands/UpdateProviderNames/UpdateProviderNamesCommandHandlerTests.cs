using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Roatp.Application.Providers.Commands;
using SFA.DAS.Roatp.Domain.Models;
using SFA.DAS.Roatp.Infrastructure;
using SFA.DAS.SharedOuterApi.Infrastructure.Ukrlp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;

namespace SFA.DAS.Roatp.UnitTests.Providers.Commands.UpdateProviderNames;

public class UpdateProviderNamesCommandHandlerTests
{
    private Mock<IRoatpApiClient> _roatpApiClient;
    private readonly Mock<HttpClient> _httpClient = new();
    private readonly Mock<IUkrlpSoapSerializer> _ukrlpSoapSerializer = new();
    private readonly Mock<UkrlpApiConfiguration> _ukrlpConfiguration = new();
    private readonly UpdateProviderNamesCommand _updateProviderNamesCommand = new UpdateProviderNamesCommand();
    private UpdateProviderNamesCommandHandler _sut;
    private GetOrganisationsQueryResult _organisations;
    private string _ukrlpRequest;

    [SetUp]
    public void Setup()
    {
        _roatpApiClient = new Mock<IRoatpApiClient>();

        _sut = new UpdateProviderNamesCommandHandler(_roatpApiClient.Object,
            Mock.Of<ILogger<UpdateProviderNamesCommandHandler>>(), _ukrlpSoapSerializer.Object,
            _ukrlpConfiguration.Object, _httpClient.Object);

        _organisations = new GetOrganisationsQueryResult
        {
            Organisations = new List<OrganisationModel>
            {
                new OrganisationModel { Ukprn = 10000001, LegalName = "Name", TradingName = "Name"},
                new OrganisationModel { Ukprn = 10000002, LegalName = "Name", TradingName = "Name"},
                new OrganisationModel { Ukprn = 10000003, LegalName = "Name", TradingName = "Name"}
            }
        };

        _roatpApiClient.Setup(x => x.GetOrganisations()).ReturnsAsync(_organisations);

        _ukrlpRequest = "Test Request";

        _ukrlpSoapSerializer.Setup(x => x.BuildGetAllUkrlpsFromUkprnsSoapRequest(It.IsAny<List<long>>(),
                _ukrlpConfiguration.Object.StakeholderId, _ukrlpConfiguration.Object.QueryId))
            .Returns(_ukrlpRequest);

        var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);

        _httpClient.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), CancellationToken.None)).ReturnsAsync(responseMessage);
    }

    [Test]
    public async Task Handle_GetsOrganisationsFromRoatpApi()
    {
        await _sut.Handle(_updateProviderNamesCommand, CancellationToken.None);

        _roatpApiClient.Verify(x => x.GetOrganisations(), Times.Once);
    }

    [Test]
    public async Task Handle_BuildsUkprnGetRequest_GetsUkrlpData()
    {
        await _sut.Handle(_updateProviderNamesCommand, CancellationToken.None);

        _ukrlpSoapSerializer.Verify(
            x => x.BuildGetAllUkrlpsFromUkprnsSoapRequest(It.IsAny<List<long>>(),
                _ukrlpConfiguration.Object.StakeholderId, _ukrlpConfiguration.Object.QueryId), Times.Once);
    }

    [Test]
    [InlineAutoData(10000001, "10000004", "TestLegalName", "TestTradingName", "TestUkrlpName", "TestUkrlpTradingName", 0)]
    [InlineAutoData(10000004, "10000004", "TestName", "TestName", "TestName", "TestName", 0)]
    [InlineAutoData(10000004, "10000004", "TestName", "TestTradingName", "TestName", "TestName", 1)]
    [InlineAutoData(10000004, "10000004", "TestLegalName", "TestName", "TestName", "TestName", 1)]

    public async Task Handle_ComparesUkrlpNames_UpdatesIfNeeded(int orgUkprn, string ukrlpUkprn, string legalName,
        string tradingName, string ukrlpLegalName, string ukrlpTradingName, int numberOfTimesExecuted, ProviderContactAddress contactAddress)
    {
        _organisations.Organisations[0] = new OrganisationModel
        {
            Ukprn = orgUkprn,
            LegalName = legalName,
            TradingName = tradingName,
            ProviderType = ProviderType.Main,
            OrganisationTypeId = 1,
            ApplicationDeterminedDate = DateTime.UtcNow,
            CharityNumber = "Charity",
            CompanyNumber = "Company",
            OrganisationId = Guid.Empty
        };

        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);

        _httpClient.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), CancellationToken.None))
            .ReturnsAsync(responseMessage);

        var providerContacts = new List<ProviderContact> { new ProviderContact { ContactAddress = contactAddress, ContactType = "L" } };

        var matchingProviderRecords = new List<Provider>
        {
            new Provider
            {
                ProviderName = ukrlpLegalName,
                UnitedKingdomProviderReferenceNumber = ukrlpUkprn,
                ProviderContacts = providerContacts,
                ProviderAliases = new List<ProviderAlias>{new ProviderAlias
                {
                    Alias = ukrlpTradingName,
                    LastUpdated = DateTime.UtcNow.AddDays(-2)
                }}
            }
        };

        _ukrlpSoapSerializer.Setup(x => x.DeserialiseMatchingProviderRecordsResponse(It.IsAny<string>()))
            .Returns(matchingProviderRecords);

        await _sut.Handle(_updateProviderNamesCommand, CancellationToken.None);

        _roatpApiClient.Verify(x => x.PutOrganisation(It.IsAny<int>(), It.IsAny<UpdateOrganisationModel>()),
            Times.Exactly(numberOfTimesExecuted));
    }
}
