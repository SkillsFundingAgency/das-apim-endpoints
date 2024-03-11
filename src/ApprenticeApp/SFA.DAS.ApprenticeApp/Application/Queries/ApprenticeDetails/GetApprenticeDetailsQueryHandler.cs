using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.ApprenticeApp.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Homepage
{
    public class GetApprenticeDetailsQueryHandler : IRequestHandler<GetApprenticeDetailsQuery, GetApprenticeDetailsQueryResult>
    {
        private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _accountsApiClient;
        private readonly CoursesService _coursesService;

        public GetApprenticeDetailsQueryHandler(
            IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> accountsApiClient,
            CoursesService coursesService
            )
        {
            _coursesService = coursesService;
            _accountsApiClient = accountsApiClient;
        }

        public async Task<GetApprenticeDetailsQueryResult> Handle(GetApprenticeDetailsQuery request, CancellationToken cancellationToken)
        {
            var apprenticeTask = _accountsApiClient.Get<Apprentice>(new GetApprenticeRequest(request.ApprenticeId));
            var myApprenticeshipTask = _accountsApiClient.Get<MyApprenticeship>(new GetMyApprenticeshipRequest(request.ApprenticeId));

            await Task.WhenAll(apprenticeTask, myApprenticeshipTask);

            var myApprenticeship = await myApprenticeshipTask;
            if (myApprenticeship != null)
            {
               await PopulateMyApprenticeshipWithCourseTitle(myApprenticeship);
            }

            return new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = await apprenticeTask,
                    MyApprenticeship = await myApprenticeshipTask
                }
            };
        }

        private async Task<MyApprenticeship> PopulateMyApprenticeshipWithCourseTitle(MyApprenticeship myApprenticeship)
        {
            if (string.IsNullOrWhiteSpace(myApprenticeship.StandardUId))
            {
                var course = await _coursesService.GetFrameworkCourse(myApprenticeship.TrainingCode);
                myApprenticeship.Title = course.Title;
                myApprenticeship.Level = course.Level;
            }
            else
            {
                var course = await _coursesService.GetStandardCourse(myApprenticeship.StandardUId);
                myApprenticeship.Title = course.Title;
                myApprenticeship.Level = course.Level;
            }
            return myApprenticeship;
        }
    }
}
