using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Models;
using System.Linq;
using SFA.DAS.FindAnApprenticeship.Domain;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.WorkHistory
{
    public class GetJobsQueryHandler : IRequestHandler<GetJobsQuery, GetJobsQueryResult>
    {
        private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;

        public GetJobsQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
        {
            _candidateApiClient = candidateApiClient;
        }

        public async Task<GetJobsQueryResult> Handle(GetJobsQuery request, CancellationToken cancellationToken)
        {
            var applicationTask = _candidateApiClient.Get<GetApplicationApiResponse>(new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, false));
            var workHistoriesTask = _candidateApiClient.Get<GetWorkHistoriesApiResponse>(new GetWorkHistoriesApiRequest(request.ApplicationId, request.CandidateId, WorkHistoryType.Job));
            
            await Task.WhenAll(applicationTask, workHistoriesTask);
            
            var application = applicationTask.Result;
            var workHistories = workHistoriesTask.Result;
            
            bool? isCompleted = application.JobsStatus switch
            {
                Constants.SectionStatus.Incomplete => false,
                Constants.SectionStatus.Completed => true,
                _ => null
            };

            return new GetJobsQueryResult
            {
                IsSectionCompleted = isCompleted,
                Jobs = workHistories.WorkHistories.Select(x =>  new GetJobsQueryResult.Job
                    {
                        ApplicationId = x.ApplicationId,
                        Description = x.Description,
                        Employer = x.Employer,
                        EndDate = x.EndDate,
                        Id = x.Id,
                        JobTitle = x.JobTitle,
                        StartDate = x.StartDate
                    }).ToList()
            };
        }
    }
}
