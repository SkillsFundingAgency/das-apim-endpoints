using MediatR;
using System;

namespace SFA.DAS.Approvals.Application.CommitmentPayment.Queries.GetDataLockEvents
{
    public class GetDataLockStatuesQuery : IRequest<GetDataLockStatusesQueryResult>
    {
        public long SinceEventId { get; set; }
        public DateTime? SinceTime { get; set; }
        public string EmployerAccountId { get; set; }
        public long Ukprn { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}