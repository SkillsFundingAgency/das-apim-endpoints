using System;

#nullable enable

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class StopApprovalCommand
    {
        public long CommitmentsApprenticeshipId { get; set; }
        public DateTime CommitmentsStoppedOn { get; set; }
    }
}