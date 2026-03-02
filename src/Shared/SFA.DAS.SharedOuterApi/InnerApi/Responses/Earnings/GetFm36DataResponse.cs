using System;
using System.Collections.Generic;
using System.Diagnostics;

#pragma warning disable CS861
namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings
{
    public class GetFm36DataResponse
    {
        public List<Apprenticeship> Apprenticeships { get; set; }
    }

    public class Apprenticeship
    {
        public long Ukprn { get; set; }
        public Guid Key { get; set; } = Guid.Empty;
        public List<Episode> Episodes { get; set; }
        public string FundingLineType { get; set; }
    }

    public class Episode
    {
        public Guid Key { get; set; }
        public int NumberOfInstalments { get; set; }
        public List<Instalment> Instalments { get; set; }
        public List<AdditionalPayment> AdditionalPayments { get; set; }
        public decimal CompletionPayment { get; set; }
        public decimal OnProgramTotal { get; set; }
    }
    
    [DebuggerDisplay("AY {AcademicYear} DP {DeliveryPeriod} Amount: {Amount} EpisodePriceKey: {EpisodePriceKey}")]

    public class Instalment
    {
        public short AcademicYear { get; set; }
        public byte DeliveryPeriod { get; set; }
        public decimal Amount { get; set; }
        public Guid EpisodePriceKey { get; set; }
        public Guid PeriodInLearningKey { get; set; }
        public string InstalmentType { get; set; }
    }

    public class AdditionalPayment
    {
        public short AcademicYear { get; set; }
        public byte DeliveryPeriod { get; set; }
        public decimal Amount { get; set; }
        public string AdditionalPaymentType { get; set; }
        public DateTime DueDate { get; set; }
    }
}
