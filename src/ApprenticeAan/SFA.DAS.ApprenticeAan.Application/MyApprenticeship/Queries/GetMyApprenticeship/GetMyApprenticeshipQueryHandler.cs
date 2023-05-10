using System.Net;
using MediatR;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Standards.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeAan.Application.MyApprenticeship.Queries.GetMyApprenticeship;

public class GetMyApprenticeshipQueryHandler : IRequestHandler<GetMyApprenticeshipQuery, GetMyApprenticeshipQueryResult>
{
    private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _apprenticeAccountsApiClient;
    private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

    public GetMyApprenticeshipQueryHandler(IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> apprenticeAccountsApiClient, ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
    {
        _apprenticeAccountsApiClient = apprenticeAccountsApiClient;
        _coursesApiClient = coursesApiClient;
    }

    public async Task<GetMyApprenticeshipQueryResult> Handle(GetMyApprenticeshipQuery request, CancellationToken cancellationToken)
    {
        var response =
            await _apprenticeAccountsApiClient.GetWithResponseCode<MyApprenticeshipResponse>(new GetMyApprenticeshipRequest(request.ApprenticeId));

        if (response.StatusCode!=HttpStatusCode.OK)
        {
            return new GetMyApprenticeshipQueryResult
            {
                StatusCode = response.StatusCode
            };
        }

        var result = new GetMyApprenticeshipQueryResult
        {
            MyApprenticeship = await ConvertResponseToResult(response.Body),
            StatusCode = response.StatusCode
        };

        return result;
    }

    private async Task<MyApprenticeship?> ConvertResponseToResult(MyApprenticeshipResponse myApprenticeshipsResponse)
    {
        var result = (MyApprenticeship)myApprenticeshipsResponse;

      
          if (myApprenticeshipsResponse.StandardUId != null)
        {
            var standard = await _coursesApiClient.Get<GetStandardResponse>(
                new GetStandardQueryRequest(myApprenticeshipsResponse.StandardUId));

            if (standard != null)
            {
                result.TrainingCourse = standard;
            }
        }
        else if (myApprenticeshipsResponse.TrainingCode != null)
        {
            var framework = await _coursesApiClient.Get<GetFrameworkResponse>(
                new GetFrameworkQueryRequest(myApprenticeshipsResponse.TrainingCode));

            if (framework != null)
            {
                result.TrainingCourse = framework;
            }
        }

        return result;
    }
}