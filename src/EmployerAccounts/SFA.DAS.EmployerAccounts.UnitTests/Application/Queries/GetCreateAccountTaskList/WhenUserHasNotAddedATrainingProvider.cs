﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Application.Queries.GetCreateAccountTaskList;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAgreements;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderPermissions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAgreements;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Queries.GetCreateAccountTaskList;

public class WhenUserHasNotAddedATrainingProvider
{
    [Test, MoqAutoData]
    public async Task Then_AgreementAcknowledge_ShouldReturnTrue(
        [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        GetAccountProvidersResponse accountProvidersResponse,
        GetCreateAccountTaskListQueryHandler sut,
        GetCreateAccountTaskListQuery query,
        GetAccountByIdResponse accountResponse,
        [NoAutoProperties] List<GetEmployerAgreementsResponse> employerAgreementsResponse,
        long agreementId
        )
    {
        accountResponse.NameConfirmed = true;
        
        employerAgreementsResponse.Add(new GetEmployerAgreementsResponse
        {
            Id = agreementId,
            Acknowledged = true,
            SignedDate = DateTime.Today
        });
        
        providerRelationshipsApiClient
            .Setup(x =>
                x.Get<GetAccountProvidersResponse>(It.Is<GetAccountProvidersRequest>(c =>
                    c.GetUrl.Equals($"accounts/{query.AccountId}/providers"))))
            .ReturnsAsync(accountProvidersResponse);

        providerRelationshipsApiClient
            .Setup(x =>
                x.Get<GetProviderAccountLegalEntitiesResponse>(It.Is<GetEmployerAccountProviderPermissionsRequest>(c =>
                    c.GetUrl.Equals($"accountproviderlegalentities?accountHashedId={query.HashedAccountId}&operations=1&operations=2"))))
            .ReturnsAsync(() => null);
        
        accountsApiClient
            .Setup(x =>
                x.Get<GetAccountByIdResponse>(It.Is<GetAccountByIdRequest>(c =>
                    c.GetUrl.Equals($"api/accounts/{query.AccountId}"))))
            .ReturnsAsync(accountResponse);
        
        accountsApiClient
            .Setup(x =>
                x.GetAll<GetEmployerAgreementsResponse>(It.Is<GetEmployerAgreementsRequest>(c =>
                    c.GetAllUrl.Equals($"api/accounts/{query.AccountId}/agreements"))))
            .ReturnsAsync(employerAgreementsResponse);

        var result = await sut.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.AgreementAcknowledged.Should().BeTrue();
        result.CompletedSections.Should().Be(4);
    }
    
    [Test, MoqAutoData]
    public async Task Then_AgreementSigned_ShouldReturnTrue(
        [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        GetAccountProvidersResponse accountProvidersResponse,
        GetCreateAccountTaskListQueryHandler sut,
        GetCreateAccountTaskListQuery query,
        GetAccountByIdResponse accountResponse,
        [NoAutoProperties] List<GetEmployerAgreementsResponse> employerAgreementsResponse,
        long agreementId
        )
    {
        accountResponse.NameConfirmed = true;
        
        employerAgreementsResponse.Add(new GetEmployerAgreementsResponse
        {
            Id = agreementId,
            Acknowledged = true,
            SignedDate = DateTime.Today
        });
        
        providerRelationshipsApiClient
            .Setup(x =>
                x.Get<GetAccountProvidersResponse>(It.Is<GetAccountProvidersRequest>(c =>
                    c.GetUrl.Equals($"accounts/{query.AccountId}/providers"))))
            .ReturnsAsync(accountProvidersResponse);

        providerRelationshipsApiClient
            .Setup(x =>
                x.Get<GetProviderAccountLegalEntitiesResponse>(It.Is<GetEmployerAccountProviderPermissionsRequest>(c =>
                    c.GetUrl.Equals($"accountproviderlegalentities?accountHashedId={query.HashedAccountId}&operations=1&operations=2"))))
            .ReturnsAsync(() => null);
        
        accountsApiClient
            .Setup(x =>
                x.Get<GetAccountByIdResponse>(It.Is<GetAccountByIdRequest>(c =>
                    c.GetUrl.Equals($"api/accounts/{query.AccountId}"))))
            .ReturnsAsync(accountResponse);
        
        accountsApiClient
            .Setup(x =>
                x.GetAll<GetEmployerAgreementsResponse>(It.Is<GetEmployerAgreementsRequest>(c =>
                    c.GetAllUrl.Equals($"api/accounts/{query.AccountId}/agreements"))))
            .ReturnsAsync(employerAgreementsResponse);

        var result = await sut.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.AgreementAcknowledged.Should().BeTrue();
        result.HasSignedAgreement.Should().BeTrue();
        result.CompletedSections.Should().Be(4);
    }
}