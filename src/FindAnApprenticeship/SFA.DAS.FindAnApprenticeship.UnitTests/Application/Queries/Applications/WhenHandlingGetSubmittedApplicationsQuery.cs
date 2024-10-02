using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetSubmittedApplications;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.FindAnApprenticeship.InnerApi.Responses.PostGetVacanciesByReferenceApiResponse;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Applications
{
    [TestFixture]
    public class WhenHandlingGetSubmittedApplicationsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_QueryResult_Is_Returned_As_Expected(
            GetSubmittedApplicationsQuery query,
            GetApplicationsApiResponse applicationApiResponse,
            List<ApprenticeshipVacancy> vacancies,
            [Frozen] Mock<IVacancyService> vacancyService,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetSubmittedApplicationsQueryHandler handler)
        {
            for (var i = 0; i < applicationApiResponse.Applications.Count; i++)
            {
                applicationApiResponse.Applications[i].Status = ApplicationStatus.Submitted.ToString();
                vacancies[i].VacancyReference = applicationApiResponse.Applications[i].VacancyReference;
            }

            var expectedGetApplicationRequest = new GetApplicationsApiRequest(query.CandidateId);
            candidateApiClient
                .Setup(client => client.Get<GetApplicationsApiResponse>(
                    It.Is<GetApplicationsApiRequest>(r => r.GetUrl == expectedGetApplicationRequest.GetUrl)))
                .ReturnsAsync(applicationApiResponse);
           

            vacancyService.Setup(x => x.GetVacancies(It.IsAny<List<string>>()))
                .ReturnsAsync(vacancies.Select(x => (IVacancy)x).ToList());

            var result = await handler.Handle(query, CancellationToken.None);

            using var scope = new AssertionScope();
            result.SubmittedApplications.Count.Should().Be(applicationApiResponse.Applications.Count);

            var expectedResult = new GetSubmittedApplicationsQueryResult();

            foreach (var application in applicationApiResponse.Applications)
            {
                var vacancy = vacancies.Single(x =>
                    x.VacancyReference == $"{application.VacancyReference}");

                expectedResult.SubmittedApplications.Add(new GetSubmittedApplicationsQueryResult.Application
                {
                    Id = application.Id,
                    Title = vacancy.Title,
                    VacancyReference = vacancy.VacancyReference,
                    EmployerName = vacancy.EmployerName,
                    CreatedDate = application.CreatedDate,
                    ClosingDate = vacancy.ClosedDate ?? vacancy.ClosingDate,
                    SubmittedDate = application.SubmittedDate,
                    City = vacancy.City,
                    Postcode = vacancy.Postcode,
                    Status = Enum.Parse<ApplicationStatus>(application.Status)
                });
            }

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        [MoqInlineAutoData(ApplicationStatus.Withdrawn)]
        [MoqInlineAutoData(ApplicationStatus.Draft)]
        [MoqInlineAutoData(ApplicationStatus.Expired)]
        [MoqInlineAutoData(ApplicationStatus.UnSuccessful)]
        [MoqInlineAutoData(ApplicationStatus.Successful)]
        public async Task Then_The_QueryResult_Is_Returned_Then_Other_ApplicationStatus_Not_Included(
           ApplicationStatus status,
           GetSubmittedApplicationsQuery query,
           GetApplicationsApiResponse applicationApiResponse,
           List<ApprenticeshipVacancy> vacancies,
           [Frozen] Mock<IVacancyService> vacancyService,
           [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
           GetSubmittedApplicationsQueryHandler handler)
        {
            for (var i = 0; i < applicationApiResponse.Applications.Count; i++)
            {
                applicationApiResponse.Applications[i].Status = status.ToString();
                vacancies[i].VacancyReference = applicationApiResponse.Applications[i].VacancyReference;
            }

            var expectedGetApplicationRequest = new GetApplicationsApiRequest(query.CandidateId);
            candidateApiClient
                .Setup(client => client.Get<GetApplicationsApiResponse>(
                    It.Is<GetApplicationsApiRequest>(r => r.GetUrl == expectedGetApplicationRequest.GetUrl)))
                .ReturnsAsync(applicationApiResponse);


            vacancyService.Setup(x => x.GetVacancies(It.IsAny<List<string>>()))
                .ReturnsAsync(vacancies.Select(x => (IVacancy)x).ToList());

            var result = await handler.Handle(query, CancellationToken.None);

            using var scope = new AssertionScope();
            result.SubmittedApplications.Count.Should().Be(0);
        }
    }
}
