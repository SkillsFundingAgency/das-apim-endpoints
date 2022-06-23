using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.Providers.Queries;
using SFA.DAS.Approvals.Application.ProviderUsers.Queries;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.UnitTests.Application.ProviderUsers.Queries
{
    public class WhenGettingProviderUsers
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_ProviderUsers_Returned(
            GetProviderUsersQuery query,
            IEnumerable<GetProviderUsersListItem> apiUsersResponse,
            [Frozen] Mock<IProviderAccountApiClient<ProviderAccountApiConfiguration>> apiClient,
            GetProviderUsersQueryHandler handler
        )
        {
            apiClient
                .Setup(x => x.GetAll<GetProviderUsersListItem>(It.IsAny<GetProviderUsersRequest>()))
                .ReturnsAsync(apiUsersResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Users.Should().BeEquivalentTo(apiUsersResponse);
        }
    }
}