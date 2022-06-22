using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.Providers.Queries;
using SFA.DAS.Approvals.Application.DeliveryModels.Queries;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.UnitTests.Application.Providers.Queries
{
    public class WhenGettingProviderCoursesDeliveryModels
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_DeliveryModels_Returned(
            GetDeliveryModelsQuery query,
            GetDeliveryModelsResponse apiResponse,
            [Frozen] Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>> apiClient,
            GetDeliveryModelsQueryHandler handler
        )
        {
            apiClient
                .Setup(x => x.Get<GetDeliveryModelsResponse>(It.IsAny<GetDeliveryModelsRequest>()))
                .ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.DeliveryModels.Should().BeEquivalentTo(apiResponse.DeliveryModels);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_When_No_Response_Returned_We_Create_Default(
            GetDeliveryModelsQuery query,
            [Frozen] Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>> apiClient,
            GetDeliveryModelsQueryHandler handler
        )
        {
            apiClient
                .Setup(x => x.Get<GetDeliveryModelsResponse>(It.IsAny<GetDeliveryModelsRequest>()))
                .ReturnsAsync((GetDeliveryModelsResponse)null);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().BeEquivalentTo(new { DeliveryModels = new[] { "Regular" } });
        }

        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_When_Null_Response_Returned_We_Create_Default(
            GetDeliveryModelsQuery query,
            [Frozen] Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>> apiClient,
            GetDeliveryModelsQueryHandler handler
        )
        {
            apiClient
                .Setup(x => x.Get<GetDeliveryModelsResponse>(It.IsAny<GetDeliveryModelsRequest>()))
                .ReturnsAsync(new GetDeliveryModelsResponse { DeliveryModels = null });

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().BeEquivalentTo(new { DeliveryModels = new[] { "Regular" } });
        }

        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_When_Empty_Response_Returned_We_Create_Default(
            GetDeliveryModelsQuery query,
            [Frozen] Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>> apiClient,
            GetDeliveryModelsQueryHandler handler
        )
        {
            apiClient
                .Setup(x => x.Get<GetDeliveryModelsResponse>(It.IsAny<GetDeliveryModelsRequest>()))
                .ReturnsAsync(new GetDeliveryModelsResponse { DeliveryModels = new System.Collections.Generic.List<string>() });

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().BeEquivalentTo(new { DeliveryModels = new[] { "Regular" } });
        }
    }
}