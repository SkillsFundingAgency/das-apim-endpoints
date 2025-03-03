using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using System.Collections.Generic;
using System;
using System.Linq;

namespace SFA.DAS.Approvals.Extensions
{
    public static class TrainingProgrammeFundingPeriodExtensions
    {
        public static int? GetFundingBandForDate(this List<TrainingProgrammeFundingPeriod> bands, DateTime? forDate)
        {
            forDate ??= DateTime.Today;
            var match = bands.FirstOrDefault(x =>
                x.EffectiveFrom <= forDate && (x.EffectiveTo ?? DateTime.Today.AddYears(5)) >= forDate);
            return match?.FundingCap;
        }
    }
}
