using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.MigrateData;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.Apply
{
    public class WhenHandlingMigrateDataCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_CommandResult_Is_Returned_As_Expected(
            MigrateDataCommand command,
            PutCandidateApiResponse putCandidateApiResponse,
            GetLegacyUserByEmailApiResponse legacyUserByEmailApiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockCandidateApiClient,
            [Frozen] Mock<IFindApprenticeshipLegacyApiClient<FindApprenticeshipLegacyApiConfiguration>> mockLegacyApiClient,
            [Frozen] Mock<ILegacyApplicationMigrationService> vacancyMigrationService,
            MigrateDataCommandHandler handler)
        {
            var expectedPutData = new PutCandidateApiRequestData
            {
                Email = command.EmailAddress,
            };

            var expectedRequest = new PutCandidateApiRequest(command.CandidateId, expectedPutData);

            mockCandidateApiClient
                .Setup(client => client.PutWithResponseCode<PutCandidateApiResponse>(
                    It.Is<PutCandidateApiRequest>(c =>
                        c.PutUrl == expectedRequest.PutUrl
                        && ((PutCandidateApiRequestData)c.Data).Email == command.EmailAddress)))
                .ReturnsAsync(new ApiResponse<PutCandidateApiResponse>(putCandidateApiResponse, HttpStatusCode.OK, string.Empty));

            var legacyGetRequest = new GetLegacyUserByEmailApiRequest(command.EmailAddress);
            mockLegacyApiClient
                .Setup(client => client.Get<GetLegacyUserByEmailApiResponse>(
                    It.Is<GetLegacyUserByEmailApiRequest>(r => r.GetUrl == legacyGetRequest.GetUrl)))
                .ReturnsAsync(legacyUserByEmailApiResponse);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().Be(Unit.Value);
            vacancyMigrationService
                .Verify(client => client.MigrateLegacyApplications(command.CandidateId, command.EmailAddress), Times.Once);
        }
    }
}
