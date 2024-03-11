using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeCommitments.Requests;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApprenticeApp.Services;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Homepage
{
    public class GetApprenticeDetailsQueryHandler : IRequestHandler<GetApprenticeDetailsQuery, GetApprenticeDetailsQueryResult>
    {
        private readonly CoursesService _coursesService;
        private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _accountsApiClient;
        private readonly IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> _commitmentsApiClient;

        public GetApprenticeDetailsQueryHandler(
            IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> accountsApiClient,
            IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> commitmentsApiClient,
            CoursesService coursesService)
        {
            _coursesService = coursesService;
            _accountsApiClient = accountsApiClient;
            _commitmentsApiClient = commitmentsApiClient;
        }

        public async Task<GetApprenticeDetailsQueryResult> Handle(GetApprenticeDetailsQuery request, CancellationToken cancellationToken)
        {
            var apprenticeTask = _accountsApiClient.Get<Apprentice>(new GetApprenticeRequest(request.ApprenticeId));
            var myApprenticeshipTask = _accountsApiClient.Get<MyApprenticeship>(new GetMyApprenticeshipRequest(request.ApprenticeId));

            await Task.WhenAll(apprenticeTask, myApprenticeshipTask);

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
            }
            else
            {
                var course = await _coursesService.GetStandardCourse(myApprenticeship.StandardUId);
                myApprenticeship.Title = course.Title;
            }
            return myApprenticeship;
        }
    }
}
