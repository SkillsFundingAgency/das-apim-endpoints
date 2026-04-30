using System.Diagnostics;

#pragma warning disable CS861
namespace SFA.DAS.LearnerData.Responses.EarningsInner;

public class GetFm36DataResponse
{
    public List<Apprenticeship> Apprenticeships { get; set; } = [];
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
    public List<EnglishAndMaths> EnglishAndMaths { get; set; }
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

public class EnglishAndMaths
{
    public string LearnAimRef { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Course { get; set; }
    public List<EnglishAndMathsInstalment> Instalments { get; set; }
}

public class EnglishAndMathsInstalment
{
    public short AcademicYear { get; set; }
    public byte DeliveryPeriod { get; set; }
    public decimal Amount { get; set; }
    public string InstalmentType { get; set; }
}