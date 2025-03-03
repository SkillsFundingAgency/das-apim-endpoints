using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds;
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
    public class WhenHandlingGetEmployerRequestsByids
    {
        [Test, MoqAutoData]
        public async Task Then_Get_EmployerRequestsByIds_From_The_Api(
           GetEmployerRequestsByIdsResult result,
           [Frozen] Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> mockRequestApprenticeTrainingClient,
           GetEmployerRequestsByIdsQueryHandler handler,
           GetEmployerRequestsByIdsQuery query)
        {
            // Arrange
            mockRequestApprenticeTrainingClient.Setup(client => client.Get<List<GetEmployerRequestsByIdsResponse>>(It.IsAny<GetEmployerRequestsByIdsRequest>()))
                .ReturnsAsync(result.EmployerRequests.ToList());

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            // Assert
            actual.EmployerRequests.Should().BeEquivalentTo(result.EmployerRequests);
        }

        [Test, MoqAutoData]
        public async Task AndNoneExist_Then_Get_EmployerRequestsByIds_ReturnsEmptyEmployerRequests(
           [Frozen] Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> mockRequestApprenticeTrainingClient,
           GetEmployerRequestsByIdsQueryHandler handler,
           GetEmployerRequestsByIdsQuery query)
        {
            //Arrange
            mockRequestApprenticeTrainingClient.Setup(client => client.Get<List<GetEmployerRequestsByIdsResponse>>(It.IsAny<GetEmployerRequestsByIdsRequest>()))
                .ReturnsAsync(new List<GetEmployerRequestsByIdsResponse>());

           //Act
           var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.EmployerRequests.Should().BeEmpty();
        }
    }
}
