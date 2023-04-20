using System.Net;
using MediatR;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Standards.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeAan.Application.MyApprenticeship.Queries.GetMyApprenticeship;

public class GetMyApprenticeshipQueryHandler : IRequestHandler<GetMyApprenticeshipQuery, GetMyApprenticeshipQueryResult?>
{
    private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _apprenticeAccountsApiClient;
    private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

    public GetMyApprenticeshipQueryHandler(IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> apprenticeAccountsApiClient, ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
    {
        _apprenticeAccountsApiClient = apprenticeAccountsApiClient;
        _coursesApiClient = coursesApiClient;
    }

    public async Task<GetMyApprenticeshipQueryResult?> Handle(GetMyApprenticeshipQuery request, CancellationToken cancellationToken)
    {
        var response =
            await _apprenticeAccountsApiClient.GetWithResponseCode<GetMyApprenticeshipsQueryResponse?>(new GetMyApprenticeshipRequest(request.ApprenticeId));

        if (response.StatusCode == HttpStatusCode.OK)
        {
            if (response.Body == null)
            {
                return null;
            }
        }
        else if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
        else
        {
            throw new InvalidOperationException(
                $"Unexpected response received from apprentice accounts api when getting MyApprenticeships details for apprenticeId: {request.ApprenticeId}");
        }

        var result = await ConvertResponseToResult(response.Body);

        return result;
    }

    private async Task<GetMyApprenticeshipQueryResult> ConvertResponseToResult(GetMyApprenticeshipsQueryResponse myApprenticeshipsResponse)
    {
        var result = (GetMyApprenticeshipQueryResult)myApprenticeshipsResponse;

        if (myApprenticeshipsResponse.MyApprenticeships == null ||
            !myApprenticeshipsResponse.MyApprenticeships.Any()) return result;

        var mostRecentMyApprenticeship = myApprenticeshipsResponse.MyApprenticeships.First();
        result.MyApprenticeship = mostRecentMyApprenticeship;

        if (mostRecentMyApprenticeship.StandardUId != null)
        {
            var standard = await _coursesApiClient.Get<GetStandardResponse>(
                new GetStandardQueryRequest(mostRecentMyApprenticeship.StandardUId));

            if (standard != null)
            {
                result.MyApprenticeship.TrainingCourse = standard;
            }
        }
        else if (mostRecentMyApprenticeship.TrainingCode != null)
        {
            var framework = await _coursesApiClient.Get<GetFrameworkResponse>(
                new GetFrameworkQueryRequest(mostRecentMyApprenticeship.TrainingCode));

            if (framework != null)
            {
                result.MyApprenticeship.TrainingCourse = framework;
            }
        }

        return result;
    }
}