using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.TrackProgress.Apis.CommitmentsV2InnerApi;

public class GetApprenticeshipsRequest : IGetApiRequest
{
    private readonly long _providerId;
    private readonly long _uln;
    private readonly DateTime _startDate;

    private string startDateString
        => _startDate.ToString("yyyy-MM-dd");

    public GetApprenticeshipsRequest(long providerId, long uln, DateTime startDate)
        => (_providerId, _uln, _startDate) = (providerId, uln, startDate);

    public string GetUrl => $"api/apprenticeships?providerid={_providerId}&searchterm={_uln}&startdate={startDateString}";
}