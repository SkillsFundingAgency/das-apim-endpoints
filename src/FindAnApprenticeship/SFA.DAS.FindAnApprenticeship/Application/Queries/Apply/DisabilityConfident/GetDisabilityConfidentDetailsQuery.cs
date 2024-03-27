using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.DisabilityConfident;

public class GetDisabilityConfidentDetailsQuery : IRequest<GetDisabilityConfidentDetailsQueryResult>
{
    public Guid CandidateId { get; set; }
    public Guid ApplicationId { get; set; }
}

public class GetDisabilityConfidentDetailsQueryResult
{
    public bool? ApplyUnderDisabilityConfidentScheme { get; set; }
    public bool? IsSectionCompleted { get; set; }
}

public class GetDisabilityConfidentDetailsQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient) : IRequestHandler<GetDisabilityConfidentDetailsQuery, GetDisabilityConfidentDetailsQueryResult>
{
    public async Task<GetDisabilityConfidentDetailsQueryResult> Handle(GetDisabilityConfidentDetailsQuery request, CancellationToken cancellationToken)
    {
        var applicationRequest = new GetApplicationApiRequest(request.CandidateId, request.ApplicationId);
        var application = await candidateApiClient.Get<GetApplicationApiResponse>(applicationRequest);

        if (application is null)
        {
            return null;
        }

        bool? isCompleted = application.DisabilityConfidenceStatus switch
        {
            Domain.Constants.SectionStatus.Incomplete => false,
            Domain.Constants.SectionStatus.Completed => true,
            _ => null
        };

        return new GetDisabilityConfidentDetailsQueryResult
        {
            ApplyUnderDisabilityConfidentScheme = application.ApplyUnderDisabilityConfidentScheme,
            IsSectionCompleted = isCompleted
        };
    }
}