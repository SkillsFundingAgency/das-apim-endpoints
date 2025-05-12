using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using RestEase;
using SFA.DAS.ProviderPR.Application.Relationships.Queries.GetRelationshipsByUkprnPayeAorn;
using SFA.DAS.ProviderPR.Infrastructure;
using SFA.DAS.ProviderPR.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.PensionRegulator;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.PensionsRegulator;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ProviderPR.UnitTests.Application.Queries.GetRelationshipsByUkprnPayeAorn;

public class GetRelationshipByUkprnPayeAornQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_GetsRequestFoundFromPRApi_ReturnsHasActiveRequestTrue(
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClientMock,
        long ukprn,
        string paye,
        string aorn,
        GetRequestByUkprnAndPayeResponse expected,
        CancellationToken cancellationToken
    )
    {
        var encodedPaye = Uri.EscapeDataString(paye);

        expected.RequestType = "CreateAccount";

        providerRelationshipsApiRestClientMock.Setup(x =>
            x.GetRequestByUkprnAndPaye(
                ukprn,
                encodedPaye,
                cancellationToken
            )
        )!.ReturnsAsync(
            new Response<GetRequestByUkprnAndPayeResponse>(
                string.Empty,
                new(HttpStatusCode.OK),
                () => expected
            ));

        var request = new GetRelationshipsByUkprnPayeAornQuery(ukprn, aorn, paye);

        GetRelationshipByUkprnPayeAornQueryHandler handler = new GetRelationshipByUkprnPayeAornQueryHandler(Mock.Of<IPensionRegulatorApiClient<PensionRegulatorApiConfiguration>>(), providerRelationshipsApiRestClientMock.Object, Mock.Of<IAccountsApiClient<AccountsConfiguration>>(), Mock.Of<ILogger<GetRelationshipByUkprnPayeAornQueryHandler>>());

        var actual = await handler.Handle(request, cancellationToken);

        var expectedResponse = new GetRelationshipsByUkprnPayeAornResult { HasActiveRequest = true };

        actual.Should().BeEquivalentTo(expectedResponse);
        providerRelationshipsApiRestClientMock.Verify(r => r.GetRequestByUkprnAndPaye(ukprn, encodedPaye, cancellationToken), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_GetsNoResultsFromPensionApi_ReturnsHasInvalidPayeTrue(
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClientMock,
        [Frozen] Mock<IPensionRegulatorApiClient<PensionRegulatorApiConfiguration>> pensionRegulatorApiClientMock,
        GetRelationshipByUkprnPayeAornQueryHandler sut,
        long ukprn,
        string paye,
        string aorn,
        GetRequestByUkprnAndPayeResponse expected,
        CancellationToken cancellationToken
    )
    {
        var encodedPaye = Uri.EscapeDataString(paye);

        SetupPrMockForGetRequestByUkprnAndPaye(providerRelationshipsApiRestClientMock, ukprn, expected, encodedPaye, cancellationToken);

        var pensionResponse = new ApiResponse<IEnumerable<PensionRegulatorOrganisation>>(new List<PensionRegulatorOrganisation>(), HttpStatusCode.NotFound, "");

        pensionRegulatorApiClientMock
            .Setup(p => p.GetWithResponseCode<IEnumerable<PensionRegulatorOrganisation>>(It.IsAny<GetPensionsRegulatorOrganisationsRequest>()))
            .ReturnsAsync(pensionResponse);

        var request = new GetRelationshipsByUkprnPayeAornQuery(ukprn, aorn, paye);

        var actual = await sut.Handle(request, cancellationToken);

        var expectedResponse = new GetRelationshipsByUkprnPayeAornResult { HasActiveRequest = false, HasInvalidPaye = true };

        actual.Should().BeEquivalentTo(expectedResponse);
        providerRelationshipsApiRestClientMock.Verify(r => r.GetRequestByUkprnAndPaye(ukprn, encodedPaye, cancellationToken), Times.Once);
        pensionRegulatorApiClientMock.Verify(r => r.GetWithResponseCode<IEnumerable<PensionRegulatorOrganisation>>(It.IsAny<GetPensionsRegulatorOrganisationsRequest>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_PensionsApiReturnsNoResults_ReturnsHasInvalidPayeTrue(
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClientMock,
        [Frozen] Mock<IPensionRegulatorApiClient<PensionRegulatorApiConfiguration>> pensionRegulatorApiClientMock,
        GetRelationshipByUkprnPayeAornQueryHandler sut,
        long ukprn,
        string paye,
        string aorn,
        GetRequestByUkprnAndPayeResponse expected,
        CancellationToken cancellationToken
    )
    {
        var encodedPaye = Uri.EscapeDataString(paye);

        SetupPrMockForGetRequestByUkprnAndPaye(providerRelationshipsApiRestClientMock, ukprn, expected, encodedPaye, cancellationToken);

        var pensionResponse = new ApiResponse<IEnumerable<PensionRegulatorOrganisation>>(Enumerable.Empty<PensionRegulatorOrganisation>(), HttpStatusCode.OK, string.Empty);

        pensionRegulatorApiClientMock
            .Setup(p => p.GetWithResponseCode<IEnumerable<PensionRegulatorOrganisation>>(It.IsAny<GetPensionsRegulatorOrganisationsRequest>()))
            .ReturnsAsync(pensionResponse);

        var request = new GetRelationshipsByUkprnPayeAornQuery(ukprn, aorn, paye);

        var actual = await sut.Handle(request, cancellationToken);

        var expectedResponse = new GetRelationshipsByUkprnPayeAornResult { HasActiveRequest = false, HasInvalidPaye = true };

        actual.Should().BeEquivalentTo(expectedResponse);
        providerRelationshipsApiRestClientMock.Verify(r => r.GetRequestByUkprnAndPaye(ukprn, encodedPaye, cancellationToken), Times.Once);
        pensionRegulatorApiClientMock.Verify(r => r.GetWithResponseCode<IEnumerable<PensionRegulatorOrganisation>>(It.IsAny<GetPensionsRegulatorOrganisationsRequest>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_PensionsApiReturnsResultsWithInvalidStatus_ReturnsHasInvalidPayeTrue(
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClientMock,
        [Frozen] Mock<IPensionRegulatorApiClient<PensionRegulatorApiConfiguration>> pensionRegulatorApiClientMock,
        GetRelationshipByUkprnPayeAornQueryHandler sut,
        long ukprn,
        string paye,
        string aorn,
        PensionRegulatorOrganisation tprOrganisation,
        GetRequestByUkprnAndPayeResponse expected,
        CancellationToken cancellationToken
    )
    {
        tprOrganisation.Status = "some invalid status";
        var encodedPaye = Uri.EscapeDataString(paye);

        SetupPrMockForGetRequestByUkprnAndPaye(providerRelationshipsApiRestClientMock, ukprn, expected, encodedPaye, cancellationToken);

        var pensionResponse = new ApiResponse<IEnumerable<PensionRegulatorOrganisation>>([tprOrganisation], HttpStatusCode.OK, string.Empty);

        pensionRegulatorApiClientMock
            .Setup(p => p.GetWithResponseCode<IEnumerable<PensionRegulatorOrganisation>>(It.IsAny<GetPensionsRegulatorOrganisationsRequest>()))
            .ReturnsAsync(pensionResponse);

        var request = new GetRelationshipsByUkprnPayeAornQuery(ukprn, aorn, paye);

        var actual = await sut.Handle(request, cancellationToken);

        var expectedResponse = new GetRelationshipsByUkprnPayeAornResult { HasActiveRequest = false, HasInvalidPaye = true };

        actual.Should().BeEquivalentTo(expectedResponse);
        providerRelationshipsApiRestClientMock.Verify(r => r.GetRequestByUkprnAndPaye(ukprn, encodedPaye, cancellationToken), Times.Once);
        pensionRegulatorApiClientMock.Verify(r => r.GetWithResponseCode<IEnumerable<PensionRegulatorOrganisation>>(It.IsAny<GetPensionsRegulatorOrganisationsRequest>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_GetsValidResultsFromPensionApi_ReturnExpectedOrganisation(
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClientMock,
        [Frozen] Mock<IPensionRegulatorApiClient<PensionRegulatorApiConfiguration>> pensionRegulatorApiClientMock,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClientMock,
        GetRelationshipByUkprnPayeAornQueryHandler sut,
        long ukprn,
        string paye,
        string aorn,
        PensionRegulatorOrganisation pensionOrganisationResponse,
        GetRequestByUkprnAndPayeResponse expected,
        CancellationToken cancellationToken
 )
    {
        var encodedPaye = Uri.EscapeDataString(paye);

        SetupPrMockForGetRequestByUkprnAndPaye(providerRelationshipsApiRestClientMock, ukprn, expected, encodedPaye, cancellationToken);

        SetupPensionResponses(pensionRegulatorApiClientMock, pensionOrganisationResponse);

        var accountHistoryResponse = new ApiResponse<AccountHistory>(new AccountHistory(), HttpStatusCode.NotFound, "");

        accountsApiClientMock.Setup(
                p => p.GetWithResponseCode<AccountHistory>(

                    It.IsAny<GetPayeSchemeAccountByRefRequest>()
                ))
            .ReturnsAsync(
                accountHistoryResponse
            );

        var request = new GetRelationshipsByUkprnPayeAornQuery(ukprn, aorn, paye);

        var actual = await sut.Handle(request, cancellationToken);

        var expectedOrganisation = BuildOrganisationDetails(pensionOrganisationResponse);

        var expectedResponse = new GetRelationshipsByUkprnPayeAornResult { HasActiveRequest = false, HasInvalidPaye = false, Organisation = expectedOrganisation };

        actual.Should().BeEquivalentTo(expectedResponse);
        providerRelationshipsApiRestClientMock.Verify(r => r.GetRequestByUkprnAndPaye(ukprn, encodedPaye, cancellationToken), Times.Once);
        pensionRegulatorApiClientMock.Verify(
            r => r.GetWithResponseCode<IEnumerable<PensionRegulatorOrganisation>>(
                It.IsAny<GetPensionsRegulatorOrganisationsRequest>()), Times.Once);
        accountsApiClientMock.Verify(
            r => r.GetWithResponseCode<AccountHistory>(
                It.IsAny<GetPayeSchemeAccountByRefRequest>()), Times.Once);

    }

    [Test, MoqAutoData]
    public async Task Handle_GetsValidResultsFromAccountsApi_ReturnExpectedAccountDetailsAndHasOneLegalEntityFalse(
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClientMock,
        [Frozen] Mock<IPensionRegulatorApiClient<PensionRegulatorApiConfiguration>> pensionRegulatorApiClientMock,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClientMock,
        GetRelationshipByUkprnPayeAornQueryHandler sut,
        long ukprn,
        string paye,
        string aorn,
        PensionRegulatorOrganisation pensionOrganisationResponse,
        GetRequestByUkprnAndPayeResponse expected,
        AccountHistory accountHistory,
        CancellationToken cancellationToken)
    {

        var encodedPaye = Uri.EscapeDataString(paye);

        SetupPrMockForGetRequestByUkprnAndPaye(providerRelationshipsApiRestClientMock, ukprn, expected, encodedPaye, cancellationToken);

        SetupPensionResponses(pensionRegulatorApiClientMock, pensionOrganisationResponse);

        var accountHistoryResponse = new ApiResponse<AccountHistory>(accountHistory, HttpStatusCode.OK, "");

        accountsApiClientMock.Setup(
                p => p.GetWithResponseCode<AccountHistory>(

                    It.IsAny<GetPayeSchemeAccountByRefRequest>()
                ))
        .ReturnsAsync(
        accountHistoryResponse
        );

        accountsApiClientMock
            .Setup(x => x.GetAll<GetAccountLegalEntityResponse>(
                It.IsAny<GetAccountLegalEntitiesRequest>()))!
            .ReturnsAsync(new List<GetAccountLegalEntityResponse>
            {
                new(),
                new()
            });

        var request = new GetRelationshipsByUkprnPayeAornQuery(ukprn, aorn, paye);

        var actual = await sut.Handle(request, cancellationToken);

        var expectedOrganisation = BuildOrganisationDetails(pensionOrganisationResponse);
        var expectedAccount = new AccountDetails
        {
            AccountId = accountHistory.AccountId,
            AddedDate = accountHistory.AddedDate,
            RemovedDate = accountHistory.RemovedDate
        };

        var expectedResponse = new GetRelationshipsByUkprnPayeAornResult { HasActiveRequest = false, HasInvalidPaye = false, Organisation = expectedOrganisation, Account = expectedAccount, HasOneLegalEntity = false };

        actual.Should().BeEquivalentTo(expectedResponse);
        providerRelationshipsApiRestClientMock.Verify(r => r.GetRequestByUkprnAndPaye(ukprn, encodedPaye, cancellationToken), Times.Once);
        pensionRegulatorApiClientMock.Verify(
            r => r.GetWithResponseCode<IEnumerable<PensionRegulatorOrganisation>>(
                It.IsAny<GetPensionsRegulatorOrganisationsRequest>()), Times.Once);
        accountsApiClientMock.Verify(
            r => r.GetWithResponseCode<AccountHistory>(
                It.IsAny<GetPayeSchemeAccountByRefRequest>()), Times.Once);
        accountsApiClientMock.Verify(r => r.GetAll<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntitiesRequest>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_GetsValidResultsFromAccountsApi_ReturnExpectedSingleLegalEntityDetails(
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClientMock,
        [Frozen] Mock<IPensionRegulatorApiClient<PensionRegulatorApiConfiguration>> pensionRegulatorApiClientMock,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClientMock,
        GetRelationshipByUkprnPayeAornQueryHandler sut,
        long ukprn,
        string paye,
        string aorn,
        PensionRegulatorOrganisation pensionOrganisationResponse,
        GetRequestByUkprnAndPayeResponse expected,
        AccountHistory accountHistory,
        GetAccountLegalEntityResponse legalEntityResponse,
        CancellationToken cancellationToken)
    {
        var encodedPaye = Uri.EscapeDataString(paye);

        SetupPrMockForGetRequestByUkprnAndPaye(providerRelationshipsApiRestClientMock, ukprn, expected, encodedPaye, cancellationToken);

        SetupPensionResponses(pensionRegulatorApiClientMock, pensionOrganisationResponse);

        var accountHistoryResponse = new ApiResponse<AccountHistory>(accountHistory, HttpStatusCode.OK, "");

        accountsApiClientMock.Setup(
                p => p.GetWithResponseCode<AccountHistory>(

                    It.IsAny<GetPayeSchemeAccountByRefRequest>()
                ))
        .ReturnsAsync(
        accountHistoryResponse
        );

        accountsApiClientMock
            .Setup(x => x.GetAll<GetAccountLegalEntityResponse>(
                It.IsAny<GetAccountLegalEntitiesRequest>()))!
            .ReturnsAsync(new List<GetAccountLegalEntityResponse>
            {
                legalEntityResponse
            });

        providerRelationshipsApiRestClientMock.Setup(x =>
            x.GetRelationship(
                ukprn,
                legalEntityResponse.AccountLegalEntityId,
                cancellationToken
            )
        )!.ReturnsAsync(
            new Response<GetRelationshipResponse>(
                string.Empty,
                new(HttpStatusCode.NotFound),
                () => new GetRelationshipResponse()
            ));

        var request = new GetRelationshipsByUkprnPayeAornQuery(ukprn, aorn, paye);

        var actual = await sut.Handle(request, cancellationToken);

        var expectedOrganisation = BuildOrganisationDetails(pensionOrganisationResponse);
        var expectedAccount = new AccountDetails
        {
            AccountId = accountHistory.AccountId,
            AddedDate = accountHistory.AddedDate,
            RemovedDate = accountHistory.RemovedDate
        };

        var expectedResponse = new GetRelationshipsByUkprnPayeAornResult
        {
            HasActiveRequest = false,
            HasInvalidPaye = false,
            Organisation = expectedOrganisation,
            Account = expectedAccount,
            HasOneLegalEntity = true,
            AccountLegalEntityId = legalEntityResponse.AccountLegalEntityId,
            AccountLegalEntityName = legalEntityResponse.Name,
            HasRelationship = false
        };

        actual.Should().BeEquivalentTo(expectedResponse);

        providerRelationshipsApiRestClientMock.Verify(r => r.GetRelationship(ukprn, legalEntityResponse.AccountLegalEntityId, cancellationToken), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_GetsValidResultsFromProviderApi_ReturnExpectedOperations(
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClientMock,
        [Frozen] Mock<IPensionRegulatorApiClient<PensionRegulatorApiConfiguration>> pensionRegulatorApiClientMock,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClientMock,
        GetRelationshipByUkprnPayeAornQueryHandler sut,
        long ukprn,
        string paye,
        string aorn,
        PensionRegulatorOrganisation pensionOrganisationResponse,
        GetRequestByUkprnAndPayeResponse expected,
        AccountHistory accountHistory,
        GetAccountLegalEntityResponse legalEntityResponse,
        GetRelationshipResponse relationshipResponse,
        CancellationToken cancellationToken)
    {
        var encodedPaye = Uri.EscapeDataString(paye);

        SetupPrMockForGetRequestByUkprnAndPaye(providerRelationshipsApiRestClientMock, ukprn, expected, encodedPaye, cancellationToken);

        SetupPensionResponses(pensionRegulatorApiClientMock, pensionOrganisationResponse);

        var accountHistoryResponse = new ApiResponse<AccountHistory>(accountHistory, HttpStatusCode.OK, "");

        accountsApiClientMock.Setup(
                p => p.GetWithResponseCode<AccountHistory>(

                    It.IsAny<GetPayeSchemeAccountByRefRequest>()
                ))
        .ReturnsAsync(
        accountHistoryResponse
        );

        accountsApiClientMock
            .Setup(x => x.GetAll<GetAccountLegalEntityResponse>(
                It.IsAny<GetAccountLegalEntitiesRequest>()))!
            .ReturnsAsync(new List<GetAccountLegalEntityResponse>
            {
                legalEntityResponse
            });

        providerRelationshipsApiRestClientMock.Setup(x =>
            x.GetRelationship(
                ukprn,
                legalEntityResponse.AccountLegalEntityId,
                cancellationToken
            )
        )!.ReturnsAsync(
            new Response<GetRelationshipResponse>(
                string.Empty,
                new(HttpStatusCode.OK),
                () => relationshipResponse
            ));

        var request = new GetRelationshipsByUkprnPayeAornQuery(ukprn, aorn, paye);

        var actual = await sut.Handle(request, cancellationToken);

        var expectedOrganisation = BuildOrganisationDetails(pensionOrganisationResponse);
        var expectedAccount = new AccountDetails
        {
            AccountId = accountHistory.AccountId,
            AddedDate = accountHistory.AddedDate,
            RemovedDate = accountHistory.RemovedDate
        };

        var expectedResponse = new GetRelationshipsByUkprnPayeAornResult
        {
            HasActiveRequest = false,
            HasInvalidPaye = false,
            Organisation = expectedOrganisation,
            Account = expectedAccount,
            HasOneLegalEntity = true,
            AccountLegalEntityId = legalEntityResponse.AccountLegalEntityId,
            AccountLegalEntityName = legalEntityResponse.Name,
            HasRelationship = true,
            Operations = relationshipResponse.Operations.ToList()
        };

        actual.Should().BeEquivalentTo(expectedResponse);

        providerRelationshipsApiRestClientMock.Verify(r => r.GetRelationship(ukprn, legalEntityResponse.AccountLegalEntityId, cancellationToken), Times.Once);
    }

    private static OrganisationDetails BuildOrganisationDetails(PensionRegulatorOrganisation pensionOrganisationResponse)
    {
        return new OrganisationDetails
        {
            Name = pensionOrganisationResponse.Name,
            Address = new AddressDetails
            {
                Line1 = pensionOrganisationResponse.Address!.Line1,
                Line2 = pensionOrganisationResponse.Address!.Line2,
                Line3 = pensionOrganisationResponse.Address!.Line3,
                Line4 = pensionOrganisationResponse.Address!.Line4,
                Line5 = pensionOrganisationResponse.Address!.Line5,
                Postcode = pensionOrganisationResponse.Address!.Postcode
            }
        };
    }

    private static void SetupPensionResponses(Mock<IPensionRegulatorApiClient<PensionRegulatorApiConfiguration>> pensionRegulatorApiClientMock,
        PensionRegulatorOrganisation pensionOrganisationResponse)
    {
        pensionOrganisationResponse.Status = GetRelationshipByUkprnPayeAornQueryHandler.TprStatusNotClosed;

        var pensionResponse = new ApiResponse<IEnumerable<PensionRegulatorOrganisation>>(
            new List<PensionRegulatorOrganisation>
            {
                pensionOrganisationResponse
            }, HttpStatusCode.OK, "");

        pensionRegulatorApiClientMock.Setup(
                p => p.GetWithResponseCode<IEnumerable<PensionRegulatorOrganisation>>(
                    It.IsAny<GetPensionsRegulatorOrganisationsRequest>()
                ))
            .ReturnsAsync(
                pensionResponse
            );
    }

    private static void SetupPrMockForGetRequestByUkprnAndPaye(Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClientMock, long ukprn, GetRequestByUkprnAndPayeResponse expected,
        string encodedPaye, CancellationToken cancellationToken)
    {
        providerRelationshipsApiRestClientMock.Setup(x =>
            x.GetRequestByUkprnAndPaye(
                ukprn,
                encodedPaye,
                cancellationToken
            )
        )!.ReturnsAsync(
            new Response<GetRequestByUkprnAndPayeResponse>(
                string.Empty,
                new(HttpStatusCode.NotFound),
                () => expected
            ));
    }
}
