using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

public class GetProvidersByCourseIdRequest : IGetApiRequest
{
    private readonly double? _latitude;
    private readonly double? _longitude;
    private readonly int _courseId;

    public GetProvidersByCourseIdRequest(int id, double? latitude = null, double? longitude = null)
    {
        _latitude = latitude;
        _longitude = longitude;
        _courseId = id;
    }

    public string GetUrl => $"api/courses/{_courseId}/providers?lat={_latitude}&lon={_longitude}";
}