using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Responses;

public class ValidateReservationResponse
{
    public List<ReservationValidationError> ValidationErrors { get; set; } = [];
}

public class ReservationValidationError
{
    public string PropertyName { get; set; }
    public string Reason { get; set; }
}
