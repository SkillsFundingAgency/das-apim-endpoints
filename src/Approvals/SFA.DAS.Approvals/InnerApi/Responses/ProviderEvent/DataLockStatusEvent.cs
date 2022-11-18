using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.Approvals.InnerApi.Responses.ProviderEvent
{
    public class DataLockStatusEvent
    {
        public long Id { get; set; }
        public DateTime ProcessDateTime { get; set; }
        public EventStatus Status { get; set; }
        public string IlrFileName { get; set; }
        public long Ukprn { get; set; }
        public long Uln { get; set; }
        public string LearnRefNumber { get; set; }
        public long AimSeqNumber { get; set; }
        public string PriceEpisodeIdentifier { get; set; }
        public long ApprenticeshipId { get; set; }
        public long EmployerAccountId { get; set; }
        public EventSource EventSource { get; set; }
        public bool HasErrors { get; set; }
        public DateTime? IlrStartDate { get; set; }
        public long? IlrStandardCode { get; set; }
        public int? IlrProgrammeType { get; set; }
        public int? IlrFrameworkCode { get; set; }
        public int? IlrPathwayCode { get; set; }
        public decimal? IlrTrainingPrice { get; set; }
        public decimal? IlrEndpointAssessorPrice { get; set; }
        public DateTime? IlrPriceEffectiveFromDate { get; set; }
        public DateTime? IlrPriceEffectiveToDate { get; set; }

        public DataLockEventError[] Errors { get; set; }
        public DataLockEventPeriod[] Periods { get; set; }
        public DataLockEventApprenticeship[] Apprenticeships { get; set; }
    }
}