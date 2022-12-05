using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeCommitments.Apis.TrainingProviderApi;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using SFA.DAS.ApprenticeCommitments.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.UnitTests.Application.Services
{
    [TestFixture]
    public class TrainingProviderServiceTests
    {
        [Test, AutoData]
        public void WhenNoTrainingProviderIsFound(
            long trainingProviderId,
            [Frozen] Mock<IInternalApiClient<TrainingProviderConfiguration>> client)
        {
            ClientReturnsSearchWith(client, Array.Empty<TrainingProviderResponse>());
            var sut = new TrainingProviderService(client.Object);
            sut.Invoking((s) => s.GetTrainingProviderDetails(trainingProviderId))
                .Should().ThrowAsync<HttpRequestContentException>()
                .WithMessage($"Training Provider Id {trainingProviderId} not found");
        }

        [Test, AutoData]
        public void WhenMultipleTrainingProvidersAreFound(
            long trainingProviderId,
            TrainingProviderResponse[] results,
            [Frozen] Mock<IInternalApiClient<TrainingProviderConfiguration>> client)
        {
            ClientReturnsSearchWith(client, results);
            var sut = new TrainingProviderService(client.Object);
            sut.Invoking((s) => s.GetTrainingProviderDetails(trainingProviderId))
                .Should().ThrowAsync<HttpRequestContentException>()
                .WithMessage($"Training Provider Id {trainingProviderId} finds multiple matches");
        }

        [Test, AutoData]
        public async Task WhenASingleTrainingProvidersIsFound(
            long trainingProviderId,
            TrainingProviderResponse result,
            [Frozen] Mock<IInternalApiClient<TrainingProviderConfiguration>> client)
        {
            ClientReturnsSearchWith(client, new[] { result });

            var sut = new TrainingProviderService(client.Object);
            var response = await sut.GetTrainingProviderDetails(trainingProviderId);
            response.Should().Be(result);
        }

        [Test, AutoData]
        public void WhenAnErrorIsFound(
            long trainingProviderId,
            [Frozen] Mock<IInternalApiClient<TrainingProviderConfiguration>> client)
        {
            ClientReturnsSearchWith(client, null, HttpStatusCode.InternalServerError, "some internal error");
            var sut = new TrainingProviderService(client.Object);
            sut.Invoking((s) => s.GetTrainingProviderDetails(trainingProviderId))
                .Should().ThrowAsync<HttpRequestContentException>().WithMessage("some internal error");
        }

        public static void ClientReturnsSearchWith(
            Mock<IInternalApiClient<TrainingProviderConfiguration>> client,
            TrainingProviderResponse[] results,
            HttpStatusCode statusCode = HttpStatusCode.OK,
            string error = null)
        {
            client.Setup(x =>
                   x.GetWithResponseCode<SearchResponse>(It.IsAny<GetTrainingProviderDetailsRequest>()))
                .ReturnsAsync(new ApiResponse<SearchResponse>(
                    new SearchResponse { SearchResults = results }, statusCode, error));
        }
    }
}