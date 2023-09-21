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
        public async Task Then_The_Query_Is_Handled_And_Data_Returned(
            GetEmployerAccountTaskListQuery query,
            GetProviderAccountLegalEntitiesResponse providerRelationshipResponse,
            [Frozen]
            Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipApiClient,
            GetEmployerAccountTaskListQueryHandler handler)
        {
            providerRelationshipApiClient
                .Setup(x =>
                    x.Get<GetProviderAccountLegalEntitiesResponse>(It.Is<GetEmployerAccountProviderPermissionsRequest>(
                        c =>
                            c.GetUrl.Equals(
                                "accountproviderlegalentities?accountHashedId={_hashedAccountId}&operations=1&operations=2"))))
                .ReturnsAsync(providerRelationshipResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.EmployerAccountLegalEntityPermissions.Should().BeEquivalentTo(providerRelationshipResponse);
        }
    }
}