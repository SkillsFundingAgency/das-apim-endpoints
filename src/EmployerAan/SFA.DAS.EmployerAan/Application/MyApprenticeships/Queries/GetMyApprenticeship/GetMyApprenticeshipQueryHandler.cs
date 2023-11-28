using System.Net;
using MediatR;
using SFA.DAS.EmployerAan.Application.InnerApi.MyApprenticeships;
using SFA.DAS.EmployerAan.Application.InnerApi.Standards.Requests;
using SFA.DAS.EmployerAan.InnerApi.MyApprenticeships;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAan.Application.MyApprenticeships.Queries.GetMyApprenticeship;

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
            await _apprenticeAccountsApiClient.GetWithResponseCode<GetMyApprenticeshipResponse>(new GetMyApprenticeshipRequest(request.ApprenticeId));


        if (response.StatusCode == HttpStatusCode.OK)
        {
            var result = await ConvertResponseToResultWithTrainingCourse(response.Body);
            return result;
        }

        if (response.StatusCode == HttpStatusCode.NotFound) return null;

        throw new InvalidOperationException(
            $"Unexpected response received from apprentice accounts api when getting MyApprenticeship for apprenticeId: {request.ApprenticeId}");
    }

    private async Task<GetMyApprenticeshipQueryResult?> ConvertResponseToResultWithTrainingCourse(GetMyApprenticeshipResponse myApprenticeshipsResponse)
    {
        var result = (GetMyApprenticeshipQueryResult)myApprenticeshipsResponse;

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