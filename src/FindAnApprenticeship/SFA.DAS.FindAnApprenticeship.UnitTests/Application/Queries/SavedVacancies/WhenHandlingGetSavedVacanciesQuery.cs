using SFA.DAS.FindAnApprenticeship.Application.Queries.GetSavedVacancies;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.SavedVacancies
{
    [TestFixture]
    public class WhenHandlingGetSavedVacanciesQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Vacancy_And_Candidate_Data_Is_Returned(
            GetSavedVacanciesQuery query,
            GetApprenticeshipVacancyItemResponse vacancy,
            GetApplicationByReferenceApiResponse applicationResponse,
            GetSavedVacanciesApiResponse savedVacanciesApiResponse,
            List<IVacancy> vacancies,
            [Frozen] Mock<IVacancyService> vacancyService,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetSavedVacanciesQueryHandler handler)
        {
            // Arrange
            for (var i = 0; i < vacancies.Count; i++)
            {
                savedVacanciesApiResponse.SavedVacancies[i].VacancyReference = vacancies[i].VacancyReference;
            }

            var expectedGetSavedVacanciesApiRequest =
                new GetSavedVacanciesApiRequest(query.CandidateId);

            candidateApiClient
                .Setup(client =>
                    client.Get<GetSavedVacanciesApiResponse>(
                        It.Is<GetSavedVacanciesApiRequest>(c => c.GetUrl == expectedGetSavedVacanciesApiRequest.GetUrl)))
                .ReturnsAsync(savedVacanciesApiResponse);

            var savedVacancyList = savedVacanciesApiResponse.SavedVacancies;

            var vacancyReferences = savedVacancyList.Select(x => $"{x.VacancyReference.TrimVacancyReference()}").ToList();
            var vacancyIds = savedVacancyList.Select(x => $"{x.Id}").ToList();

            vacancyService
                .Setup(x => x.GetVacancies(vacancyReferences))
                .ReturnsAsync(vacancies);

            foreach (var expectedGetApplicationByReferenceApiRequest in vacancyIds.Select(vacancyId => new GetApplicationByReferenceApiRequest(query.CandidateId, vacancyId)))
            {
                candidateApiClient
                    .Setup(client =>
                        client.Get<GetApplicationByReferenceApiResponse>(
                            It.Is<GetApplicationByReferenceApiRequest>(c => c.GetUrl == expectedGetApplicationByReferenceApiRequest.GetUrl)))
                    .ReturnsAsync(applicationResponse);
            }

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.SavedVacancies.Should().BeEquivalentTo(savedVacanciesApiResponse.SavedVacancies, options => options
                .Excluding(x => x.CandidateId)
                .Excluding(x => x.CreatedOn)
                .Excluding(x => x.VacancyId)
            );
        }

        [Test, MoqAutoData]
        public async Task Then_Application_Not_Found_The_Vacancy_And_Candidate_Data_Is_Returned(
            GetSavedVacanciesQuery query,
            GetApprenticeshipVacancyItemResponse vacancy,
            GetSavedVacanciesApiResponse savedVacanciesApiResponse,
            List<IVacancy> vacancies,
            [Frozen] Mock<IVacancyService> vacancyService,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetSavedVacanciesQueryHandler handler)
        {
            // Arrange
            for (var i = 0; i < vacancies.Count; i++)
            {
                savedVacanciesApiResponse.SavedVacancies[i].VacancyReference = vacancies[i].VacancyReference;
            }

            var expectedGetSavedVacanciesApiRequest =
                new GetSavedVacanciesApiRequest(query.CandidateId);

            candidateApiClient
                .Setup(client =>
                    client.Get<GetSavedVacanciesApiResponse>(
                        It.Is<GetSavedVacanciesApiRequest>(c => c.GetUrl == expectedGetSavedVacanciesApiRequest.GetUrl)))
                .ReturnsAsync(savedVacanciesApiResponse);

            var savedVacancyList = savedVacanciesApiResponse.SavedVacancies;

            var vacancyReferences = savedVacancyList.Select(x => $"{x.VacancyReference.TrimVacancyReference()}").ToList();
            var vacancyIds = savedVacancyList.Select(x => $"{x.Id}").ToList();

            vacancyService
                .Setup(x => x.GetVacancies(vacancyReferences))
                .ReturnsAsync(vacancies);

            foreach (var expectedGetApplicationByReferenceApiRequest in vacancyIds.Select(vacancyId => new GetApplicationByReferenceApiRequest(query.CandidateId, vacancyId)))
            {
                candidateApiClient
                    .Setup(client =>
                        client.Get<GetApplicationByReferenceApiResponse>(
                            It.Is<GetApplicationByReferenceApiRequest>(c => c.GetUrl == expectedGetApplicationByReferenceApiRequest.GetUrl)))
                    .ReturnsAsync((GetApplicationByReferenceApiResponse)null!);
            }

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.SavedVacancies.Should().BeEquivalentTo(savedVacanciesApiResponse.SavedVacancies, options => options
                .Excluding(x => x.CandidateId)
                .Excluding(x => x.CreatedOn)
                .Excluding(x => x.VacancyId)
            );

            result.SavedVacancies.ForEach(x => x.ApplicationStatus.Should().BeNullOrEmpty());
        }

        [Test, MoqAutoData]
        public async Task Then_Vacancies_Not_Found_And_Empty_Is_Returned(
            GetSavedVacanciesQuery query,
            GetApprenticeshipVacancyItemResponse vacancy,
            GetSavedVacanciesApiResponse savedVacanciesApiResponse,
            List<IVacancy> vacancies,
            [Frozen] Mock<IVacancyService> vacancyService,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetSavedVacanciesQueryHandler handler)
        {
            // Arrange
            var expectedGetSavedVacanciesApiRequest =
                new GetSavedVacanciesApiRequest(query.CandidateId);

            candidateApiClient
                .Setup(client =>
                    client.Get<GetSavedVacanciesApiResponse>(
                        It.Is<GetSavedVacanciesApiRequest>(c => c.GetUrl == expectedGetSavedVacanciesApiRequest.GetUrl)))
                .ReturnsAsync(savedVacanciesApiResponse);

            var savedVacancyList = savedVacanciesApiResponse.SavedVacancies;

            var vacancyReferences = savedVacancyList.Select(x => $"{x.VacancyReference}").ToList();

            vacancyService
                .Setup(x => x.GetVacancies(vacancyReferences))
                .ReturnsAsync(vacancies);

            foreach (var expectedGetApplicationByReferenceApiRequest in vacancyReferences.Select(vacancyReference => new GetApplicationByReferenceApiRequest(query.CandidateId, vacancyReference)))
            {
                candidateApiClient
                    .Setup(client =>
                        client.Get<GetApplicationByReferenceApiResponse>(
                            It.Is<GetApplicationByReferenceApiRequest>(c => c.GetUrl == expectedGetApplicationByReferenceApiRequest.GetUrl)))
                    .ReturnsAsync((GetApplicationByReferenceApiResponse)null!);
            }

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.SavedVacancies.ForEach(x => x.ApplicationStatus.Should().BeEmpty());
        }
    }
}
