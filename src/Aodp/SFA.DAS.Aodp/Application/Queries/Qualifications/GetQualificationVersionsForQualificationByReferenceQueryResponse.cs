namespace SFA.DAS.Aodp.Application.Queries.Qualifications
{
    public class GetQualificationVersionsForQualificationByReferenceQueryResponse
    {
        public Guid Id { get; set; }

        public string QualificationReference { get; set; }

        public string? QualificationName { get; set; }

        public List<QualificationVersions> QualificationVersionsList { get; set; } = new();

        public class QualificationVersions
        {
            public Guid Id { get; set; }

            public Guid QualificationId { get; set; }

            public DateTime LastUpdatedDate { get; set; }

            public DateTime UiLastUpdatedDate { get; set; }

            public DateTime InsertedDate { get; set; }

            public int? Version { get; set; }
        }
    }
}

