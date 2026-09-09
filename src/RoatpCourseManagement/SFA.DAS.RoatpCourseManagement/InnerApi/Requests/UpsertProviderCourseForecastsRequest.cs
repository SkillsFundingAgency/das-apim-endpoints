using System.Collections.Generic;
using SFA.DAS.RoatpCourseManagement.Application.ProviderCourseForecasts.Commands.UpsertProviderCourseForecasts;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

public class UpsertProviderCourseForecastsRequest : IPostApiRequest
{
    public int Ukprn { get; set; }
    public string LarsCode { get; set; }
    public object Data { get; set; }
    public string PostUrl => $"providers/{Ukprn}/courses/{LarsCode}/forecasts";
    public UpsertProviderCourseForecastsRequest(int ukprn, string larsCode, IEnumerable<UpsertProviderCourseForecastModel> Forecasts)
    {
        Ukprn = ukprn;
        LarsCode = larsCode;
        Data = Forecasts;
    }
}
