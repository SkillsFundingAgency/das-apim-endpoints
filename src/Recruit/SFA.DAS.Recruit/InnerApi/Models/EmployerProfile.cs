using System.Collections.Generic;

namespace SFA.DAS.Recruit.InnerApi.Models;

public record EmployerProfile(long AccountLegalEntityId,
    long AccountId,
    string? AboutOrganisation,
    string? TradingName,
    List<EmployerProfileAddress> Addresses);
