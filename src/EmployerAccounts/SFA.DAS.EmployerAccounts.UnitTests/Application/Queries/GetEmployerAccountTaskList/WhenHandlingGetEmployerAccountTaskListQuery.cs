using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Application.Queries.GetEmployerAccountTaskList;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Queries.AccountUsers
{
    public class WhenHandlingGetEmployerAccountTaskListQuery
    {
        [Test, MoqAutoData]
        public async Task When_HasProviders_And_Permissions_ThenReturned(
            GetEmployerAccountTaskListQuery query,
            GetProviderAccountLegalEntitiesResponse providerRelationshipResponse,
            GetAccountProvidersResponse accountProvidersResponse,
            [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipApiClient,
            GetEmployerAccountTaskListQueryHandler handler)
        {
            providerRelationshipApiClient
                .Setup(x =>
                    x.Get<GetAccountProvidersResponse>(It.Is<GetAccountProvidersRequest>(
                        c =>
                            c.GetUrl.Equals(
                                $"accounts/{query.AccountId}/providers"))))
                .ReturnsAsync(accountProvidersResponse);
            
            providerRelationshipApiClient
                .Setup(x =>
                    x.Get<GetProviderAccountLegalEntitiesResponse>(It.Is<GetEmployerAccountProviderPermissionsRequest>(
                        c =>
                            c.GetUrl.Equals(
                                $"accountproviderlegalentities?accountHashedId={query.HashedAccountId}&operations=1&operations=2"))))
                .ReturnsAsync(providerRelationshipResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.HasPermissions.Should().BeTrue();
        }
        
        [Test, MoqAutoData]
        public async Task When_HasProviders_And_NoPermissions_ThenReturned(
            GetEmployerAccountTaskListQuery query,
            GetAccountProvidersResponse accountProvidersResponse,
            GetProviderAccountLegalEntitiesResponse providerRelationshipResponse,
            [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipApiClient,
            GetEmployerAccountTaskListQueryHandler handler)
        {
            providerRelationshipResponse.AccountProviderLegalEntities = Array.Empty<GetProviderAccountLegalEntityItem>().ToList();
            
            providerRelationshipApiClient
                .Setup(x =>
                    x.Get<GetAccountProvidersResponse>(It.Is<GetAccountProvidersRequest>(
                        c =>
                            c.GetUrl.Equals(
                                $"accounts/{query.AccountId}/providers"))))
                .ReturnsAsync(accountProvidersResponse);
            
            providerRelationshipApiClient
                .Setup(x =>
                    x.Get<GetProviderAccountLegalEntitiesResponse>(It.Is<GetEmployerAccountProviderPermissionsRequest>(
                        c =>
                            c.GetUrl.Equals(
                                $"accountproviderlegalentities?accountHashedId={query.HashedAccountId}&operations=1&operations=2"))))
                .ReturnsAsync(providerRelationshipResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.HasProviders.Should().BeTrue();
            actual.HasPermissions.Should().BeFalse();
        }
        
        [Test, MoqAutoData]
        public async Task When_HasNoProviders_And_NoPermissions_ThenReturned(
            GetEmployerAccountTaskListQuery query,
            GetAccountProvidersResponse accountProvidersResponse,
            GetProviderAccountLegalEntitiesResponse providerRelationshipResponse,
            [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipApiClient,
            GetEmployerAccountTaskListQueryHandler handler)
        {
            providerRelationshipResponse.AccountProviderLegalEntities = Array.Empty<GetProviderAccountLegalEntityItem>().ToList();
            accountProvidersResponse.AccountProviders = Array.Empty<AccountProviderResponse>().ToList();
            
            providerRelationshipApiClient
                .Setup(x =>
                    x.Get<GetAccountProvidersResponse>(It.Is<GetAccountProvidersRequest>(
                        c =>
                            c.GetUrl.Equals(
                                $"accounts/{query.AccountId}/providers"))))
                .ReturnsAsync(accountProvidersResponse);
            
            providerRelationshipApiClient
                .Setup(x =>
                    x.Get<GetProviderAccountLegalEntitiesResponse>(It.Is<GetEmployerAccountProviderPermissionsRequest>(
                        c =>
                            c.GetUrl.Equals(
                                $"accountproviderlegalentities?accountHashedId={query.HashedAccountId}&operations=1&operations=2"))))
                .ReturnsAsync(providerRelationshipResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.HasProviders.Should().BeFalse();
            actual.HasPermissions.Should().BeFalse();
        }
    }
}