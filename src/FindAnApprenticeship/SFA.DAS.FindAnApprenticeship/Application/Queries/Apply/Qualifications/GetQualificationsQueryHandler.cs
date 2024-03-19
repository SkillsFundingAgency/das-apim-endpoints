using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Domain;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using static SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses.GetQualificationsApiResponse;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Qualifications;

public class GetQualificationsQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient) : IRequestHandler<GetQualificationsQuery, GetQualificationsQueryResult>
{
    public async Task<GetQualificationsQueryResult> Handle(GetQualificationsQuery request, CancellationToken cancellationToken)
    {
        var applicationRequest = new GetApplicationApiRequest(request.CandidateId, request.ApplicationId);
        var applicationTask = candidateApiClient.Get<GetApplicationApiResponse>(applicationRequest);

        var qualificationTypesRequest = new GetQualificationsReferenceDataApiRequest();
        var qualificationTypesTask = candidateApiClient.Get<GetQualificationsReferenceDataApiResponse>(qualificationTypesRequest);

        var qualificationsRequest = new GetQualificationsApiRequest(request.ApplicationId, request.CandidateId);
        var qualificationsTask = candidateApiClient.Get<GetQualificationsApiResponse>(qualificationsRequest);

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
            Qualifications = qualifications.Qualifications.Select(x => new GetQualificationsQueryResult.Qualification
            {
                Id = x.Id,
                AdditionalInformation = x.AdditionalInformation,
                Grade = x.Grade,
                IsPredicted = x.IsPredicted,
                Subject = x.Subject,
                QualificationReference =x.QualificationReference.Id
            }).ToList(),
            QualificationTypes = qualificationTypes.QualificationReferences.Select(x => new GetQualificationsQueryResult.QualificationReferenceDataItem
            {
                Id = x.Id,
                Name = x.Name,
                Order = x.Order
            }).ToList()
        };
    }
}