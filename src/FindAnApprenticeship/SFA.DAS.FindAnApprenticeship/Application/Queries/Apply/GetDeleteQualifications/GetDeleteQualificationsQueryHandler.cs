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
        var applicationRequest = new GetApplicationApiRequest(request.CandidateId, request.ApplicationId);
        var applicationTask = candidateApiClient.Get<GetApplicationApiResponse>(applicationRequest);

        var qualificationsRequest = new GetQualificationsApiRequest(request.ApplicationId, request.CandidateId, request.QualificationReference);
        var qualificationsTask = candidateApiClient.Get<GetQualificationsApiResponse>(qualificationsRequest);

        await Task.WhenAll(applicationTask, qualificationsTask);

        var application = applicationTask.Result;
        var qualifications = qualificationsTask.Result;

        return new GetDeleteQualificationsQueryResult
        {
            QualificationReference = request.QualificationReference,
            Qualifications = qualifications.Qualifications.Select(x => new GetDeleteQualificationsQueryResult.Qualification
            {
                AdditionalInformation = x.AdditionalInformation,
                Grade = x.Grade,
                IsPredicted = x.IsPredicted,
                Subject = x.Subject
            }).ToList()

        };
    }
}