using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SavedSearches;

public class GetUnsubscribeSavedSearchQuery : IRequest<GetUnsubscribeSavedSearchQueryResult>
{
    public Guid SavedSearchId { get; set; }
}