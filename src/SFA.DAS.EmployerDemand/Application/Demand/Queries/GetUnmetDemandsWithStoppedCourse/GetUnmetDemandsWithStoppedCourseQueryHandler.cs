using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetUnmetDemandsWithStoppedCourse
{
    public class GetUnmetDemandsWithStoppedCourseQueryHandler : IRequestHandler<GetUnmetDemandsWithStoppedCourseQuery, GetUnmetDemandsWithStoppedCourseResult> 
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly IEmployerDemandApiClient<EmployerDemandApiConfiguration> _employerDemandApiClient;

        public GetUnmetDemandsWithStoppedCourseQueryHandler(
            ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
            IEmployerDemandApiClient<EmployerDemandApiConfiguration> employerDemandApiClient)
        {
            _coursesApiClient = coursesApiClient;
            _employerDemandApiClient = employerDemandApiClient;
        }

        public async Task<GetUnmetDemandsWithStoppedCourseResult> Handle(GetUnmetDemandsWithStoppedCourseQuery request, CancellationToken cancellationToken)
        {
            var stoppedCourses = await _coursesApiClient.Get<GetStandardsListResponse>(new GetStandardsClosedToNewStartsRequest());

            var unmetDemandIdsForAllCourses = new List<Guid>();
            foreach (var standard in stoppedCourses.Standards)
            {
                var unmetDemandsResponse = await _employerDemandApiClient.Get<GetUnmetCourseDemandsResponse>(
                        new GetUnmetEmployerDemandsRequest(0, standard.LarsCode));
                unmetDemandIdsForAllCourses.AddRange(unmetDemandsResponse.EmployerDemandIds);
            }

            return new GetUnmetDemandsWithStoppedCourseResult
            {
                EmployerDemandIds = unmetDemandIdsForAllCourses
            };
        }
    }
}