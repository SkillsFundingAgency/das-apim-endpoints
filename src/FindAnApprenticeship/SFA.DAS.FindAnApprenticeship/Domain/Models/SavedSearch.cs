using System;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Domain.Models;

public record SavedSearch(
    Guid Id,
    Guid UserReference,
    DateTime DateCreated,
    DateTime? LastRunDate,
    DateTime? EmailLastSendDate,
    SearchParameters SearchParameters
);
