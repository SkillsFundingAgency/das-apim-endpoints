using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Reservations;

public class DeleteReservationRequest : IDeleteApiRequest
{
    public string DeleteUrl => $"api/reservations/{Id}";
    public Guid Id { get; }

    public DeleteReservationRequest(Guid id)
    {
        Id = id;
    }
}