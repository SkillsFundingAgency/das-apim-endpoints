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
        public async Task Then_Gets_Provider_From_Courses_Api(
            GetProviderQuery query,
            GetProviderResponse apiResponse,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockApiClient,
            GetProviderQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetProviderResponse>(It.Is<GetProviderRequest>(request => request.Ukprn == query.Ukprn)))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Provider.Should().BeEquivalentTo(apiResponse);
        }
    }
}