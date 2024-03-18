using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Domain;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Qualifications
{
    public class GetQualificationsQuery : IRequest<GetQualificationsQueryResult>
    {
        public Guid CandidateId { get; set; }
        public Guid ApplicationId { get; set; }
    }

    public class GetQualificationsQueryResult
    {
        public bool? IsSectionCompleted { get; set; }
        public List<Qualification> Qualifications { get; set; }
        public List<QualificationReferenceDataItem> QualificationTypes { get; set; }

        public class Qualification
        {
            public Guid Id { get; set; }
            public string? Subject { get; set; }
            public string? Grade { get; set; }
            public string? AdditionalInformation { get; set; }
            public bool? IsPredicted { get; set; }
        }

        public class QualificationReferenceDataItem
        {
            public Guid Id { get; set; }
            public required string Name { get; set; }
            public short Order { get; set; }
        }
    }

    public class GetQualificationsQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient) : IRequestHandler<GetQualificationsQuery, GetQualificationsQueryResult>
    {
        public async Task<GetQualificationsQueryResult> Handle(GetQualificationsQuery request, CancellationToken cancellationToken)
        {
            var applicationRequest = new GetApplicationApiRequest(request.CandidateId, request.ApplicationId);
            var application = await candidateApiClient.Get<GetApplicationApiResponse>(applicationRequest);

            var qualificationTypesRequest = new GetQualificationsReferenceDataApiRequest();
            var qualificationTypes = await candidateApiClient.Get<GetQualificationsReferenceDataApiResponse>(qualificationTypesRequest);

            var qualificationsRequest = new GetQualificationsApiRequest(request.ApplicationId, request.CandidateId);
            var qualifications = await candidateApiClient.Get<GetQualificationsApiResponse>(qualificationsRequest);

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
                    Subject = x.Subject
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
}
