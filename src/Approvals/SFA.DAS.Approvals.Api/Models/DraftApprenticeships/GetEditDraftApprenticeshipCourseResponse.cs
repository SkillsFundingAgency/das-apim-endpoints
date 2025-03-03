﻿using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeshipCourse;

namespace SFA.DAS.Approvals.Api.Models.DraftApprenticeships
{
    public class GetEditDraftApprenticeshipCourseResponse
    {
        public string ProviderName { get; set; }
        public string EmployerName { get; set; }
        public bool IsMainProvider { get; set; }
        public IEnumerable<Standard> Standards { get; set; }


        public class Standard
        {
            public string CourseCode { get; set; }
            public string Name { get; set; }
        }

        public static implicit operator GetEditDraftApprenticeshipCourseResponse(GetEditDraftApprenticeshipCourseQueryResult source)
        {
            return new GetEditDraftApprenticeshipCourseResponse
            {
                ProviderName = source.ProviderName,
                EmployerName = source.EmployerName,
                IsMainProvider = source.IsMainProvider,
                Standards = source.Standards.Select(x => new Standard { CourseCode = x.CourseCode, Name = x.Name}).ToList()
            };
        }
    }
}
