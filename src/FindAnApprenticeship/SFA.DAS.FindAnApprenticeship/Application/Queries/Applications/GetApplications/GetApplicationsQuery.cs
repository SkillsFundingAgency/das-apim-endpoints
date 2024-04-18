using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplications
{
    public class GetApplicationsQuery : IRequest<GetApplicationsQueryResult>
    {
        public Guid CandidateId { get; set; }
    }

    public class GetApplicationsQueryResult
    {
        public List<Application> Applications { get; set; } = [];


        public class Application
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string EmployerName { get; set; }
        }
    }

    public class GetApplicationsQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient, IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient) : IRequestHandler<GetApplicationsQuery, GetApplicationsQueryResult>
    {
        public async Task<GetApplicationsQueryResult> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var applications =
                    await candidateApiClient.Get<GetApplicationsApiResponse>(
                        new GetApplicationsApiRequest(request.CandidateId));

                var vacancyReferences = applications.Applications.Select(x => $"VAC{x.VacancyReference}");

                var vacanciesRequest = new PostGetVacanciesByReferenceApiRequest(new PostGetVacanciesByReferenceApiRequestBody
                {
                    VacancyReferences = vacancyReferences.ToList()
                });

                var vacancies = await findApprenticeshipApiClient.PostWithResponseCode<PostGetVacanciesByReferenceApiResponse>(vacanciesRequest);

                var result = new GetApplicationsQueryResult();

                foreach (var application in applications.Applications)
                {
                    var vacancy = vacancies.Body.ApprenticeshipVacancies.Single(v => v.VacancyReference == $"VAC{application.VacancyReference}");

                    result.Applications.Add(new GetApplicationsQueryResult.Application
                    {
                        Id = application.Id,
                        EmployerName = vacancy.EmployerName,
                        Title = vacancy.Title
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

            
        }
    }
}
