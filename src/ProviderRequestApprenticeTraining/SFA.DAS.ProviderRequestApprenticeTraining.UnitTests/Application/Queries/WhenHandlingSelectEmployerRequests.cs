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
           GetSelectEmployerRequestsResult requests,
           [Frozen] Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> mockRequestApprenticeTrainingClient,
           GetSelectEmployerRequestsQueryHandler handler,
           long ukprn,
           string standardReference)
        {
            // Arrange
            mockRequestApprenticeTrainingClient.Setup(client => client.Get<List<GetSelectEmployerRequestsResponse>>(It.IsAny<GetSelectEmployerRequestsRequest>()))
                .ReturnsAsync(requests.SelectEmployerRequests.ToList());

            // Act
            var actual = await handler.Handle(new GetSelectEmployerRequestsQuery { Ukprn = ukprn, StandardReference = standardReference}, CancellationToken.None);
            
            // Assert
            actual.SelectEmployerRequests.Should().BeEquivalentTo(requests.SelectEmployerRequests);
        }
    }
}
