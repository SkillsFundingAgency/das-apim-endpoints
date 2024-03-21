using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerRequest;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerRequests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.UnitTests.Application.Queries
{
    public class WhenHandlingGetEmployerRequests
    {
        [Test, MoqAutoData]
        public async Task Then_Get_EmployerRequests_From_The_Api(
           GetEmployerRequestsResult employerRequestsResult,
           GetEmployerRequestQuery query,
           [Frozen] Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> mockRequestApprenticeTrainingClient,
           GetEmployerRequestQueryHandler handler)
        {
            // Arrange
            mockRequestApprenticeTrainingClient
                .Setup(client => client.Get<List<Models.EmployerRequest>>(It.IsAny<GetEmployerRequestsRequest>()))
                .ReturnsAsync(employerRequestsResult.EmployerRequests);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            // Assert
            actual.EmployerRequest.Should().BeEquivalentTo(employerRequestsResult, options => options.ExcludingMissingMembers());
        }
    }
}
