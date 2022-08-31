using MediatR;
using System;

namespace SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Queries
{
    public class ValidateUlnOverlapOnStartDateQuery : IRequest<ValidateUlnOverlapOnStartDateQueryResult>
    {
        public long ProviderId { get; set; }
        public string Uln { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
