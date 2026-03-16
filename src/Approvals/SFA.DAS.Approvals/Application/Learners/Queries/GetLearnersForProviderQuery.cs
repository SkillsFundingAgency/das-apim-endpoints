using MediatR;
using System;
using System.Collections.Generic;

namespace SFA.DAS.Approvals.Application.Learners.Queries
{
    public class GetLearnersForProviderQuery : IRequest<GetLearnersForProviderQueryResult>
    {
        public long ProviderId { get; set; }
        public long? AccountLegalEntityId { get; set; }
        public long? CohortId { get; set; }
        public string SearchTerm { get; set; }
        public string SortField { get; set; }
        public bool SortDescending { get; set; }
        public int Page { get; set; }
        public int? PageSize { get; set; }
        public int? StartMonth { get; set; }
        public int StartYear { get; set; }
        public DateTime? MaxStartDate { get; set; }
        public List<string> ExcludeUlns { get; set; } = new();
        public int? CourseCode { get; set; }
    }
}
