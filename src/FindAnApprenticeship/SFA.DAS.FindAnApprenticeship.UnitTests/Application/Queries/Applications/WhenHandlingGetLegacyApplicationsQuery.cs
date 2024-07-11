using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Users.MigrateData;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Applications
{
    [TestFixture]
    public class WhenHandlingGetLegacyApplicationsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_QueryResult_Is_Returned_As_Expected(
            MigrateDataQuery query,
            GetLegacyApplicationsByEmailApiResponse applicationsApiResponse,
            GetLegacyUserByEmailApiResponse apiResponse,
            GetCandidateApiResponse candidateApiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApi,
            [Frozen] Mock<IFindApprenticeshipLegacyApiClient<FindApprenticeshipLegacyApiConfiguration>> legacyApi,
            [Frozen] Mock<ILegacyApplicationMigrationService> vacancyMigrationService,
            MigrateDataQueryHandler handler)
        {  
            vacancyMigrationService.Setup(client => client.GetLegacyApplications(query.EmailAddress))
                .ReturnsAsync(applicationsApiResponse);
            var expectedRequest = new GetLegacyUserByEmailApiRequest(query.EmailAddress);
            legacyApi.Setup(x =>
                    x.Get<GetLegacyUserByEmailApiResponse>(
                        It.Is<GetLegacyUserByEmailApiRequest>(c => c.GetUrl == expectedRequest.GetUrl)))
                .ReturnsAsync(apiResponse);
            var expectedCandidate = new GetCandidateApiRequest(query.CandidateId.ToString());
            candidateApi
                .Setup(x => x.GetWithResponseCode<GetCandidateApiResponse>(
                    It.Is<GetCandidateApiRequest>(c => c.GetUrl == expectedCandidate.GetUrl)))
                .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(candidateApiResponse, HttpStatusCode.OK, ""));

            var result = await handler.Handle(query, CancellationToken.None);

            using var scope = new AssertionScope();
            result.Applications.Should().BeEquivalentTo(applicationsApiResponse.Applications);
            result.LegacyUserDetail.Should().BeEquivalentTo(apiResponse.RegistrationDetails);
            result.CandidateDetail.Should().BeEquivalentTo(candidateApiResponse);
        }
    }
}
