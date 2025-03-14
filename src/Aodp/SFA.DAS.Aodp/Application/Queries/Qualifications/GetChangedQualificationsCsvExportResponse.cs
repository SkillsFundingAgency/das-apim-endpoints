namespace SFA.DAS.Aodp.Application.Queries.Qualifications
{
    public class GetChangedQualificationsCsvExportResponse
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public List<ChangedExport>? QualificationExports { get; set; }
    }

    public class ChangedExport
    {
        // Identifiers and Meta Information
        public string? QualificationNumber { get; set; }

        public string? QANText { get; set; }
        public DateTime DateOfDownload { get; set; }

        // Organisation Details
        public string? RecognitionNumber { get; set; }
        public string? OrganisationName { get; set; }
        public string? OrganisationAcronym { get; set; }
        public int? OrganisationReferenceNumber { get; set; }

        // Qualification Details
        public string? Title { get; set; }
        public string? QualificationType { get; set; }
        public string? QualificationLevelCode { get; set; }
        public string? QualSSADescription { get; set; }
        public string? QualSSACode { get; set; }

        // Qualification Versions Details
        public string? LinkToSpecification { get; set; }
        public int? QualCredit { get; set; }
        public string? OverallGradingType { get; set; }
        public string? GradingScale { get; set; }
        public string? EntitlementFrameworkDesignation { get; set; }
        public string? Specialism { get; set; }
        public string? Pathways { get; set; }

        // Region Availability
        public bool? OfferedInEngland { get; set; }
        public int? OfferedInWales { get; set; }                  // Placeholder
        public bool? OfferedInNorthernIreland { get; set; }

        // Age Group Availability
        public bool? PreSixteen { get; set; }
        public bool? SixteenToEighteen { get; set; }
        public bool? EighteenPlus { get; set; }
        public bool? NineteenPlus { get; set; }

        // Funding Details (Placeholders)
        public int? FundingInEngland { get; set; }              // Placeholder
        public int? FundingInWales { get; set; }                // Placeholder
        public int? FundingInNorthernIreland { get; set; }      // Placeholder

        // Qualification Size & Credit Information
        public string? GCSESizeEquivalence { get; set; }
        public string? GCESizeEquivalence { get; set; }
        public int? QualGLH { get; set; }
        public int? QualMinimumGLH { get; set; }
        public int? QualMaximumGLH { get; set; }
        public int? TQT { get; set; }

        // Programme Approval & Discount Codes
        public string? ApprovedForDELFundedProgramme { get; set; }
        public string? NIDiscountCode { get; set; }

        // Qualification Dates
        public DateTime? RegulationStartDate { get; set; }
        public DateTime? OperationalStartDate { get; set; }
        public DateTime? ReviewDate { get; set; }
        public DateTime? OperationalEndDate { get; set; }
        public int? EmbargoDate { get; set; }                // Placeholder
        public DateTime? CertificationEndDate { get; set; }

        // Metadata & Versioning
        public DateTime? UILastUpdatedDate { get; set; }
        public DateTime? InsertedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public int? Version { get; set; }
        public bool? OfferedInternationally { get; set; }
    }
}
