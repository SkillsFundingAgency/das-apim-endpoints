using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests.Qualifications;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses.Qualifications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetDeleteQualification;

public class GetDeleteQualificationQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient) : IRequestHandler<GetDeleteQualificationQuery, GetDeleteQualificationQueryResult>
{
    public async Task<GetDeleteQualificationQueryResult> Handle(GetDeleteQualificationQuery request, CancellationToken cancellationToken)
    {
        var applicationTask = candidateApiClient.Get<GetApplicationApiResponse>(new GetApplicationApiRequest(request.CandidateId, request.ApplicationId));

        var qualificationsTask = candidateApiClient.Get<GetQualificationApiResponse>(new GetQualificationApiRequest(request.ApplicationId, request.CandidateId, request.Id));

        var qualificationTypesTask = candidateApiClient.Get<GetQualificationReferenceTypesApiResponse>(new GetQualificationReferenceTypesApiRequest());

        await Task.WhenAll(applicationTask, qualificationTypesTask, qualificationsTask);

        var application = applicationTask.Result;
        var qualifications = qualificationsTask.Result;
        var qualificationTypes = qualificationTypesTask.Result;

        var qualification = qualificationTypes.QualificationReferences.Single(x => x.Id == request.QualificationReference);

        return new GetDeleteQualificationQueryResult
        {
            Qualifications = [qualifications.Qualification],
            QualificationReference = qualification.Name,
        };
    }
}