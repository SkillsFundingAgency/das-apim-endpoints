using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Services
{
    public class VacancyService(IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient,
        IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) : IVacancyService
    {
        public async Task<IVacancy> GetVacancy(string vacancyReference)
        {
            var result = await findApprenticeshipApiClient.Get<GetApprenticeshipVacancyItemResponse>(new GetVacancyRequest(vacancyReference));

            if (result != null) return result;

            var closedVacancy = await recruitApiClient.Get<GetClosedVacancyResponse>(new GetClosedVacancyRequest(vacancyReference));

            return closedVacancy;
        }

        public async Task<List<IVacancy>> GetVacancies(List<string> vacancyReferences)
        {
            var vacanciesRequest = new PostGetVacanciesByReferenceApiRequest(new PostGetVacanciesByReferenceApiRequestBody
            {
                VacancyReferences = vacancyReferences.Select(x => $"VAC{x}").ToList()
            });

            var vacancies = await findApprenticeshipApiClient.PostWithResponseCode<PostGetVacanciesByReferenceApiResponse>(vacanciesRequest);

            var result = vacancies.Body.ApprenticeshipVacancies.Select(x => (IVacancy)x).ToList();

            var notFoundVacancies = vacancyReferences.Where(x => result.All(y => y.VacancyReference != $"VAC{x}")).ToList();

            if (notFoundVacancies.Any())
            {
                var recruitRequest = new PostGetClosedVacanciesByReferenceApiRequest(
                    new PostGetClosedVacanciesByReferenceApiRequestBody
                    {
                        VacancyReferences = notFoundVacancies.Select(x => Convert.ToInt64(x)).ToList()
                    });

                var recruitResponse =
                    await recruitApiClient.PostWithResponseCode<GetClosedVacanciesByReferenceResponse>(recruitRequest);

                result.AddRange(recruitResponse.Body.Vacancies);
            }

            return result;
        }
    }
}
