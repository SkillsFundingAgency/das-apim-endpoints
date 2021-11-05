using System;

#nullable enable

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class StopRegistrationCommand
    {
        public long CommitmentsApprenticeshipId { get; set; }
        public DateTime CommitmentsStoppedOn { get; set; }
    }
}