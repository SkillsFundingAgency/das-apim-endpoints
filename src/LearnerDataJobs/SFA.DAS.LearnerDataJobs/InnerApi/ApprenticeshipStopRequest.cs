namespace SFA.DAS.LearnerDataJobs.InnerApi
{
    public record ApprenticeshipStopRequest
    {
        public long ApprenticeshipId { get; set; }

        public bool IsWithDrawnAtStartOfCourse { get; set; }

        public long? LearnerDataId { get; set; }
    }
}
