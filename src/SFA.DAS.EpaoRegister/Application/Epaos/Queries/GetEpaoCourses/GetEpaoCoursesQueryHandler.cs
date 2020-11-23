using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EpaoRegister.InnerApi.Requests;
using SFA.DAS.EpaoRegister.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpaoCourses
{
    public class GetEpaoCoursesQueryHandler : IRequestHandler<GetEpaoCoursesQuery, GetEpaoCoursesResult>
    {
        private readonly IAssessorsApiClient<AssessorsApiConfiguration> _assessorsApiClient;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public GetEpaoCoursesQueryHandler(
            IAssessorsApiClient<AssessorsApiConfiguration> assessorsApiClient,
            ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _assessorsApiClient = assessorsApiClient;
            _coursesApiClient = coursesApiClient;
        }

        public async Task<GetEpaoCoursesResult> Handle(GetEpaoCoursesQuery request, CancellationToken cancellationToken)
        {
            var apiRequest = new GetEpaoCoursesRequest{EpaoId = request.EpaoId};
            var epaoCoursesListItems = (await _assessorsApiClient.GetAll<GetEpaoCoursesListItem>(apiRequest))?.ToList();

            if (epaoCoursesListItems == null || epaoCoursesListItems.Count == 0)
            {
                throw new EntityNotFoundException<EpaoCourse>();
            }
            
            var courseTasks = new List<Task<GetStandardResponse>>();
            foreach (var epaoCoursesListItem in epaoCoursesListItems)
            {
                var getCourseTask = _coursesApiClient.Get<GetStandardResponse>(
                    new GetStandardRequest(epaoCoursesListItem.StandardCode));
                courseTasks.Add(getCourseTask);
            }
            await Task.WhenAll(courseTasks);

            var epaoCourses = new List<EpaoCourse>();
            for (var i = 0; i < courseTasks.Count; i++)
            {
                var course = courseTasks[i].Result;
                epaoCourses.Add(new EpaoCourse
                {
                    Course = course,
                    GetEpaoCoursesListItem = epaoCoursesListItems[i]
                });
            }

            return new GetEpaoCoursesResult{EpaoCourses = epaoCourses};
        }
    }
}