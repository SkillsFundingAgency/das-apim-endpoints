﻿using MediatR;

namespace SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetProviderCourseQuery
{
    public class GetProviderCourseQuery : IRequest<GetProviderCourseResult>
    {
        public int Ukprn { get; }
        public int LarsCode { get; }
      

        public GetProviderCourseQuery(int ukprn,  int larsCode)
        {
            Ukprn = ukprn;
            LarsCode = larsCode;
        }
    }
}