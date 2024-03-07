using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Domain;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.DisabilityConfident
{
    public class GetDisabilityConfidentQuery : IRequest<GetDisabilityConfidentQueryResult>
    {
        public Guid CandidateId { get; set; }
        public Guid ApplicationId { get; set; }
    }

    public class GetDisabilityConfidentQueryResult
    {
        public bool? ApplyUnderDisabilityConfidentScheme { get; set; }
        public bool? IsSectionCompleted { get; set; }
    }

    public class GetDisabilityConfidentQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient) : IRequestHandler<GetDisabilityConfidentQuery, GetDisabilityConfidentQueryResult>
    {
        public async Task<GetDisabilityConfidentQueryResult> Handle(GetDisabilityConfidentQuery request, CancellationToken cancellationToken)
        {
            var applicationRequest = new GetApplicationApiRequest(request.CandidateId, request.ApplicationId);
            var application = await candidateApiClient.Get<GetApplicationApiResponse>(applicationRequest);

            if (application == null)
            {
                return null;
            }

            bool? isCompleted = application.InterestsStatus switch
            {
                Constants.SectionStatus.InProgress => false,
                Constants.SectionStatus.Completed => true,
                _ => null
            };

            return new GetDisabilityConfidentQueryResult
            {
                ApplyUnderDisabilityConfidentScheme = application.ApplyUnderDisabilityConfidentScheme,
                IsSectionCompleted = isCompleted
            };
        }
    }
}
