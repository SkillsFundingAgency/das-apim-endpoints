using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.UnitTests.Application.Queries
{
    public class WhenHandlingGetSelectEmployerRequests
    {
        [Test, MoqAutoData]
        public async Task Then_Get_AggregatedEmployerRequests_From_The_Api(
           GetSelectEmployerRequestsResult result,
           [Frozen] Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> mockRequestApprenticeTrainingClient,
           GetSelectEmployerRequestsQueryHandler handler,
           GetSelectEmployerRequestsQuery query)
        {
            // Arrange
            mockRequestApprenticeTrainingClient.Setup(client => client.Get<List<GetSelectEmployerRequestsResponse>>(It.IsAny<GetSelectEmployerRequestsRequest>()))
                .ReturnsAsync(result.SelectEmployerRequests.ToList());

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            // Assert
            actual.SelectEmployerRequests.Should().BeEquivalentTo(result.SelectEmployerRequests);
        }

        [Test, MoqAutoData]
        public async Task AndNoneExist_Then_Get_AggregatedEmployerRequests_ReturnsEmptySelectEmployerRequests_WithUkprn(
           [Frozen] Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> mockRequestApprenticeTrainingClient,
           GetSelectEmployerRequestsQueryHandler handler,
           GetSelectEmployerRequestsQuery query)
        {
            //Arrange
            mockRequestApprenticeTrainingClient.Setup(client => client.Get<List<GetSelectEmployerRequestsResponse>>(It.IsAny<GetSelectEmployerRequestsRequest>()))
                .ReturnsAsync(new List<GetSelectEmployerRequestsResponse>());

           //Act
           var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.SelectEmployerRequests.Should().BeEmpty();
        }
    }
}
