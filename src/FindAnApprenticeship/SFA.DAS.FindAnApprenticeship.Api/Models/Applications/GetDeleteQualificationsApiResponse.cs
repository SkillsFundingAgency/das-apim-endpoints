using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetDeleteQualifications;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications
{
    public class GetDeleteQualificationsApiResponse
    {
        public Guid QualificationReference { get; set; }

        public List<Qualification> Qualifications { get; set; }

        public class Qualification
        {
            public string? Subject { get; set; }
            public string? Grade { get; set; }
            public string? AdditionalInformation { get; set; }
            public bool? IsPredicted { get; set; }
        }

        public static implicit operator GetDeleteQualificationsApiResponse(GetDeleteQualificationsQueryResult source)
        {
            return new GetDeleteQualificationsApiResponse
            {
                QualificationReference = source.QualificationReference,
                Qualifications = source.Qualifications.Select(x => new Qualification
                {
                    AdditionalInformation = x.AdditionalInformation,
                    Grade = x.Grade,
                    IsPredicted = x.IsPredicted,
                    Subject = x.Subject
                }).ToList()
            };
        }
    }
}
