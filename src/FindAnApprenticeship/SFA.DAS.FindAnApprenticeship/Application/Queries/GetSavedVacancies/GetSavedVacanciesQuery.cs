using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetSavedVacancies
{
    public class GetSavedVacanciesQuery : IRequest<GetSavedVacanciesQueryResult>
    {
        public Guid CandidateId { get; set; }
    }

    public class GetSavedVacanciesQueryResult
    {
        public List<SavedVacancy> SavedVacancies { get; set; } = [];

        public class SavedVacancy
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string VacancyReference { get; set; }
            public string EmployerName { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime ClosingDate { get; set; }
        }
    }

    public class GetSavedVacanciesQueryHandler(
        IVacancyService vacancyService,
        ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
        : IRequestHandler<GetSavedVacanciesQuery, GetSavedVacanciesQueryResult>
    {
        public async Task<GetSavedVacanciesQueryResult> Handle(GetSavedVacanciesQuery request, CancellationToken cancellationToken)
        {
            var candidateApiResponseTask =
                        candidateApiClient.Get<GetCandidateApiResponse>(new GetCandidateApiRequest(request.CandidateId.ToString()));

            var applicationsTask =
                candidateApiClient.Get<GetSavedVacanciesApiResponse>(
                    new GetSavedVacanciesApiRequest(request.CandidateId));

            var candidateApiResponse = candidateApiResponseTask.Result;
            var savedVacancyList = applicationsTask.Result.SavedVacancies;

            if (savedVacancyList.Count == 0) { return new GetSavedVacanciesQueryResult(); }

            var vacancyReferences = savedVacancyList.Select(x => $"{x.VacancyReference}").ToList();

            var vacancies = await vacancyService.GetVacancies(vacancyReferences);

            var result = new GetSavedVacanciesQueryResult();

            foreach (var application in savedVacancyList)
            {
                var vacancy = vacancies.FirstOrDefault(v => v.VacancyReference.Replace("VAC", string.Empty) == application.VacancyReference);
                
                result.SavedVacancies.Add(new GetSavedVacanciesQueryResult.SavedVacancy
                {
                    Id = application.Id,
                    VacancyReference = vacancy?.VacancyReference,
                    EmployerName = vacancy?.EmployerName,
                    Title = vacancy?.Title,
                    ClosingDate = vacancy?.ClosedDate ?? vacancy!.ClosingDate,
                    CreatedDate = application.CreatedOn,
                });
            }

            return result;
        }
    }
}
