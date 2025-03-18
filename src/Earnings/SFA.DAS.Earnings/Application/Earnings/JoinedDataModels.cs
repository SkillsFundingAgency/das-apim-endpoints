using SFA.DAS.Earnings.Application.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using Apprenticeship = SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships.Apprenticeship;
using EarningsApprenticeship = SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings.Apprenticeship;
using EarningsEpisode = SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings.Episode;
using Episode = SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships.Episode;

namespace SFA.DAS.Earnings.Application.Earnings;

// The Models in this file are used to join data from the Apprenticeships and Earnings APIs
public class JoinedEarningsApprenticeship
{
    /// <summary> Derived from Apprenticeships API, apprenticeship.Key </summary>
    public Guid Key { get; set; }
    /// <summary> Derived from Apprenticeships API, apprenticeship.Uln </summary>
    public string Uln { get; set; }
    /// <summary> Derived from Apprenticeships API, apprenticeship.StartDate </summary>
    public DateTime StartDate { get; set; }
    /// <summary> Derived from Apprenticeships API, apprenticeship.PlannedEndDate </summary>
    public DateTime PlannedEndDate { get; set; }
    /// <summary> Derived from combining earnings.Episodes and apprenticeship.Episodes</summary>
    public List<JoinedPriceEpisode> Episodes { get; set; }
    /// <summary> Derived from Apprenticeships API, apprenticeship.AgeAtStartOfApprenticeship </summary>
    public int AgeAtStartOfApprenticeship { get; set; }
    /// <summary> Derived from Apprenticeships API, apprenticeship.WithdrawnDate </summary>
    public DateTime? WithdrawnDate { get; set; }
    /// <summary> Derived from earnings.FundingLineType </summary>
    public string FundingLineType { get; set; }

    internal JoinedEarningsApprenticeship(Apprenticeship apprenticeship, EarningsApprenticeship earningsApprenticeship)
    {
        Key = apprenticeship.Key;
        Uln = apprenticeship.Uln;
        StartDate = apprenticeship.StartDate;
        PlannedEndDate = apprenticeship.PlannedEndDate;
        Episodes = JoinEpisodes(apprenticeship,earningsApprenticeship);
        AgeAtStartOfApprenticeship = apprenticeship.AgeAtStartOfApprenticeship;
        WithdrawnDate = apprenticeship.WithdrawnDate;
        FundingLineType = earningsApprenticeship.FundingLineType;
    }

    private static List<JoinedPriceEpisode> JoinEpisodes(Apprenticeship apprenticeship, EarningsApprenticeship earningsApprenticeship)
    {
        var joinedEpisodes = new List<JoinedPriceEpisode>();

        foreach(var apprenticeshipEpisode in apprenticeship.Episodes)
        {
            foreach(var apprenticeshipEpisodePrice in apprenticeshipEpisode.Prices)
            {
                var earningEpisode = earningsApprenticeship.Episodes.SingleOrDefault(x => x.Instalments.Any(y=>y.EpisodePriceKey == apprenticeshipEpisodePrice.Key));
                
                if(earningEpisode == null)
                {
                    earningEpisode = ReallyNastyMethodThatShouldNotExistButIsRequiredAsExistingEarningsDataDoesNotRecordEpisodePriceKey(earningsApprenticeship, apprenticeshipEpisodePrice);
                }

                var joinedEpisode = new JoinedPriceEpisode(apprenticeshipEpisode, apprenticeshipEpisodePrice, earningEpisode);

                joinedEpisodes.Add(joinedEpisode);
            }

        }

        return joinedEpisodes;
    }

    // This beautiful method can be deleted once all Instalment records in the earnings database have the EpisodePriceKey populated
    private static EarningsEpisode ReallyNastyMethodThatShouldNotExistButIsRequiredAsExistingEarningsDataDoesNotRecordEpisodePriceKey(EarningsApprenticeship earningsApprenticeship, EpisodePrice episodePrice)
    {
        return earningsApprenticeship.Episodes.Single(x => 
            x.Instalments.Any(y => 
                y.AcademicYear.GetDateTime(y.DeliveryPeriod) >= episodePrice.StartDate &&
                y.AcademicYear.GetDateTime(y.DeliveryPeriod) <= episodePrice.EndDate));
    }
}

public class JoinedPriceEpisode
{
    /// <summary> Derived from Apprenticeships API, apprenticeship.Episode.EpisodePrice.TrainingCode </summary>
    public string TrainingCode { get; set; }

    /// <summary> Derived from Apprenticeships API, apprenticeship.Episode.EpisodePrice.StartDate </summary>
    public DateTime StartDate { get; set; }

    /// <summary> Derived from Apprenticeships API, apprenticeship.Episode.EpisodePrice.EndDate </summary>
    public DateTime EndDate { get; set; }

    /// <summary> Derived from combining earnings.Instalments and apprenticeship.Episode.EpisodePrice</summary>
    public List<JoinedInstalment> Instalments { get; set; }

    /// <summary> Derived from combining earnings.AdditionalPayments and apprenticeship.Episode.EpisodePrice</summary>
    public List<JoinedAdditionalPayment> AdditionalPayments { get; set; }

    /// <summary> Derived from earnings.CompletionPayment </summary>
    public decimal CompletionPayment { get; set; }

    /// <summary> Derived from earnings.OnProgramTotal </summary>
    public decimal OnProgramTotal { get; set; }

    /// <summary> Derived from apprenticeship.Episode.EpisodePrice.TotalPrice </summary>
    public decimal TotalPrice { get; set; }

    /// <summary> Derived from apprenticeship.Episode.EpisodePrice.TrainingPrice </summary>
    public decimal? TrainingPrice { get; set; }

    /// <summary> Derived from apprenticeship.Episode.EpisodePrice.EndPointAssessmentPrice </summary>
    public decimal? EndPointAssessmentPrice { get; set; }

    /// <summary> Derived from apprenticeship.Episode.EpisodePrice.FundingBandMaximum </summary>
    public int FundingBandMaximum { get; set; }

    public JoinedPriceEpisode()
    {
        
    }

    /// <summary>
    /// Constructs a new JoinedPriceEpisode by joining data from an ApprenticeshipEpisode, ApprenticeshipEpisodePrice and EarningsEpisode
    /// </summary>
    /// <param name="apprenticeshipEpisode"></param>
    /// <param name="apprenticeshipEpisodePrice"></param>
    /// <param name="earningsEpisode"></param>
    public JoinedPriceEpisode(Episode apprenticeshipEpisode, EpisodePrice apprenticeshipEpisodePrice, EarningsEpisode earningsEpisode)
    {
        TrainingCode = apprenticeshipEpisode.TrainingCode;
        StartDate = apprenticeshipEpisodePrice.StartDate;
        EndDate = apprenticeshipEpisodePrice.EndDate;
        CompletionPayment = earningsEpisode.CompletionPayment;
        OnProgramTotal = earningsEpisode.OnProgramTotal;
        TotalPrice = apprenticeshipEpisodePrice.TotalPrice;
        TrainingPrice = apprenticeshipEpisodePrice.TrainingPrice;
        EndPointAssessmentPrice = apprenticeshipEpisodePrice.EndPointAssessmentPrice;
        FundingBandMaximum = apprenticeshipEpisodePrice.FundingBandMaximum;
        Instalments = GetInstalments(apprenticeshipEpisodePrice, earningsEpisode.Instalments);
        AdditionalPayments = GetAdditionalPayments(apprenticeshipEpisodePrice, earningsEpisode.AdditionalPayments);
    }

    /// <summary>
    /// This constructor creates a new JoinedPriceEpisode based on an existing JoinedPriceEpisode, but sets the StartDate and EndDate and 
    /// only includes Instalments and AdditionalPayments that fall within the new StartDate and EndDate
    /// </summary>
    public JoinedPriceEpisode(JoinedPriceEpisode existingEpisode, DateTime newStartDate, DateTime newEndDate)
    {
        StartDate = newStartDate;
        EndDate = existingEpisode.EndDate;
        TrainingCode = existingEpisode.TrainingCode;
        CompletionPayment = existingEpisode.CompletionPayment;
        OnProgramTotal = existingEpisode.OnProgramTotal;
        TotalPrice = existingEpisode.TotalPrice;
        TrainingPrice = existingEpisode.TrainingPrice;
        EndPointAssessmentPrice = existingEpisode.EndPointAssessmentPrice;
        FundingBandMaximum = existingEpisode.FundingBandMaximum;
        Instalments = existingEpisode.Instalments.Where(x =>
            x.AcademicYear.GetDateTime(x.DeliveryPeriod) >= newStartDate &&
            x.AcademicYear.GetDateTime(x.DeliveryPeriod) <= newEndDate).ToList();
        AdditionalPayments = existingEpisode.AdditionalPayments.Where(x =>
            x.AcademicYear.GetDateTime(x.DeliveryPeriod) >= newStartDate &&
            x.AcademicYear.GetDateTime(x.DeliveryPeriod) <= newEndDate).ToList();
    }

    private List<JoinedInstalment> GetInstalments(EpisodePrice apprenticeshipEpisodePrice, List<Instalment> instalments)
    {
        var matchingInstalments = instalments
            .Where(x => x.EpisodePriceKey == apprenticeshipEpisodePrice.Key)
            .Select(x=> new JoinedInstalment
            {
                AcademicYear = x.AcademicYear,
                DeliveryPeriod = x.DeliveryPeriod,
                Amount = x.Amount
            }).ToList(); 

        if(matchingInstalments.Any())
        {
            return matchingInstalments;
        }

        return ReallyHorribleMethodToGetInstalmentsIfEpisodePriceKeyMissing(apprenticeshipEpisodePrice, instalments);
    }

    // This beautiful method can be deleted once all Instalment records in the earnings database have the EpisodePriceKey populated
    private static List<JoinedInstalment> ReallyHorribleMethodToGetInstalmentsIfEpisodePriceKeyMissing(EpisodePrice apprenticeshipEpisodePrice, List<Instalment> instalments)
    {
        return instalments.Where(y =>
                y.AcademicYear.GetDateTime(y.DeliveryPeriod) >= apprenticeshipEpisodePrice.StartDate &&
                y.AcademicYear.GetDateTime(y.DeliveryPeriod) <= apprenticeshipEpisodePrice.EndDate)
            .Select(x => new JoinedInstalment
            {
                AcademicYear = x.AcademicYear,
                DeliveryPeriod = x.DeliveryPeriod,
                Amount = x.Amount
            }).ToList();
    }

    private List<JoinedAdditionalPayment> GetAdditionalPayments(EpisodePrice apprenticeshipEpisodePrice, List<AdditionalPayment> additionalPayments)
    {
        var allAdditionalPayments = additionalPayments.Select(x => new JoinedAdditionalPayment
        {
            AcademicYear = x.AcademicYear,
            DeliveryPeriod = x.DeliveryPeriod,
            Amount = x.Amount,
            AdditionalPaymentType = x.AdditionalPaymentType,
            DueDate = x.DueDate
        }).ToList();

        return allAdditionalPayments.Where(x => 
                x.DateTime >= apprenticeshipEpisodePrice.StartDate && 
                x.DateTime <= apprenticeshipEpisodePrice.EndDate).ToList();
    }
}

public class JoinedInstalment
{
    public short AcademicYear { get; set; }
    public byte DeliveryPeriod { get; set; }
    public decimal Amount { get; set; }
}

public class JoinedAdditionalPayment
{
    public short AcademicYear { get; set; }
    public byte DeliveryPeriod { get; set; }
    public decimal Amount { get; set; }
    public string AdditionalPaymentType { get; set; }
    public DateTime DueDate { get; set; }

    public DateTime DateTime => GetDateTime();

    private DateTime GetDateTime()
    {
        var calendarYear = ToCalendarYear(AcademicYear, DeliveryPeriod);
        var calendarMonth = ToCalendarMonth(DeliveryPeriod);
        return new DateTime(calendarYear, calendarMonth, 1);
    }

    private short ToCalendarYear(short academicYear, byte deliveryPeriod)
    {
        if (deliveryPeriod >= 6)
            return short.Parse($"20{academicYear.ToString().Substring(2, 2)}");
        else
            return short.Parse($"20{academicYear.ToString().Substring(0, 2)}");
    }

    private byte ToCalendarMonth(byte deliveryPeriod)
    {
        if (deliveryPeriod >= 6)
            return (byte)(deliveryPeriod - 5);
        else
            return (byte)(deliveryPeriod + 7);
    }
}