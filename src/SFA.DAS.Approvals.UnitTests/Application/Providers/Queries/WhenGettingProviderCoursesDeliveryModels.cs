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
    public class WhenGettingProviderCoursesDeliveryModels
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_DeliveryModels_Returned(
            GetProviderCoursesDeliveryModelQuery query,
            GetProviderCourseDeliveryModelsResponse apiResponse,
            [Frozen] Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>> apiClient,
            GetProviderCoursesDeliveryModelsQueryHandler handler
        )
        {
            apiClient.Setup(x => x.Get<GetProviderCourseDeliveryModelsResponse>(It.IsAny<GetProviderCoursesDeliveryModelsRequest>())).ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.DeliveryModels.Should().BeEquivalentTo(apiResponse.DeliveryModels);
        }


        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_When_No_Response_Returned_We_Create_Default(
            GetProviderCoursesDeliveryModelQuery query,
            [Frozen] Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>> apiClient,
            GetProviderCoursesDeliveryModelsQueryHandler handler
        )
        {
            apiClient.Setup(x => x.Get<GetProviderCourseDeliveryModelsResponse>(It.IsAny<GetProviderCoursesDeliveryModelsRequest>())).ReturnsAsync((GetProviderCourseDeliveryModelsResponse)null);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().BeEquivalentTo(new { DeliveryModels = new [] {"Regular"} });
        }
    }

}