using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderEvent
{
    public class SubmissionEvent
    {
        public long Id { get; set; }

        public string IlrFileName { get; set; }

        public DateTime FileDateTime { get; set; }

        public DateTime SubmittedDateTime { get; set; }

        public int ComponentVersionNumber { get; set; }

        public long Ukprn { get; set; }

        public long Uln { get; set; }

        public long? StandardCode { get; set; }

        public int? ProgrammeType { get; set; }

        public int? FrameworkCode { get; set; }

        public int? PathwayCode { get; set; }

        public DateTime? ActualStartDate { get; set; }

        public DateTime? PlannedEndDate { get; set; }

        public DateTime? ActualEndDate { get; set; }

        public decimal? TrainingPrice { get; set; }

        public decimal? EndpointAssessorPrice { get; set; }

        public string NiNumber { get; set; }

        public long? ApprenticeshipId { get; set; }

        public string AcademicYear { get; set; }

        public int? EmployerReferenceNumber { get; set; }

        public string EPAOrgId { get; set; }

        public string GivenNames { get; set; }

        public string FamilyName { get; set; }

        public int? CompStatus { get; set; }
    }
}
