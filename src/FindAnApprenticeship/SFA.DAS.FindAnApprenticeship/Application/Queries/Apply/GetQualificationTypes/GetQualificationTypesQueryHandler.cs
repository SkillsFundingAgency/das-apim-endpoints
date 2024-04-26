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
        var referenceDataTask = apiClient.GetWithResponseCode<GetQualificationReferenceTypesApiResponse>(
                new GetQualificationReferenceTypesApiRequest());

        var qualificationsTask = apiClient.GetWithResponseCode<GetQualificationsApiResponse>(
            new GetQualificationsApiRequest(request.ApplicationId, request.CandidateId));

        await Task.WhenAll(referenceDataTask, qualificationsTask);

        var referenceData = referenceDataTask.Result;
        var qualifications = qualificationsTask.Result;

        return new GetQualificationTypesQueryResult
        {
            HasAddedQualifications = qualifications.Body.Qualifications.Count > 0,
            QualificationTypes = referenceData.Body.QualificationReferences
        };
    }
}