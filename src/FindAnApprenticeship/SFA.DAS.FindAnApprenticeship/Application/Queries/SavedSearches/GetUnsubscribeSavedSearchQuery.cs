using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SavedSearches;

public class GetUnsubscribeSavedSearchQuery (Guid Id) : IRequest<GetUnsubscribeSavedSearchQueryResult>
{
    public Guid SavedSearchId { get; set; } = Id;
}