using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Domain;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Qualifications;

public class GetQualificationsQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient) : IRequestHandler<GetQualificationsQuery, GetQualificationsQueryResult>
{
    public async Task<GetQualificationsQueryResult> Handle(GetQualificationsQuery request, CancellationToken cancellationToken)
    {
        var applicationTask = candidateApiClient.Get<GetApplicationApiResponse>(new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, false));
        var qualificationTypesTask = candidateApiClient.Get<GetQualificationReferenceTypesApiResponse>(new GetQualificationReferenceTypesApiRequest());
        var qualificationsTask = candidateApiClient.Get<GetQualificationsApiResponse>(new GetQualificationsApiRequest(request.ApplicationId, request.CandidateId));

        await Task.WhenAll(applicationTask, qualificationTypesTask, qualificationsTask);

        var application = applicationTask.Result;
        var qualifications = qualificationsTask.Result;
        var qualificationTypes = qualificationTypesTask.Result;

        bool? isCompleted = application.QualificationsStatus switch
        {
            Constants.SectionStatus.Incomplete => false,
            Constants.SectionStatus.Completed => true,
            _ => null
        };

        return new GetQualificationsQueryResult
        {
            IsSectionCompleted = isCompleted,
            Qualifications = qualifications.Qualifications.ToList(),
            QualificationTypes = qualificationTypes.QualificationReferences.ToList()
        };
    }
}