using System.Collections.Generic;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Api.Models;

public class SelectMultipleValidateApimRequest
{
    public long ProviderId { get; set; }
    public long AccountId { get; set; }
    public bool IsLevyEmployer { get; set; }
    public IEnumerable<long> LearnerIds { get; set; }
    public long? AccountLegalEntityId { get; set; }
    public string AgreementId { get; set; }
    public IEnumerable<SelectMultipleReservationAssignment> ReservationAssignments { get; set; }
    public UserInfo UserInfo { get; set; }
}
