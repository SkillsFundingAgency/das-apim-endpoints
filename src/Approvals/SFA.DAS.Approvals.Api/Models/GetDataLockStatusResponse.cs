using SFA.DAS.Approvals.Api.Models.DataLock;
using SFA.DAS.Approvals.InnerApi.Responses.ProviderEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using DataLockEventError = SFA.DAS.Approvals.InnerApi.Responses.ProviderEvent.DataLockEventError;

namespace SFA.DAS.Approvals.Api.Models
{
    public class GetDataLockStatusResponse
    {
        public long DataLockEventId { get; set; }
        public DateTime DataLockEventDatetime { get; set; }
        public string PriceEpisodeIdentifier { get; set; }
        public long ApprenticeshipId { get; set; }
        public string IlrTrainingCourseCode { get; set; }
        public TrainingType IlrTrainingType { get; set; }
        public DateTime? IlrActualStartDate { get; set; }
        public DateTime? IlrEffectiveFromDate { get; set; }
        public DateTime? IlrPriceEffectiveToDate { get; set; }
        public decimal? IlrTotalCost { get; set; }
        public Status Status { get; set; }
        public TriageStatus TriageStatus { get; set; }
        public DataLockErrorCode ErrorCode { get; set; }
        public long? ApprenticeshipUpdateId { get; set; }
        public bool IsResolved { get; set; }
        public DataLock.EventStatus EventStatus { get; set; }
        public bool IsExpired { get; set; }
        public DateTime? Expired { get; set; }

        public static implicit operator GetDataLockStatusResponse(DataLockStatusEvent dataLockEvent)
        {
            return new GetDataLockStatusResponse
            {
                DataLockEventId = dataLockEvent.Id,
                DataLockEventDatetime = dataLockEvent.ProcessDateTime,
                PriceEpisodeIdentifier = dataLockEvent.PriceEpisodeIdentifier,
                ApprenticeshipId = dataLockEvent.ApprenticeshipId,
                IlrTrainingCourseCode = DeriveTrainingCourseCode(dataLockEvent),
                IlrTrainingType = DeriveTrainingType(dataLockEvent),
                IlrActualStartDate = dataLockEvent.IlrStartDate,
                IlrEffectiveFromDate = dataLockEvent.IlrPriceEffectiveFromDate,
                IlrPriceEffectiveToDate = dataLockEvent.IlrPriceEffectiveToDate,
                IlrTotalCost = dataLockEvent.IlrTrainingPrice + dataLockEvent.IlrEndpointAssessorPrice,
                ErrorCode = DetermineErrorCode(dataLockEvent.Errors),
                Status = dataLockEvent.Errors?.Any() ?? false ? Status.Fail : Status.Pass,
                EventStatus = (DataLock.EventStatus)dataLockEvent.Status
            };
        }

        private static TrainingType DeriveTrainingType(DataLockStatusEvent dataLockEvent)
        {
            return dataLockEvent.IlrProgrammeType == 25
                ? TrainingType.Standard
                : TrainingType.Framework;
        }

        private static string DeriveTrainingCourseCode(DataLockStatusEvent dataLockEvent)
        {
            return dataLockEvent.IlrProgrammeType == 25
                ? $"{dataLockEvent.IlrStandardCode}" :
                $"{dataLockEvent.IlrFrameworkCode}-{dataLockEvent.IlrProgrammeType}-{dataLockEvent.IlrPathwayCode}";
        }

        private static DataLockErrorCode DetermineErrorCode(DataLockEventError[] errors)
        {
            return ListToEnumFlags<DataLockErrorCode>(
                errors?.Select(m => m.ErrorCode)
                    .Select(m => m.Replace("_", ""))
                    .ToList());
        }

        public static T ListToEnumFlags<T>(List<string> enumFlagsAsList) where T : struct
        {
            if (!typeof(T).IsEnum)
                throw new NotSupportedException(typeof(T).Name + " is not an Enum");
            T flags;
            if (enumFlagsAsList == null)
                return default(T);

            enumFlagsAsList.RemoveAll(c => !Enum.TryParse(c, true, out flags));
            var commaSeparatedFlags = string.Join(",", enumFlagsAsList);
            Enum.TryParse(commaSeparatedFlags, true, out flags);
            return flags;
        }
    }
}