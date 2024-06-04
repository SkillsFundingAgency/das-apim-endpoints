using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetDeleteQualifications;

public class GetDeleteQualificationsQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient) : IRequestHandler<GetDeleteQualificationsQuery, GetDeleteQualificationsQueryResult>
{
    public async Task<GetDeleteQualificationsQueryResult> Handle(GetDeleteQualificationsQuery request, CancellationToken cancellationToken)
    {
        var qualificationsTask = candidateApiClient.Get<GetQualificationsApiResponse>(new GetQualificationsApiRequest(request.ApplicationId, request.CandidateId, request.QualificationReference));

        var qualificationTypesTask = candidateApiClient.Get<GetQualificationReferenceTypesApiResponse>(new GetQualificationReferenceTypesApiRequest());

        await Task.WhenAll(qualificationTypesTask, qualificationsTask);

        var qualifications = qualificationsTask.Result;
        var qualificationTypes = qualificationTypesTask.Result;

        var qualification = qualificationTypes.QualificationReferences.Single(x => x.Id == request.QualificationReference);

        return new GetDeleteQualificationsQueryResult
        {
            Qualifications = qualifications.Qualifications,
            QualificationReference = qualification.Name,
        };
    }
}