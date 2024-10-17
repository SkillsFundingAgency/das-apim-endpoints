using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplications;
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
    public class WhenHandlingGetIndexQuery
    {
        [Test, MoqAutoData]
        public async Task Then_If_No_Applications_And_No_MigratedEmail_ShowBanner_Is_True(
            GetApplicationsQuery query,
            GetCandidateApiResponse candidateResponse,
            GetApplicationsApiResponse applicationApiResponse,
            List<ApprenticeshipVacancy> vacancies,
            [Frozen] Mock<IVacancyService> vacancyService,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetApplicationsQueryHandler handler)
        {
            var expectedGetApplicationRequest = new GetApplicationsApiRequest(query.CandidateId);
            candidateApiClient
                .Setup(client => client.Get<GetApplicationsApiResponse>(
                    It.Is<GetApplicationsApiRequest>(r => r.GetUrl == expectedGetApplicationRequest.GetUrl)))
                .ReturnsAsync(new GetApplicationsApiResponse
                {
                    Applications = []
                });
            candidateResponse.MigratedEmail = string.Empty;
            candidateApiClient
                .Setup(x => x.Get<GetCandidateApiResponse>(
                    It.Is<GetCandidateApiRequest>(c => c.GetUrl.Contains(query.CandidateId.ToString()))))
                .ReturnsAsync(candidateResponse);
            
            var result = await handler.Handle(query, CancellationToken.None);
            
            result.ShowAccountRecoveryBanner.Should().BeTrue();
        }
        [Test, MoqAutoData]
        public async Task Then_If_No_Applications_And_MigratedEmail_ShowBanner_Is_False(
            GetApplicationsQuery query,
            GetCandidateApiResponse candidateResponse,
            GetApplicationsApiResponse applicationApiResponse,
            List<ApprenticeshipVacancy> vacancies,
            [Frozen] Mock<IVacancyService> vacancyService,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetApplicationsQueryHandler handler)
        {
            var expectedGetApplicationRequest = new GetApplicationsApiRequest(query.CandidateId);
            candidateApiClient
                .Setup(client => client.Get<GetApplicationsApiResponse>(
                    It.Is<GetApplicationsApiRequest>(r => r.GetUrl == expectedGetApplicationRequest.GetUrl)))
                .ReturnsAsync(new GetApplicationsApiResponse
                {
                    Applications = []
                });
            candidateApiClient
                .Setup(x => x.Get<GetCandidateApiResponse>(
                    It.Is<GetCandidateApiRequest>(c => c.GetUrl.Contains(query.CandidateId.ToString()))))
                .ReturnsAsync(candidateResponse);
            
            var result = await handler.Handle(query, CancellationToken.None);

            result.ShowAccountRecoveryBanner.Should().BeFalse();
        }
        [Test, MoqAutoData]
        public async Task Then_The_QueryResult_Is_Returned_As_Expected(
            GetApplicationsQuery query,
            GetCandidateApiResponse candidateResponse,
            GetApplicationsApiResponse applicationApiResponse,
            List<ApprenticeshipVacancy> vacancies,
            [Frozen] Mock<IVacancyService> vacancyService,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetApplicationsQueryHandler handler)
        {
            for (var i = 0; i < applicationApiResponse.Applications.Count; i++)
            {
                applicationApiResponse.Applications[i].Status = ApplicationStatus.Draft.ToString();
                vacancies[i].VacancyReference = applicationApiResponse.Applications[i].VacancyReference;
            }

            var expectedGetApplicationRequest = new GetApplicationsApiRequest(query.CandidateId);
            candidateApiClient
                .Setup(client => client.Get<GetApplicationsApiResponse>(
                    It.Is<GetApplicationsApiRequest>(r => r.GetUrl == expectedGetApplicationRequest.GetUrl)))
                .ReturnsAsync(applicationApiResponse);
            
            candidateApiClient
                .Setup(x => x.Get<GetCandidateApiResponse>(
                    It.Is<GetCandidateApiRequest>(c => c.GetUrl.Contains(query.CandidateId.ToString()))))
                .ReturnsAsync(candidateResponse);

            vacancyService.Setup(x => x.GetVacancies(It.IsAny<List<string>>()))
                .ReturnsAsync(vacancies.Select(x => (IVacancy)x).ToList());

            var result = await handler.Handle(query, CancellationToken.None);

            using var scope = new AssertionScope();
            result.Applications.Count.Should().Be(applicationApiResponse.Applications.Count);

            var expectedResult = new GetApplicationsQueryResult();

            foreach (var application in applicationApiResponse.Applications)
            {
                var vacancy = vacancies.Single(x =>
                    x.VacancyReference == $"{application.VacancyReference}");

                expectedResult.Applications.Add(new GetApplicationsQueryResult.Application
                {
                    Id = application.Id,
                    Title = vacancy.Title,
                    VacancyReference = vacancy.VacancyReference,
                    EmployerName = vacancy.EmployerName,
                    CreatedDate = application.CreatedDate,
                    ClosingDate = vacancy.ClosedDate ?? vacancy.ClosingDate,
                    WithdrawnDate = application.WithdrawnDate,
                    SubmittedDate = application.SubmittedDate,
                    ResponseDate = application.ResponseDate,
                    ResponseNotes = application.ResponseNotes,
                });
            }

            result.Should().BeEquivalentTo(expectedResult);
        }
        
        [Test, MoqAutoData]
        public async Task Then_The_If_Submitted_Applications_Retrieved_Then_Withdrawn_Also_Returned(
            GetCandidateApiResponse candidateResponse,
            GetApplicationsQuery query,
            GetApplicationsApiResponse applicationApiResponse,
            List<ApprenticeshipVacancy> vacancies,
            [Frozen] Mock<IVacancyService> vacancyService,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetApplicationsQueryHandler handler)
        {
            query.Status = ApplicationStatus.Submitted;
            for (var i = 0; i < applicationApiResponse.Applications.Count; i++)
            {
                applicationApiResponse.Applications[i].Status = i == 0 ? ApplicationStatus.Submitted.ToString() : ApplicationStatus.Withdrawn.ToString();
                vacancies[i].VacancyReference = applicationApiResponse.Applications[i].VacancyReference;
            }

            var expectedGetApplicationRequest = new GetApplicationsApiRequest(query.CandidateId);
            candidateApiClient
                .Setup(client => client.Get<GetApplicationsApiResponse>(
                    It.Is<GetApplicationsApiRequest>(r => r.GetUrl == expectedGetApplicationRequest.GetUrl)))
                .ReturnsAsync(applicationApiResponse);

            candidateApiClient
                .Setup(x => x.Get<GetCandidateApiResponse>(
                    It.Is<GetCandidateApiRequest>(c => c.GetUrl.Contains(query.CandidateId.ToString()))))
                .ReturnsAsync(candidateResponse);

            vacancyService.Setup(x => x.GetVacancies(It.IsAny<List<string>>()))
                .ReturnsAsync(vacancies.Select(x => (IVacancy)x).ToList());

            var result = await handler.Handle(query, CancellationToken.None);

            using var scope = new AssertionScope();
            result.Applications.Count.Should().Be(applicationApiResponse.Applications.Count(x => x.Status == ApplicationStatus.Submitted.ToString() || x.Status == ApplicationStatus.Withdrawn.ToString()));

            var expectedResult = new GetApplicationsQueryResult();
            foreach (var application in applicationApiResponse.Applications)
            {
                var vacancy = vacancies.Single(x =>
                    x.VacancyReference == $"{application.VacancyReference}");

                expectedResult.Applications.Add(new GetApplicationsQueryResult.Application
                {
                    Id = application.Id,
                    Title = vacancy.Title,
                    VacancyReference = vacancy.VacancyReference,
                    EmployerName = vacancy.EmployerName,
                    CreatedDate = application.CreatedDate,
                    ClosingDate = vacancy.ClosedDate ?? vacancy.ClosingDate,
                    SubmittedDate = application.SubmittedDate,
                    WithdrawnDate = application.WithdrawnDate,
                    ResponseDate = application.ResponseDate,
                    ResponseNotes = application.ResponseNotes,
                    Status = Enum.Parse<ApplicationStatus>(application.Status)
                });
            }

            result.Should().BeEquivalentTo(expectedResult);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_Draft_Applications_Retrieved_Then_Expired_Also_Returned(
            GetCandidateApiResponse candidateResponse,
            GetApplicationsQuery query,
            GetApplicationsApiResponse applicationApiResponse,
            List<ApprenticeshipVacancy> vacancies,
            [Frozen] Mock<IVacancyService> vacancyService,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetApplicationsQueryHandler handler)
        {
            query.Status = ApplicationStatus.Draft;
            for (var i = 0; i < applicationApiResponse.Applications.Count; i++)
            {
                applicationApiResponse.Applications[i].Status = i == 0 ? ApplicationStatus.Draft.ToString() : ApplicationStatus.Expired.ToString();
                vacancies[i].VacancyReference = applicationApiResponse.Applications[i].VacancyReference;
            }

            var expectedGetApplicationRequest = new GetApplicationsApiRequest(query.CandidateId);
            candidateApiClient
                .Setup(client => client.Get<GetApplicationsApiResponse>(
                    It.Is<GetApplicationsApiRequest>(r => r.GetUrl == expectedGetApplicationRequest.GetUrl)))
                .ReturnsAsync(applicationApiResponse);

            candidateApiClient
                .Setup(x => x.Get<GetCandidateApiResponse>(
                    It.Is<GetCandidateApiRequest>(c => c.GetUrl.Contains(query.CandidateId.ToString()))))
                .ReturnsAsync(candidateResponse);

            vacancyService.Setup(x => x.GetVacancies(It.IsAny<List<string>>()))
                .ReturnsAsync(vacancies.Select(x => (IVacancy)x).ToList());

            var result = await handler.Handle(query, CancellationToken.None);

            using var scope = new AssertionScope();
            result.Applications.Count.Should().Be(applicationApiResponse.Applications.Count(x => x.Status == ApplicationStatus.Draft.ToString() || x.Status == ApplicationStatus.Expired.ToString()));

            var expectedResult = new GetApplicationsQueryResult();
            foreach (var application in applicationApiResponse.Applications)
            {
                var vacancy = vacancies.Single(x =>
                    x.VacancyReference == $"{application.VacancyReference}");

                expectedResult.Applications.Add(new GetApplicationsQueryResult.Application
                {
                    Id = application.Id,
                    Title = vacancy.Title,
                    VacancyReference = vacancy.VacancyReference,
                    EmployerName = vacancy.EmployerName,
                    CreatedDate = application.CreatedDate,
                    ClosingDate = vacancy.ClosedDate ?? vacancy.ClosingDate,
                    SubmittedDate = application.SubmittedDate,
                    WithdrawnDate = application.WithdrawnDate,
                    ResponseDate = application.ResponseDate,
                    ResponseNotes = application.ResponseNotes,
                    Status = Enum.Parse<ApplicationStatus>(application.Status)
                });
            }

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
