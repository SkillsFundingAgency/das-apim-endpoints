using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Queries.GetRoatpProviders;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GetProvidersResponse = SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2.GetProvidersResponse;
using Provider = SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2.Provider;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class GetRoatpProviderQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_MapsProvidersCorrectly(
             GetRoatpProvidersQuery query)
        {
            // Arrange
            var mockApi = new Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>>();

            var apiResponse = new ApiResponse<GetProvidersResponse>(
                new GetProvidersResponse
                {
                    RegisteredProviders = new List<Provider>
                    {
                        new Provider { Name = "Provider A", Ukprn = 11111111 },
                        new Provider { Name = "Provider B", Ukprn = 22222222 }
                    }
                },
                (System.Net.HttpStatusCode)200,
                null
            );

            mockApi
                .Setup(x => x.GetWithResponseCode<GetProvidersResponse>(
                    It.IsAny<GetRoatpProvidersRequest>()))
                .ReturnsAsync(apiResponse);

            var handler = new GetRoatpProvidersQueryHandler(mockApi.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Providers.Should().HaveCount(2);
            var providerList = result.Providers.ToList();

            providerList[0].Name.Should().Be("Provider A");
            providerList[0].Ukprn.Should().Be(11111111);

            providerList[1].Name.Should().Be("Provider B");
            providerList[1].Ukprn.Should().Be(22222222);

            mockApi.Verify(x =>
                x.GetWithResponseCode<GetProvidersResponse>(
                    It.Is<GetRoatpProvidersRequest>(r => (bool)r.Live)), Times.Once);
        }
    }
}
