using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses;

public record GetSavedSearchesCountApiResponse(Guid UserReference, int SavedSearchesCount);