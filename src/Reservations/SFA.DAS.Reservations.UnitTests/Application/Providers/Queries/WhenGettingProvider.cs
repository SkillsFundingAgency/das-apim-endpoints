using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Reservations.Application.Providers.Queries.GetProvider;
using SFA.DAS.Reservations.InnerApi.Requests;
using SFA.DAS.Reservations.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Reservations.UnitTests.Application.Providers.Queries
{
    public class WhenGettingProvider
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Provider_From_Roatp_Api(
            GetProviderQuery query,
            GetRoatpProviderResponse apiResponse,
            [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> roatpApiClient)
        {
            GetProviderQueryHandler handler = new GetProviderQueryHandler(roatpApiClient.Object);
            roatpApiClient
                .Setup(client => client.Get<GetRoatpProviderResponse>(It.Is<GetProviderRequest>(request => request.Ukprn == query.Ukprn)))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.IsNotNull(result);
            result.Should().BeEquivalentTo(new GetProviderResult { Provider = (GetProviderResponse)apiResponse });
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_Null_From_Roatp_Api(
            GetProviderQuery query,
            [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> roatpApiClient)
        {
            GetRoatpProviderResponse apiResponse = null;

            GetProviderQueryHandler handler = new GetProviderQueryHandler( roatpApiClient.Object);
            roatpApiClient
                .Setup(client =>
                    client.Get<GetRoatpProviderResponse>(
                        It.Is<GetProviderRequest>(request => request.Ukprn == query.Ukprn)))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.IsNull(result.Provider);
        }
    }
}