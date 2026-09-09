using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Reservations;

public class GetReservationRequest(Guid id) : IGetApiRequest
{
    public Guid Id { get; } = id;
    public string GetUrl => $"api/Reservations/{Id}";
}
