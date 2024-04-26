using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
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
        public List<Application> Applications { get; set; }


        public class Application
        {
            public Guid Id { get; set; }
        }
    }

    public class GetApplicationsQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient) : IRequestHandler<GetApplicationsQuery, GetApplicationsQueryResult>
    {
        public async Task<GetApplicationsQueryResult> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
        {
            var applications =
                await candidateApiClient.Get<GetApplicationsApiResponse>(
                    new GetApplicationsApiRequest(request.CandidateId));

            return new GetApplicationsQueryResult
            {
                Applications = applications.Applications.Select(x => new GetApplicationsQueryResult.Application
                {
                    Id = x.Id
                }).ToList()
            };
        }
    }
}
