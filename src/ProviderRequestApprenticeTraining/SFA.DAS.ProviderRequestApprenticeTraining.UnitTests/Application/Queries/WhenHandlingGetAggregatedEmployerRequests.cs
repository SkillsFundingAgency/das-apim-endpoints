using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests;
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
    public class WhenHandlingGetAggregatedEmployerRequests
    {
        [Test, MoqAutoData]
        public async Task Then_Get_AggregatedEmployerRequests_From_The_Api(
           GetAggregatedEmployerRequestsResult requests,
           [Frozen] Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> mockRequestApprenticeTrainingClient,
           GetAggregatedEmployerRequestsQueryHandler handler,
           long ukprn)
        {
            // Arrange
            mockRequestApprenticeTrainingClient.Setup(client => client.Get<List<GetAggregatedEmployerRequestsResponse>>(It.IsAny<GetAggregatedEmployerRequestsRequest>()))
                .ReturnsAsync(requests.AggregatedEmployerRequests.ToList());

            // Act
            var actual = await handler.Handle(new GetAggregatedEmployerRequestsQuery(ukprn), CancellationToken.None);
            
            // Assert
            actual.AggregatedEmployerRequests.Should().BeEquivalentTo(requests.AggregatedEmployerRequests);
        }
    }
}
