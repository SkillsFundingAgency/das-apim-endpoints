using System;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.SavedSearches;

public class PostUnsubscribeSavedSearchApiRequest
{
    public Guid SavedSearchId { get; set; }
}