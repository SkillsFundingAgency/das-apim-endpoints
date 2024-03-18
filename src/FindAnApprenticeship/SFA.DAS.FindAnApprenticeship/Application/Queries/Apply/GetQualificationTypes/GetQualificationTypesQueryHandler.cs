using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetQualificationTypes;

public class GetQualificationTypesQueryHandler(ICandidateApiClient<CandidateApiConfiguration> apiClient) : IRequestHandler<GetQualificationTypesQuery, GetQualificationTypesQueryResult>
{
    public async Task<GetQualificationTypesQueryResult> Handle(GetQualificationTypesQuery request, CancellationToken cancellationToken)
    {
        var response = await apiClient.GetWithResponseCode<GetQualificationReferenceTypesApiResponse>(
                new GetQualificationReferenceTypesApiRequest());

        return new GetQualificationTypesQueryResult
        {
            QualificationTypes = response.Body.QualificationReferences
        };
    }
}