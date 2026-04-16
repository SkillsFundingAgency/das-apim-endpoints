using MediatR;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Queries.GetActiveStandards
{
    public class GetActiveStandardsQueryHandler : IRequestHandler<GetActiveStandardsQuery, GetActiveStandardsQueryResult>
    {
        private readonly ICourseService _courseService;

        public GetActiveStandardsQueryHandler(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<GetActiveStandardsQueryResult> Handle(GetActiveStandardsQuery request, CancellationToken cancellationToken)
        {
            var result = await _courseService.GetActiveStandards<GetStandardsListResponse>(string.Empty);
            var courses = result.Standards.Select(courses => new Courses
            {
                StandardUId = courses.StandardUId,
                Title = courses.Title
            }).ToList();

            return new GetActiveStandardsQueryResult
            {
                Courses = courses
            };
        }
    }
}
