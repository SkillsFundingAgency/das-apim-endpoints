using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;
using System.Diagnostics;
using EarningsApprenticeship = SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings.Apprenticeship;
using EarningsEpisode = SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings.Episode;
using Episode = SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning.Episode;

namespace SFA.DAS.LearnerData.Application.Fm36;

// The Models in this file are used to join data from the Learning and Earnings APIs, as well as cached SLD data

/// <summary>
/// This object is the combination of data from the Learning Api, Earnings API and SLD cache for a single learner
/// </summary>
public class JoinedLearnerData
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
    /// <summary>Derived from combining sld data with earnings.PeriodsInLearning</summary>
    public List<JoinedLearningDelivery> LearningDeliveries { get; set; } = new List<JoinedLearningDelivery>();
    /// <summary> Derived from Apprenticeships API, apprenticeship.AgeAtStartOfApprenticeship </summary>
    public int AgeAtStartOfApprenticeship { get; set; }
    /// <summary> Derived from Apprenticeships API, apprenticeship.WithdrawnDate </summary>
    public DateTime? WithdrawnDate { get; set; }
    /// <summary> Derived from earnings.FundingLineType </summary>
    public string FundingLineType { get; set; }
    /// <summary> Derived from Apprenticeships API, apprenticeship.CompletionDate </summary>
    public DateTime? CompletionDate { get; set; }

    internal JoinedLearnerData(Learning learning, EarningsApprenticeship earningsApprenticeship, UpdateLearnerRequest sldLearnerData, short academicYear)
    {
        Key = learning.Key;
        Uln = learning.Uln;
        StartDate = learning.StartDate;
        PlannedEndDate = learning.PlannedEndDate;
        Episodes = JoinEpisodes(learning, earningsApprenticeship, academicYear);
        LearningDeliveries = JoinLearningDeliveries(sldLearnerData, Episodes);
        AgeAtStartOfApprenticeship = learning.AgeAtStartOfApprenticeship;
        WithdrawnDate = learning.WithdrawnDate;
        FundingLineType = earningsApprenticeship.FundingLineType;
        CompletionDate = learning.CompletionDate;
    }

    private static List<JoinedPriceEpisode> JoinEpisodes(Learning learning, EarningsApprenticeship earningsApprenticeship, short academicYear)
    {
        var joinedEpisodes = new List<JoinedPriceEpisode>();

        foreach (var apprenticeshipEpisode in learning.Episodes)
        {
            foreach (var apprenticeshipEpisodePrice in apprenticeshipEpisode.Prices)
            {
                var earningEpisode = earningsApprenticeship.Episodes.SingleOrDefault(x => x.Instalments.Any(y => y.EpisodePriceKey == apprenticeshipEpisodePrice.Key));

                var joinedEpisode = new JoinedPriceEpisode(apprenticeshipEpisode, apprenticeshipEpisodePrice, earningEpisode);

                joinedEpisodes.Add(joinedEpisode);
            }

        }

        return joinedEpisodes.OrderBy(x => x.StartDate).ToList();
    }

    private static List<JoinedLearningDelivery> JoinLearningDeliveries(UpdateLearnerRequest sldLearnerData, List<JoinedPriceEpisode> joinedPriceEpisodes)
    {
        var joinedLearningDeliveries = new List<JoinedLearningDelivery>();

        foreach(var onProgram in sldLearnerData.Delivery.OnProgramme)
        {
            var delivery = new JoinedLearningDelivery(onProgram, joinedPriceEpisodes.SelectMany(x=>x.Instalments));
            joinedLearningDeliveries.Add(delivery);
        }

        return joinedLearningDeliveries;
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

    /// <summary>
    /// Denotes an episode terminated by an "artificial" academic year-end boundary
    /// </summary>
    public bool IsTerminatedByAcademicYearEnd { get; set; }
    public Guid EpisodePriceKey { get; set; }

    /// <summary> Derived from Apprenticeships API, apprenticeship.Episodes.LastDayOfLearning </summary>
    public DateTime? ActualEndDate { get; set; }

    public JoinedPriceEpisode()
    {

    }

    /// <summary>
    /// Constructs a new JoinedPriceEpisode by joining data from an ApprenticeshipEpisode, ApprenticeshipEpisodePrice and EarningsEpisode
    /// </summary>
    /// <param name="apprenticeshipEpisode"></param>
    /// <param name="apprenticeshipEpisodePrice"></param>
    /// <param name="earningsEpisode"></param>
    /// <param name="academicYear"></param>
    public JoinedPriceEpisode(Episode apprenticeshipEpisode, EpisodePrice apprenticeshipEpisodePrice, EarningsEpisode? earningsEpisode)
    {
        EpisodePriceKey = apprenticeshipEpisodePrice.Key;
        TrainingCode = apprenticeshipEpisode.TrainingCode;
        StartDate = apprenticeshipEpisodePrice.StartDate;
        EndDate = apprenticeshipEpisodePrice.EndDate;
        CompletionPayment = earningsEpisode?.CompletionPayment ?? 0;
        OnProgramTotal = earningsEpisode?.OnProgramTotal ?? 0;
        TotalPrice = apprenticeshipEpisodePrice.TotalPrice;
        TrainingPrice = apprenticeshipEpisodePrice.TrainingPrice;
        EndPointAssessmentPrice = apprenticeshipEpisodePrice.EndPointAssessmentPrice;
        FundingBandMaximum = apprenticeshipEpisodePrice.FundingBandMaximum;
        Instalments = GetInstalments(apprenticeshipEpisodePrice, earningsEpisode?.Instalments ?? []);
        AdditionalPayments = GetAdditionalPayments(apprenticeshipEpisodePrice, earningsEpisode?.AdditionalPayments ?? []);
        ActualEndDate = apprenticeshipEpisode.LastDayOfLearning;
    }

    /// <summary>
    /// This constructor creates a new JoinedPriceEpisode based on an existing JoinedPriceEpisode, but sets the StartDate and EndDate and 
    /// only includes Instalments and AdditionalPayments for that academic year (excluding instalments that "overlap" other academic years)
    /// </summary>
    public JoinedPriceEpisode(JoinedPriceEpisode existingEpisode, DateTime newStartDate, DateTime newEndDate, short academicYear, bool isTerminatedByAcademicYearEnd)
    {
        EpisodePriceKey = existingEpisode.EpisodePriceKey;
        StartDate = newStartDate;
        EndDate = newEndDate;
        TrainingCode = existingEpisode.TrainingCode;
        CompletionPayment = existingEpisode.CompletionPayment;
        OnProgramTotal = existingEpisode.OnProgramTotal;
        TotalPrice = existingEpisode.TotalPrice;
        TrainingPrice = existingEpisode.TrainingPrice;
        EndPointAssessmentPrice = existingEpisode.EndPointAssessmentPrice;
        FundingBandMaximum = existingEpisode.FundingBandMaximum;
        Instalments = existingEpisode.Instalments.Where(x => x.AcademicYear == academicYear).ToList();
        AdditionalPayments = existingEpisode.AdditionalPayments.Where(x => x.AcademicYear == academicYear).ToList();
        IsTerminatedByAcademicYearEnd = isTerminatedByAcademicYearEnd;
        ActualEndDate = existingEpisode.ActualEndDate;
    }

    private List<JoinedInstalment> GetInstalments(EpisodePrice apprenticeshipEpisodePrice, List<Instalment> instalments)
    {
        var matchingInstalments = instalments
            .Where(x => x.EpisodePriceKey == apprenticeshipEpisodePrice.Key)
            .Select(x => new JoinedInstalment
            {
                AcademicYear = x.AcademicYear,
                DeliveryPeriod = x.DeliveryPeriod,
                Amount = x.Amount,
                InstalmentType = Enum.Parse<InstalmentType>(x.InstalmentType)
            })
            .OrderBy(x => x.AcademicYear)
            .ThenBy(x => x.DeliveryPeriod)
            .ToList();

        if (matchingInstalments.Any())
        {
            return matchingInstalments;
        }

        return ResolveLegacyInstalments(apprenticeshipEpisodePrice, instalments);
    }

    // This beautiful method can be deleted once all Instalment records in the earnings database have the EpisodePriceKey populated
    private static List<JoinedInstalment> ResolveLegacyInstalments(EpisodePrice apprenticeshipEpisodePrice, List<Instalment> instalments)
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
                x.DueDate >= apprenticeshipEpisodePrice.StartDate &&
                x.DueDate <= apprenticeshipEpisodePrice.EndDate).ToList();
    }
}

public class JoinedLearningDelivery
{
    public int AimSequenceNumber { get; set; }
    public string LearnAimRef { get; set; }
    public List<JoinedInstalment> Instalments { get; set; }

    public JoinedLearningDelivery(OnProgrammeRequestDetails onProgramme, IEnumerable<JoinedInstalment> instalments)
    {
        AimSequenceNumber = onProgramme.AimSequenceNumber;
        LearnAimRef = onProgramme.LearnAimRef;

        Instalments = instalments
            .Where(x => x.AcademicYear.GetDateTime(x.DeliveryPeriod) >= onProgramme.StartDate &&
                        x.AcademicYear.GetDateTime(x.DeliveryPeriod) <= (onProgramme.ActualEndDate ?? onProgramme.ExpectedEndDate))
            .ToList();


    }
}

[DebuggerDisplay("AY: {AcademicYear}, DP: {DeliveryPeriod}, Amount: {Amount}, InstalmentType: {InstalmentType}")]
public class JoinedInstalment
{
    public short AcademicYear { get; set; }
    public byte DeliveryPeriod { get; set; }
    public decimal Amount { get; set; }
    public InstalmentType InstalmentType { get; set; }
}

public enum InstalmentType
{
    Regular = 0,
    Completion = 1,
    Balancing = 2
}

public class JoinedAdditionalPayment
{
    public short AcademicYear { get; set; }
    public byte DeliveryPeriod { get; set; }
    public decimal Amount { get; set; }
    public string AdditionalPaymentType { get; set; }
    public DateTime DueDate { get; set; }
}