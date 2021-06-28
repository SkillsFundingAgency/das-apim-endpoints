using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using SFA.DAS.ApprenticeCommitments.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.UnitTests.Application.Services
{
    [TestFixture]
    public class CommitmentsV2ServiceTests
    {
        [Test, AutoData]
        public void WhenApprenticeshipIsNotFound(
            long accountId,
            long apprenticeshipId,
            [Frozen] Mock<IInternalApiClient<CommitmentsV2Configuration>> client)
        {
            client
                .Setup(x =>
                   x.GetWithResponseCode<Apis.CommitmentsV2InnerApi.ApprenticeshipResponse>(
                       It.IsAny<Apis.CommitmentsV2InnerApi.GetApprenticeshipDetailsRequest>()))
                .ReturnsAsync(new ApiResponse<Apis.CommitmentsV2InnerApi.ApprenticeshipResponse>(
                    null, HttpStatusCode.NotFound, ""));
            var sut = new CommitmentsV2Service(client.Object);
            sut.Invoking((s) => s.GetApprenticeshipDetails(accountId, apprenticeshipId)).Should().Throw<HttpRequestContentException>();
        }

        [Test, AutoData]
        public void WhenApprenticeshipIsFoundButWithWrongAccountId(
            long accountId,
            long apprenticeshipId,
            [Frozen] Mock<IInternalApiClient<CommitmentsV2Configuration>> client)
        {
            ClientReturnsApprenticeshipWith(client, accountId - 1, apprenticeshipId);

            var sut = new CommitmentsV2Service(client.Object);
            sut.Invoking((s) => s.GetApprenticeshipDetails(accountId, apprenticeshipId)).Should().Throw<HttpRequestContentException>();
        }

        [Test, AutoData]
        public async Task WhenApprenticeshipIsFoundAndTheAccountIdMatches(
            long accountId,
            long apprenticeshipId,
            [Frozen] Mock<IInternalApiClient<CommitmentsV2Configuration>> client)
        {
            ClientReturnsApprenticeshipWith(client, accountId, apprenticeshipId);

            var sut = new CommitmentsV2Service(client.Object);
            var response = await sut.GetApprenticeshipDetails(accountId, apprenticeshipId);
            response.EmployerAccountId.Should().Be(accountId);
            response.Id.Should().Be(apprenticeshipId);
        }

        private void ClientReturnsApprenticeshipWith(Mock<IInternalApiClient<CommitmentsV2Configuration>> client, long accountId, long apprenticeshipId)
        {
            client
                .Setup(x =>
                   x.GetWithResponseCode<Apis.CommitmentsV2InnerApi.ApprenticeshipResponse>(
                       It.IsAny<Apis.CommitmentsV2InnerApi.GetApprenticeshipDetailsRequest>()))

                .ReturnsAsync(new ApiResponse<Apis.CommitmentsV2InnerApi.ApprenticeshipResponse>(
                    new Apis.CommitmentsV2InnerApi.ApprenticeshipResponse
                    {
                        EmployerAccountId = accountId,
                        Id = apprenticeshipId,
                    },
                    HttpStatusCode.OK,
                    null));
        }
    }
}