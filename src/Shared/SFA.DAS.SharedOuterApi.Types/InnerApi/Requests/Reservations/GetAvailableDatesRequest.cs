using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Reservations;

public class GetAvailableDatesRequest(long accountLegalEntityId, string courseId = null) : IGetApiRequest
{
    public long AccountLegalEntityId { get; } = accountLegalEntityId;
    public string CourseId { get; } = courseId;

    public string GetUrl => string.IsNullOrEmpty(CourseId)
        ? $"api/rules/available-dates/{AccountLegalEntityId}"
        : $"api/rules/available-dates/{AccountLegalEntityId}?courseId={Uri.EscapeDataString(CourseId)}";
}