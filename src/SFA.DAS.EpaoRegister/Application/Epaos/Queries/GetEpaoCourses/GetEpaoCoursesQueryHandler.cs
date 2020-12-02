using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using SFA.DAS.SharedOuterApi.Validation;

namespace SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpaoCourses
{
    public class GetEpaoCoursesQueryHandler : IRequestHandler<GetEpaoCoursesQuery, GetEpaoCoursesResult>
    {
        private readonly IValidator<GetEpaoCoursesQuery> _validator;
        private readonly IAssessorsApiClient<AssessorsApiConfiguration> _assessorsApiClient;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public GetEpaoCoursesQueryHandler(
            IValidator<GetEpaoCoursesQuery> validator,
            IAssessorsApiClient<AssessorsApiConfiguration> assessorsApiClient,
            ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _validator = validator;
            _assessorsApiClient = assessorsApiClient;
            _coursesApiClient = coursesApiClient;
        }

        public async Task<GetEpaoCoursesResult> Handle(GetEpaoCoursesQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid())
            {
                throw new ValidationException(validationResult.DataAnnotationResult, null, null);
            }

            var apiRequest = new GetEpaoCoursesRequest{EpaoId = request.EpaoId};
            var epaoCoursesListItems = (await _assessorsApiClient.GetAll<GetEpaoCoursesListItem>(apiRequest))?.ToList();

            if (epaoCoursesListItems == null || epaoCoursesListItems.Count == 0)
            {
                throw new NotFoundException<GetEpaoCoursesResult>();
            }
            
            var courseTasks = new List<Task<GetStandardResponse>>();
            foreach (var epaoCoursesListItem in epaoCoursesListItems)
            {
                var getCourseTask = _coursesApiClient.Get<GetStandardResponse>(
                    new GetStandardRequest(epaoCoursesListItem.StandardCode));
                courseTasks.Add(getCourseTask);
            }
            await Task.WhenAll(courseTasks);

            var epaoCourses = new List<GetStandardResponse>();
            foreach (var task in courseTasks)
            {
                var course = task.Result;
                epaoCourses.Add(course);
            }

            return new GetEpaoCoursesResult
            {
                EpaoId = request.EpaoId,
                Courses = epaoCourses
            };
        }
    }
}