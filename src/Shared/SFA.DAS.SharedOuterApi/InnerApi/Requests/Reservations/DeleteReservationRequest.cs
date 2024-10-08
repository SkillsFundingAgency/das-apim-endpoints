using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Reservations;

public class DeleteReservationRequest : IDeleteApiRequest
{
    public string DeleteUrl => $"api/reservations/{Id}";
    public Guid Id { get; }

    public DeleteReservationRequest(Guid id)
    {
        Id = id;
    }
}