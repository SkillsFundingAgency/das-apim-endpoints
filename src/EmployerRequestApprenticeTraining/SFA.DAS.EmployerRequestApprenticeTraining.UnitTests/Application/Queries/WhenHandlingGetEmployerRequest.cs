using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerRequest;
using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.UnitTests.Application.Queries
{
    public class WhenHandlingGetEmployerRequest
    {
        [Test, MoqAutoData]
        public async Task Then_Get_EmployerRequest_From_The_Api(
           EmployerRequest employerRequest,
           GetEmployerRequestQuery query,
           [Frozen] Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> mockRequestApprenticeTrainingClient,
           GetEmployerRequestQueryHandler handler)
        {
            // Arrange
            mockRequestApprenticeTrainingClient
                .Setup(client => client.Get<EmployerRequest>(It.IsAny<GetEmployerRequestRequest>()))
                .ReturnsAsync(employerRequest);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            // Assert
            actual.EmployerRequest.Should().BeEquivalentTo(employerRequest, options => options.ExcludingMissingMembers());
        }
    }
}
