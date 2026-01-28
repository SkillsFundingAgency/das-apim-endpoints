using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProviders;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

public class GetProvidersByCourseIdRequest : IGetApiRequest
{
    public required string CourseId { get; init; }
    public ProviderOrderBy? OrderBy { get; set; }
    public decimal? Distance { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }

    public string Location { get; set; }
    public List<DeliveryMode?> DeliveryModes { get; set; }
    public List<ProviderRating?> EmployerProviderRatings { get; set; }
    public List<ProviderRating?> ApprenticeProviderRatings { get; set; }
    public List<QarRating?> Qar { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
    public Guid? UserId { get; set; }

    public string GetUrl => BuildUrl();

    private string BuildUrl()
    {
        var url = $"api/courses/{CourseId}/providers?OrderBy={OrderBy}";

        if (Distance != null)
        {
            url += $"&distance={Distance}";
        }

        if (Latitude != null)
        {
            url += $"&latitude={Latitude}";
        }

        if (Longitude != null)
        {
            url += $"&longitude={Longitude}";
        }

        if (DeliveryModes != null && DeliveryModes.Any())
        {
            url += "&DeliveryModes=" + string.Join("&deliveryModes=", DeliveryModes);
        }

        if (EmployerProviderRatings != null && EmployerProviderRatings.Any())
        {
            url += "&employerProviderRatings=" + string.Join("&employerProviderRatings=", EmployerProviderRatings);
        }

        if (ApprenticeProviderRatings != null && ApprenticeProviderRatings.Any())
        {
            url += "&apprenticeProviderRatings=" + string.Join("&apprenticeProviderRatings=", ApprenticeProviderRatings);
        }

        if (Qar != null && Qar.Any())
        {
            url += "&qar=" + string.Join("&qar=", Qar);
        }

        if (Page != null)
        {
            url += $"&page={Page}";
        }

        if (PageSize != null)
        {
            url += $"&pageSize={PageSize}";
        }

        if (!string.IsNullOrEmpty(Location))
        {
            url += $"&location={Location}";
        }

        if (UserId != null)
        {
            url += $"&userId={UserId}";
        }

        return url;
    }
}