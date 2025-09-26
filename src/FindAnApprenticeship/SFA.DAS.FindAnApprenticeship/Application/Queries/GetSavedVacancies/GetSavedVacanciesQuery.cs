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
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

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
            public string? VacancyId { get; set; }
            public string Title { get; set; }
            public string VacancyReference { get; set; }
            public string EmployerName { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime ClosingDate { get; set; }
            public bool IsExternalVacancy { get; set; }
            public string ExternalVacancyUrl { get; set; }
            public string ApplicationStatus { get; set; }
            public Address Address { get; set; }
            public List<Address>? OtherAddresses { get; set; } = [];
            public string? EmploymentLocationInformation { get; set; }
            public AvailableWhere? EmployerLocationOption { get; set; }
        }
    }

    public class GetSavedVacanciesQueryHandler(
        IVacancyService vacancyService,
        ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
        : IRequestHandler<GetSavedVacanciesQuery, GetSavedVacanciesQueryResult>
    {
        public async Task<GetSavedVacanciesQueryResult> Handle(GetSavedVacanciesQuery request, CancellationToken cancellationToken)
        {
            var savedVacanciesResponse =
                await candidateApiClient.Get<GetSavedVacanciesApiResponse>(
                    new GetSavedVacanciesApiRequest(request.CandidateId));

            var savedVacancyList = savedVacanciesResponse.SavedVacancies
                .GroupBy(x => x.VacancyReference.TrimVacancyReference())
                .Select(g => g.OrderByDescending(x => x.CreatedOn).First())
                .ToList();

            if (savedVacancyList.Count == 0) { return new GetSavedVacanciesQueryResult(); }

            var vacancyReferences = savedVacancyList
                .Select(x => x.VacancyReference.TrimVacancyReference())
                .ToList();
            var vacancies = await vacancyService.GetVacancies(vacancyReferences);

            var result = new GetSavedVacanciesQueryResult();

            foreach (var application in savedVacancyList)
            {
                var vacancy = vacancies.FirstOrDefault(v => v.VacancyReference.TrimVacancyReference() == application.VacancyReference);

                if (vacancy == null) continue;

                var applicationResult = await candidateApiClient.Get<GetApplicationByReferenceApiResponse>(
                    new GetApplicationByReferenceApiRequest(request.CandidateId, application.VacancyReference));

                result.SavedVacancies.Add(new GetSavedVacanciesQueryResult.SavedVacancy
                {
                    Id = application.Id,
                    VacancyReference = vacancy.VacancyReference,
                    EmployerName = vacancy.EmployerName,
                    Title = vacancy.Title,
                    ClosingDate = vacancy.ClosedDate ?? vacancy.ClosingDate,
                    CreatedDate = application.CreatedOn,
                    Address = vacancy.Address,
                    OtherAddresses = vacancy.OtherAddresses?.ToList(),
                    EmployerLocationOption = vacancy.EmployerLocationOption,
                    EmploymentLocationInformation = vacancy.EmploymentLocationInformation,
                    IsExternalVacancy = vacancy.IsExternalVacancy,
                    ExternalVacancyUrl = vacancy.ExternalVacancyUrl,
                    ApplicationStatus = applicationResult != null ? applicationResult.Status : string.Empty
                });

            }

            return result;
        }
    }
}
