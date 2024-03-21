using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Qualifications;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications
{
    public class GetQualificationsApiResponse
    {
        public bool? IsSectionCompleted { get; set; }

        public List<Qualification> Qualifications { get; set; }

        public List<QualificationType> QualificationTypes { get; set; }

        public class QualificationType
        {
            public Guid Id { get; set; }
            public required string Name { get; set; }
            public int Order { get; set; }
        }

        public class Qualification
        {
            public Guid Id { get; set; }
            public string? Subject { get; set; }
            public string? Grade { get; set; }
            public string? AdditionalInformation { get; set; }
            public bool? IsPredicted { get; set; }
            public Guid QualificationReference { get; set; }
        }

        public static implicit operator GetQualificationsApiResponse(GetQualificationsQueryResult source)
        {
            return new GetQualificationsApiResponse
            {
                IsSectionCompleted = source.IsSectionCompleted,
                Qualifications = source.Qualifications.Select(x => new Qualification
                {
                    Id = x.Id,
                    AdditionalInformation = x.AdditionalInformation,
                    Grade = x.Grade,
                    IsPredicted = x.IsPredicted,
                    Subject = x.Subject,
                    QualificationReference = x.QualificationReference.Id
                }).ToList(),
                QualificationTypes = source.QualificationTypes.Select(x => new QualificationType
                {
                    Id = x.Id,
                    Name = x.Name,
                    Order = x.Order
                }).ToList()
            };
        }
    }
}
