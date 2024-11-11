using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses.Shared;

public record SavedSearchDto(
    Guid Id,
    Guid UserReference,
    DateTime DateCreated,
    DateTime? LastRunDate,
    DateTime? EmailLastSendDate,
    SearchParametersDto SearchParameters
);