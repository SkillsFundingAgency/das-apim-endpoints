using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderResponseConfirmation;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.UnitTests.Application.Queries
{
    public class WhenHandlingGetProviderResponseConfirmation
    {
        [Test, MoqAutoData]
        public async Task Then_Get_ProviderResponseConfirmation_From_The_Api(
            [Frozen] Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> apiClient,
            GetProviderResponseConfirmationResponse responseConfirmation,
            GetProviderResponseConfirmationQueryHandler handler,
            GetProviderResponseConfirmationQuery query
       )
        {
            // Arrange
            apiClient.Setup(client => client.Get<GetProviderResponseConfirmationResponse>(It.IsAny<GetProviderResponseConfirmationRequest>()))
                    .ReturnsAsync(responseConfirmation);
            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Email.Should().Be(responseConfirmation.Email);
            actual.Phone.Should().Be(responseConfirmation.Phone);
            actual.Website.Should().Be(responseConfirmation.Website);
            actual.Ukprn.Should().Be(responseConfirmation.Ukprn);
            actual.EmployerRequests.Should().BeEquivalentTo(responseConfirmation.EmployerRequests);

        }
    }
}
