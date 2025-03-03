using SFA.DAS.FindAnApprenticeship.Application.Queries.Users.GetAccountDeletionQuery;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using static SFA.DAS.FindAnApprenticeship.InnerApi.Responses.PostGetVacanciesByReferenceApiResponse;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Applications
{
    [TestFixture]
    public class WhenHandlingGetAccountDeletionQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_QueryResult_Is_Returned_As_Expected(
            GetAccountDeletionQuery query,
            GetApplicationsApiResponse applicationApiResponse,
            List<ApprenticeshipVacancy> vacancies,
            [Frozen] Mock<IVacancyService> vacancyService,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetAccountDeletionQueryHandler handler)
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

            var expectedResult = new GetAccountDeletionQueryResult();

            foreach (var application in applicationApiResponse.Applications)
            {
                var vacancy = vacancies.Single(x =>
                    x.VacancyReference == $"{application.VacancyReference}");

                expectedResult.SubmittedApplications.Add(new GetAccountDeletionQueryResult.Application
                {
                    Id = application.Id,
                    Title = vacancy.Title,
                    VacancyReference = vacancy.VacancyReference,
                    EmployerName = vacancy.EmployerName,
                    CreatedDate = application.CreatedDate,
                    ClosingDate = vacancy.ClosedDate ?? vacancy.ClosingDate,
                    SubmittedDate = application.SubmittedDate,
                    Address = vacancy?.Address,
                    OtherAddresses = vacancy?.OtherAddresses,
                    EmployerLocationOption = vacancy?.EmployerLocationOption,
                    EmploymentLocationInformation = vacancy?.EmploymentLocationInformation,
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
           GetAccountDeletionQuery query,
           GetApplicationsApiResponse applicationApiResponse,
           List<ApprenticeshipVacancy> vacancies,
           [Frozen] Mock<IVacancyService> vacancyService,
           [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
           GetAccountDeletionQueryHandler handler)
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
