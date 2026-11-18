using System;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class GetValidateReservationRequest(Guid reservationId, string courseCode, DateTime startDate) : IGetApiRequest
{
    public string GetUrl => $"api/Reservations/validate/{reservationId}?courseCode={courseCode}&startDate={startDate:O}";
}
