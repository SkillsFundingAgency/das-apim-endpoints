using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.RoatpCourseManagement.Application.ProviderCourseForecasts.Commands.UpsertProviderCourseForecasts;

public class UpsertProviderCourseForecastsCommand : IRequest
{
    public int Ukprn { get; set; }
    public string LarsCode { get; set; }
    public IEnumerable<UpsertProviderCourseForecastModel> Forecasts { get; set; }
    public UpsertProviderCourseForecastsCommand(int ukprn, string larsCode, IEnumerable<UpsertProviderCourseForecastModel> forecasts)
    {
        Ukprn = ukprn;
        LarsCode = larsCode;
        Forecasts = forecasts;
    }
}
