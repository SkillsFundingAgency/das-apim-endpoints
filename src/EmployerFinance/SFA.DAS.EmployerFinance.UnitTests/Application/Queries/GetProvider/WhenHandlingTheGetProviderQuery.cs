using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Queries.GetProvider;
using SFA.DAS.EmployerFinance.InnerApi.Requests;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Queries.GetProvider
{
    public class WhenHandlingTheGetProviderQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Providers_Returned(
            GetProviderQuery query,
            GetProvidersListItem apiResponse,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> apiClient,
            GetProviderQueryHandler handler
        )
        {
            apiClient.Setup(x =>
                    x.Get<GetProvidersListItem>(
                        It.Is<GetProviderRequest>(c => c.GetUrl.Equals($"api/providers/{query.Id}"))))
                .ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Provider.Should().BeEquivalentTo(apiResponse);
        }
    }
}