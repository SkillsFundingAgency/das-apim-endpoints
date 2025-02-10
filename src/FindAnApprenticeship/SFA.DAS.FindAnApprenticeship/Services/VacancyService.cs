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
using static SFA.DAS.FindAnApprenticeship.InnerApi.Responses.PostGetVacanciesByReferenceApiResponse;

namespace SFA.DAS.FindAnApprenticeship.Services
{
    public class VacancyService(IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient,
        IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) : IVacancyService
    {
        public async Task<IVacancy> GetVacancy(string vacancyReference)
        {
            return await findApprenticeshipApiClient.Get<GetApprenticeshipVacancyItemResponse>(new GetVacancyRequest(vacancyReference));
        }

        public async Task<IVacancy> GetClosedVacancy(string vacancyReference)
        {
            return await recruitApiClient.Get<GetClosedVacancyResponse>(new GetClosedVacancyRequest(vacancyReference.Replace("VAC", "")));
        }

        public async Task<List<ApprenticeshipVacancy>> GetVacancies(List<string> vacancyReferences)
        {
            var vacanciesRequest = new PostGetVacanciesByReferenceApiRequest(new PostGetVacanciesByReferenceApiRequestBody
            {
                VacancyReferences = vacancyReferences.Select(x => $"VAC{x}").ToList()
            });

            var vacancies = await findApprenticeshipApiClient.PostWithResponseCode<PostGetVacanciesByReferenceApiResponse>(vacanciesRequest);

            var result = vacancies.Body.ApprenticeshipVacancies.ToList();

            return result;
        }

        public async Task<List<IVacancy>> GetClosedVacancies(List<string> vacancyReferences)
        {
            var result = new List<IVacancy>();

            var recruitRequest = new PostGetClosedVacanciesByReferenceApiRequest(
                new PostGetClosedVacanciesByReferenceApiRequestBody
                {
                    VacancyReferences = vacancyReferences.Select(x => Convert.ToInt64(x)).ToList()
                });

            var recruitResponse =
                await recruitApiClient.PostWithResponseCode<GetClosedVacanciesByReferenceResponse>(recruitRequest);

            result.AddRange(recruitResponse.Body.Vacancies);

            return result;
        }
    }
}
