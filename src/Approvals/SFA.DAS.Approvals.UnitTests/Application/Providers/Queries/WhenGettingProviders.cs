using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.Providers.Queries;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.UnitTests.Application.Providers.Queries
{
    public class WhenGettingProviders
    {
        [Test, MoqAutoData]
        public async Task Then_The_CourseDeliveryApi_Is_Called_With_The_Request_And_Epaos_Returned(
            GetProvidersQuery query,
            GetProvidersListResponse apiResponse,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> apiClient,
            [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> roatpApiClient,
            FeatureToggles featureToggles
        )
        {
            featureToggles.RoatpStandardsEnabled = false;
            GetProvidersQueryHandler handler = new GetProvidersQueryHandler(apiClient.Object, roatpApiClient.Object, featureToggles);
            apiClient.Setup(x => x.Get<GetProvidersListResponse>(It.IsAny<GetProvidersRequest>())).ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Providers.Should().BeEquivalentTo(apiResponse.Providers);
        }

        [Test, MoqAutoData]
        public async Task Then_The_RoatpApi_Is_Called_With_The_Request_And_Epaos_Returned(
            GetProvidersQuery query,
            GetRoatpProvidersListResponse apiResponse,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> apiClient,
            [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> roatpApiClient,
            FeatureToggles featureToggles
        )
        {
            featureToggles.RoatpStandardsEnabled = true;
            GetProvidersQueryHandler handler = new GetProvidersQueryHandler(apiClient.Object, roatpApiClient.Object, featureToggles);
            roatpApiClient.Setup(x => x.Get<GetRoatpProvidersListResponse>(It.IsAny<GetProvidersRequest>())).ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Providers.Should().BeEquivalentTo(apiResponse.RegisteredProviders);
        }
    }
}