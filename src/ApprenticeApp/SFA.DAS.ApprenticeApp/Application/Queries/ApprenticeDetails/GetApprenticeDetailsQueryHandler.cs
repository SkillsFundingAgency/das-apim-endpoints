using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.ApprenticeApp.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.ApprenticeApp.Extensions;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeCommitments.Requests;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Details
{
    public class GetApprenticeDetailsQueryHandler : IRequestHandler<GetApprenticeDetailsQuery, GetApprenticeDetailsQueryResult>
    {
        private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _accountsApiClient;
        private readonly IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> _commitmentsApiClient;
        private readonly CoursesService _coursesService;

        public GetApprenticeDetailsQueryHandler(
            IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> accountsApiClient,
            IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> apprenticeCommitmentsApiClient,
            CoursesService coursesService
            )
        {
            _accountsApiClient = accountsApiClient;
            _commitmentsApiClient = apprenticeCommitmentsApiClient;
            _coursesService = coursesService;            
        }

        public async Task<GetApprenticeDetailsQueryResult> Handle(GetApprenticeDetailsQuery request, CancellationToken cancellationToken)
        {
            var apprenticeTask = await _accountsApiClient.Get<Apprentice>(new GetApprenticeRequest(request.ApprenticeId));
            var apprenticeship = await _commitmentsApiClient.Get<ApprenticeshipsList>(new GetApprenticeApprenticeshipsRequest(request.ApprenticeId));
            var myApprenticeshipTask = await _accountsApiClient.Get<MyApprenticeship>(new GetMyApprenticeshipRequest(request.ApprenticeId));            

            if (myApprenticeshipTask != null)
            {
               await PopulateMyApprenticeshipWithCourseTitle(myApprenticeshipTask);
            }

            return new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = apprenticeTask,
                    Apprenticeship = apprenticeship,
                    MyApprenticeship = myApprenticeshipTask,
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
                myApprenticeship.ApprenticeshipType = course.ApprenticeshipType.GetApprenticeshipType();
            }
            return myApprenticeship;
        }
    }
}
