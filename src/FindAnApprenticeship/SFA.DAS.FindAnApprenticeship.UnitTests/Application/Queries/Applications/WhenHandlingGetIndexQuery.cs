using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplications;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Applications
{
    [TestFixture]
    public class WhenHandlingGetIndexQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_QueryResult_Is_Returned_As_Expected(
            GetApplicationsQuery query,
            GetApplicationsApiResponse applicationApiResponse,
            PostGetVacanciesByReferenceApiResponse vacanciesResponse,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> faaApiClient,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetApplicationsQueryHandler handler)
        {
            for (var i = 0; i < applicationApiResponse.Applications.Count; i++)
            {
                vacanciesResponse.ApprenticeshipVacancies.ToList()[i].VacancyReference =
                    $"VAC{applicationApiResponse.Applications[i].VacancyReference}";
            }

            var expectedGetApplicationRequest = new GetApplicationsApiRequest(query.CandidateId, query.Status);
            candidateApiClient
                .Setup(client => client.Get<GetApplicationsApiResponse>(
                    It.Is<GetApplicationsApiRequest>(r => r.GetUrl == expectedGetApplicationRequest.GetUrl)))
                .ReturnsAsync(applicationApiResponse);

            var expectedVacanciesRequest = new PostGetVacanciesByReferenceApiRequest(
                new PostGetVacanciesByReferenceApiRequestBody
                {
                    VacancyReferences = applicationApiResponse.Applications.Select(x => $"VAC{x.VacancyReference}")
                        .ToList()
                });

            faaApiClient.Setup(client =>
                    client.PostWithResponseCode<PostGetVacanciesByReferenceApiResponse>(
                        It.Is<PostGetVacanciesByReferenceApiRequest>(r =>
                            r.PostUrl == expectedVacanciesRequest.PostUrl), true))
                .ReturnsAsync(new ApiResponse<PostGetVacanciesByReferenceApiResponse>(vacanciesResponse, HttpStatusCode.OK, string.Empty));

            var result = await handler.Handle(query, CancellationToken.None);

            using var scope = new AssertionScope();
            result.Applications.Count.Should().Be(applicationApiResponse.Applications.Count);

            var expectedResult = new GetApplicationsQueryResult();

            foreach (var application in applicationApiResponse.Applications)
            {
                var vacancy = vacanciesResponse.ApprenticeshipVacancies.Single(x =>
                    x.VacancyReference == $"VAC{application.VacancyReference}");

                expectedResult.Applications.Add(new GetApplicationsQueryResult.Application
                {
                    Id = application.Id,
                    Title = vacancy.Title,
                    VacancyReference = vacancy.VacancyReference,
                    EmployerName = vacancy.EmployerName,
                    CreatedDate = application.CreatedDate,
                    ClosingDate = vacancy.ClosingDate
                });
            }

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
