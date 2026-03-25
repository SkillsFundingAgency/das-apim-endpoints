using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Reservations;

public class GetReservationRequest(Guid id) : IGetApiRequest
{
    public Guid Id { get; } = id;
    public string GetUrl => $"api/Reservations/{Id}";
}
