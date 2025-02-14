﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

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
            var response = await recruitApiClient.Get<GetClosedVacancyResponse>(new GetClosedVacancyRequest(vacancyReference.Replace("VAC", "")));
            if (response is not { IsAnonymous: true })
            {
                return response;
            }
            
            switch (response.EmployerLocationOption)
            {
                case AvailableWhere.OneLocation:
                case AvailableWhere.MultipleLocations:
                    response.EmployerLocations?.ForEach(x => x.Anonymise());
                    break;
                case AvailableWhere.AcrossEngland:
                    break;
                default:
                    response.EmployerLocation.Anonymise();
                    break;
            }

            return response;
        }

        public async Task<List<IVacancy>> GetVacancies(List<string> vacancyReferences)
        {
            var vacanciesRequest = new PostGetVacanciesByReferenceApiRequest(new PostGetVacanciesByReferenceApiRequestBody
            {
                VacancyReferences = vacancyReferences.Select(x => $"VAC{x}").ToList()
            });

            var vacancies = await findApprenticeshipApiClient.PostWithResponseCode<PostGetVacanciesByReferenceApiResponse>(vacanciesRequest);

            var result = vacancies.Body.ApprenticeshipVacancies.Select(IVacancy (x) => x).ToList();

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

        public string GetVacancyWorkLocation(IVacancy vacancy)
        {
            switch (vacancy.EmployerLocationOption)
            {
                case AvailableWhere.AcrossEngland:
                    return "Recruiting nationally";
                case AvailableWhere.MultipleLocations:
                {
                    var addresses = new List<Address> { vacancy.Address };
                    if (vacancy.OtherAddresses != null) addresses.AddRange(vacancy.OtherAddresses);
                    return EmailTemplateAddressExtension.GetEmploymentLocations(addresses);
                }
                case null:
                case AvailableWhere.OneLocation:
                default:
                    return EmailTemplateAddressExtension.GetOneLocationCityName(vacancy.Address);
            }
        }
    }
}