using System;
using System.Collections.Generic;

namespace SFA.DAS.AparRegister.InnerApi.Responses;

public record GetOrganisationStatusEventsResponse(IEnumerable<OrganisationStatusEvent> Events);
public record OrganisationStatusEvent(long EventId, int Ukprn, OrganisationStatus Status, DateTime CreatedOn);
public enum OrganisationStatus
{
    Removed = 0,
    Active = 1,
    ActiveNoStarts = 2,
    OnBoarding = 3
}
