using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AparRegister.InnerApi.Requests;

public record GetProviderStatusEventsRequest(int SinceEventId, int PageSize, int PageNumber) : IGetApiRequest
{
    public string GetUrl => $"organisations/status-events?sinceEventId={SinceEventId}&pageSize={PageSize}&pageNumber={PageNumber}";
}
