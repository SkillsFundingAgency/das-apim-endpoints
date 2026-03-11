using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.Application.ProviderCourseForecasts.Commands.UpsertProviderCourseForecasts;

public class UpsertProviderCourseForecastsCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient) : IRequestHandler<UpsertProviderCourseForecastsCommand>
{
    public async Task Handle(UpsertProviderCourseForecastsCommand request, CancellationToken cancellationToken)
    {
        await _courseManagementApiClient.PostWithResponseCode<object>(new UpsertProviderCourseForecastsRequest(request.Ukprn, request.LarsCode, request.Forecasts), false);
    }
}