using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AparRegister.InnerApi.Requests;

public record GetOrganisationStatusEventsRequest(int SinceEventId, int PageSize, int PageNumber) : IGetApiRequest
{
    public string GetUrl => $"api/organisation-status-events?sinceEventId={SinceEventId}&pageSize={PageSize}&pageNumber={PageNumber}";
}
