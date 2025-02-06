using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProviders;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

public class GetProvidersByCourseIdRequest : IGetApiRequest
{

    private readonly int _courseId;
    private readonly ProviderOrderBy? _orderBy;
    private readonly decimal? _distance;
    private readonly decimal? _latitude;
    private readonly decimal? _longitude;
    private readonly List<DeliveryMode?> _deliveryModes;
    private readonly List<ProviderRating?> _employerProviderRatings;
    private readonly List<ProviderRating?> _apprenticeProviderRatings;
    private readonly List<QarRating?> _qar;
    private readonly int? _page;
    private readonly int? _pageSize;

    public GetProvidersByCourseIdRequest(int id,
        ProviderOrderBy? orderBy,
        decimal? distance = null,
        decimal? latitude = null,
        decimal? longitude = null,
        List<DeliveryMode?> deliveryModes = null,
        List<ProviderRating?> employerProviderRatings = null,
        List<ProviderRating?> apprenticeProviderRatings = null,
        List<QarRating?> qar = null,
        int? page = null,
        int? pageSize = null)
    {
        _courseId = id;
        _orderBy = orderBy;
        _distance = distance;
        _deliveryModes = deliveryModes;
        _latitude = latitude;
        _longitude = longitude;
        _employerProviderRatings = employerProviderRatings;
        _apprenticeProviderRatings = apprenticeProviderRatings;
        _qar = qar;
        _page = page;
        _pageSize = pageSize;
    }

    public string GetUrl => BuildUrl();

    private string BuildUrl()
    {
        var url = $"api/courses/{_courseId}/providers?OrderBy={_orderBy}";

        if (_distance != null)
        {
            url += $"&distance={_distance}";
        }

        if (_latitude != null)
        {
            url += $"&latitude={_latitude}";
        }

        if (_longitude != null)
        {
            url += $"&longitude={_longitude}";
        }

        if (_deliveryModes != null && _deliveryModes.Any())
        {
            url += "&DeliveryModes=" + string.Join("&deliveryModes=", _deliveryModes);
        }

        if (_employerProviderRatings != null && _employerProviderRatings.Any())
        {
            url += "&employerProviderRatings=" + string.Join("&employerProviderRatings=", _employerProviderRatings);
        }

        if (_apprenticeProviderRatings != null && _apprenticeProviderRatings.Any())
        {
            url += "&apprenticeProviderRatings=" + string.Join("&apprenticeProviderRatings=", _apprenticeProviderRatings);
        }

        if (_qar != null && _qar.Any())
        {
            url += "&qar=" + string.Join("&qar=", _qar);
        }

        if (_page != null)
        {
            url += $"&page={_page}";
        }

        if (_pageSize != null)
        {
            url += $"&pageSize={_pageSize}";
        }

        return url;
    }
}