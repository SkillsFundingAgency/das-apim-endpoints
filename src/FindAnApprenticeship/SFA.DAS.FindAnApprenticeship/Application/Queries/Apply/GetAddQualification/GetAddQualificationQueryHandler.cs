using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAddQualification;

public class GetAddQualificationQueryHandler(ICandidateApiClient<CandidateApiConfiguration> apiClient) : IRequestHandler<GetAddQualificationQuery,GetAddQualificationQueryResult>
{
    public async Task<GetAddQualificationQueryResult> Handle(GetAddQualificationQuery request, CancellationToken cancellationToken)
    {
        var response = await apiClient.GetWithResponseCode<GetQualificationReferenceTypesApiResponse>(
            new GetQualificationReferenceTypesApiRequest());
        var result = response.Body.QualificationReferences.FirstOrDefault(x => x.Id == request.QualificationReferenceTypeId);

        return new GetAddQualificationQueryResult
        {
            QualificationType = result
        };
    }
}