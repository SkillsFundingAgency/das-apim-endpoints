using MediatR;
using System;

namespace SFA.DAS.EmployerFeedback.Application.Queries.GetTrainingProviderSearch
{ 
    public class GetTrainingProviderSearchQuery : IRequest<GetTrainingProviderSearchResult>
    {
        public GetTrainingProviderSearchQuery(long accountId, Guid userRef)
        {
            AccountId = accountId;
            UserRef = userRef;
        }

        public long AccountId { get; }
        public Guid UserRef { get; }
    }
}