using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ManageApprenticeships.Application.Queries.GetProviders;
using SFA.DAS.ManageApprenticeships.InnerApi.Requests;
using SFA.DAS.ManageApprenticeships.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ManageApprenticeships.UnitTests.Application.Queries.GetProviders
{
    public class WhenHandlingTheGetProvidersQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Providers_Returned(
            GetProvidersQuery query,
            GetProvidersListResponse apiResponse,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> apiClient,
            GetProvidersQueryHandler handler
        )
        {
            apiClient.Setup(x => x.Get<GetProvidersListResponse>(It.IsAny<GetProvidersRequest>())).ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Providers.Should().BeEquivalentTo(apiResponse.Providers);
        }
    }
}