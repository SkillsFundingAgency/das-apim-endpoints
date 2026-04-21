using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Reservations;

public class DeleteReservationRequest(Guid id) : IDeleteApiRequest
{
    public string DeleteUrl => $"api/reservations/{Id}";
    public Guid Id { get; } = id;
}