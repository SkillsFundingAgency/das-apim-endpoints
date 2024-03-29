﻿using System.Net;
using MediatR;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.InnerApi.MyApprenticeships;


namespace SFA.DAS.ApprenticeAan.Application.MyApprenticeships.Queries.GetMyApprenticeship;

public class GetMyApprenticeshipQueryHandler : IRequestHandler<GetMyApprenticeshipQuery, GetMyApprenticeshipQueryResult?>
{
    private readonly IApprenticeAccountsApiClient _apprenticeAccountsApiClient;
    private readonly ICoursesApiClient _coursesApiClient;

    public GetMyApprenticeshipQueryHandler(IApprenticeAccountsApiClient apprenticeAccountsApiClient, ICoursesApiClient coursesApiClient)
    {
        _apprenticeAccountsApiClient = apprenticeAccountsApiClient;
        _coursesApiClient = coursesApiClient;
    }

    public async Task<GetMyApprenticeshipQueryResult?> Handle(GetMyApprenticeshipQuery request, CancellationToken cancellationToken)
    {
        var response = await _apprenticeAccountsApiClient.GetMyApprenticeship(request.ApprenticeId, cancellationToken);

        return response.ResponseMessage.StatusCode switch
        {
            HttpStatusCode.OK => await ConvertResponseToResultWithTrainingCourse(response.GetContent(),
                cancellationToken),
            HttpStatusCode.NotFound => null,
            _ => throw new InvalidOperationException(
                $"Unexpected response received from apprentice accounts api when getting MyApprenticeship for apprenticeId: {request.ApprenticeId}")
        };
    }

    private async Task<GetMyApprenticeshipQueryResult?> ConvertResponseToResultWithTrainingCourse(GetMyApprenticeshipResponse myApprenticeshipsResponse, CancellationToken cancellationToken)
    {
        var result = (GetMyApprenticeshipQueryResult)myApprenticeshipsResponse;

        if (myApprenticeshipsResponse.StandardUId != null)
        {
            var standard =
                await _coursesApiClient.GetStandard(myApprenticeshipsResponse.StandardUId, cancellationToken);

            if (standard.ResponseMessage.IsSuccessStatusCode)
            {
                result.TrainingCourse = standard.GetContent();
            }
        }
        else if (myApprenticeshipsResponse.TrainingCode != null)
        {
            var framework =
                await _coursesApiClient.GetFramework(myApprenticeshipsResponse.TrainingCode, cancellationToken);

            if (framework.ResponseMessage.IsSuccessStatusCode)
            {
                result.TrainingCourse = framework.GetContent();
            }
        }

        return result;
    }
}