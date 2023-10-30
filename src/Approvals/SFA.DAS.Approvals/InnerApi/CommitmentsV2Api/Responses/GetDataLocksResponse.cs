using System.Collections.Generic;
using System;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses
{
    public class GetDataLocksResponse
    {
        public IReadOnlyCollection<DataLock> DataLocks { get; set; }
        public class DataLock
        {
            public long Id { get; set; }
            public DateTime DataLockEventDatetime { get; set; }
            public string PriceEpisodeIdentifier { get; set; }
            public long ApprenticeshipId { get; set; }
            public string IlrTrainingCourseCode { get; set; }
            public DateTime? IlrActualStartDate { get; set; }
            public DateTime? IlrEffectiveFromDate { get; set; }
            public DateTime? IlrPriceEffectiveToDate { get; set; }
            public Decimal? IlrTotalCost { get; set; }
            public short ErrorCode { get; set; }
            public byte DataLockStatus { get; set; }
            public byte TriageStatus { get; set; }
            public bool IsResolved { get; set; }
        }
    }
}
