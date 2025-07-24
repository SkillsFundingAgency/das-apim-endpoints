﻿using System.Diagnostics;
using SFA.DAS.Apprenticeships.Types;
using SFA.DAS.Earnings.Application.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using EarningsApprenticeship = SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings.Apprenticeship;
using EarningsEpisode = SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings.Episode;
using Episode = SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning.Episode;

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

    internal JoinedEarningsApprenticeship(Learning learning, EarningsApprenticeship earningsApprenticeship, short academicYear)
    {
        Key = learning.Key;
        Uln = learning.Uln;
        StartDate = learning.StartDate;
        PlannedEndDate = learning.PlannedEndDate;
        Episodes = JoinEpisodes(learning,earningsApprenticeship, academicYear);
        AgeAtStartOfApprenticeship = learning.AgeAtStartOfApprenticeship;
        WithdrawnDate = learning.WithdrawnDate;
        FundingLineType = earningsApprenticeship.FundingLineType;
    }

    private static List<JoinedPriceEpisode> JoinEpisodes(Learning learning, EarningsApprenticeship earningsApprenticeship, short academicYear)
    {
        var joinedEpisodes = new List<JoinedPriceEpisode>();

        foreach(var apprenticeshipEpisode in learning.Episodes)
        {
            foreach(var apprenticeshipEpisodePrice in apprenticeshipEpisode.Prices)
            {
                var earningEpisode = earningsApprenticeship.Episodes.SingleOrDefault(x => x.Instalments.Any(y=>y.EpisodePriceKey == apprenticeshipEpisodePrice.Key));
                
                if(earningEpisode == null)
                {
                    earningEpisode = ResolveLegacyEpisodes(earningsApprenticeship, apprenticeshipEpisodePrice);
                }

                var joinedEpisode = new JoinedPriceEpisode(apprenticeshipEpisode, apprenticeshipEpisodePrice, earningEpisode);

                joinedEpisodes.Add(joinedEpisode);
            }

        }
        
        return joinedEpisodes.OrderBy(x => x.StartDate).ToList();
    }

    // This beautiful method can be deleted once all Instalment records in the earnings database have the EpisodePriceKey populated
    private static EarningsEpisode? ResolveLegacyEpisodes(EarningsApprenticeship earningsApprenticeship, EpisodePrice episodePrice)
    {
        return earningsApprenticeship.Episodes.SingleOrDefault(x => 
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
            .Select(x=> new JoinedInstalment
            {
                AcademicYear = x.AcademicYear,
                DeliveryPeriod = x.DeliveryPeriod,
                Amount = x.Amount
            })
            .OrderBy(x => x.AcademicYear)
            .ThenBy(x => x.DeliveryPeriod)
            .ToList(); 

        if(matchingInstalments.Any())
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

[DebuggerDisplay("AY: {AcademicYear}, DP: {DeliveryPeriod}, Amount: {Amount}")]
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
}