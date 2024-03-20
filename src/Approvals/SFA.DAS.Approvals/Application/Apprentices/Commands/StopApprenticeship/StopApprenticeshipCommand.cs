using System;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Application.Apprentices.Commands.StopApprenticeship
{
    public class StopApprenticeshipCommand : IRequest<Unit>
    {
        public long AccountId { get; set; }
        public DateTime StopDate { get; set; }
        public bool MadeRedundant { get; set; }
        public long ApprenticeshipId { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}
