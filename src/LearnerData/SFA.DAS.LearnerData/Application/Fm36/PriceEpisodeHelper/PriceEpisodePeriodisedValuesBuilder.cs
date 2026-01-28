using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using SFA.DAS.LearnerData.Application.Fm36.Common;

namespace SFA.DAS.LearnerData.Application.Fm36.PriceEpisodeHelper;

public static class PriceEpisodePeriodisedValuesBuilder
{
    public static void AddWithSamePeriodisedValues(
        this List<PriceEpisodePeriodisedValues> list,
        string attributeName,
        decimal value)
    {
        list.Add(BuildWithSameValues(attributeName, value));
    }

    private static PriceEpisodePeriodisedValues BuildWithSameValues(string attributeName, decimal value)
    {
        return new PriceEpisodePeriodisedValues
        {
            AttributeName = attributeName,
            Period1 = value,
            Period2 = value,
            Period3 = value,
            Period4 = value,
            Period5 = value,
            Period6 = value,
            Period7 = value,
            Period8 = value,
            Period9 = value,
            Period10 = value,
            Period11 = value,
            Period12 = value
        };
    }

    public static void AddPriceEpisodeInstalmentsThisPeriodValues(
        this List<PriceEpisodePeriodisedValues> list,
        JoinedPriceEpisode joinedPriceEpisode,
        short academicYear,
        InstalmentType instalmentType = InstalmentType.Regular)
    {
        try
        {
            list.Add(BuildPriceEpisodeInstalmentsThisPeriodValues(joinedPriceEpisode, instalmentType, academicYear));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to Add PriceEpisodeInstalmentsThisPeriodValues {EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeInstalmentsThisPeriod} to PriceEpisodePeriodisedValues list.",
                ex);
        }
    }

    private static PriceEpisodePeriodisedValues BuildPriceEpisodeInstalmentsThisPeriodValues(JoinedPriceEpisode joinedPriceEpisode, InstalmentType instalmentType, short academicYear)
    {
        var instalments = GetInstalmentsForAcademicYear(joinedPriceEpisode, instalmentType, academicYear);

        return BuildPriceEpisodePeriodisedValuesFromFunc(
            EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeInstalmentsThisPeriod,
            period => (instalments.SingleOrDefault(i => i.DeliveryPeriod == period)?.Amount).GetValueOrDefault() != 0 ? 1 : 0
        );
    }

    public static void AddAdditionalPaymentPerPeriodValues(
        this List<PriceEpisodePeriodisedValues> list,
        string attributeName,
        JoinedPriceEpisode joinedPriceEpisode,
        short academicYear,
        string additionalPaymentType)
    {
        try
        {
            list.Add(BuildAdditionalPaymentPerPeriodValues(joinedPriceEpisode, academicYear, attributeName, additionalPaymentType));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to Add AdditionalPaymentPerPeriodValues {attributeName} to PriceEpisodePeriodisedValues list.",
                ex);
        }
    }

    private static PriceEpisodePeriodisedValues BuildAdditionalPaymentPerPeriodValues(JoinedPriceEpisode joinedPriceEpisode, short academicYear, string attributeName, string additionalPaymentType)
    {
        var additionalPayments = GetAdditionalPayments(joinedPriceEpisode, additionalPaymentType);

        return BuildPriceEpisodePeriodisedValuesFromFunc(
            attributeName,
            period => additionalPayments.SingleOrDefault(x => x.AcademicYear == academicYear && x.DeliveryPeriod == period)?.Amount ?? 0
        );

    }

    public static void AddInstallmentAmountValues(
        this List<PriceEpisodePeriodisedValues> list,
        string attributeName,
        JoinedPriceEpisode joinedPriceEpisode,
        short academicYear,
        InstalmentType instalmentType = InstalmentType.Regular)
    {
        try
        {
            list.Add(BuildInstallmentAmountValues(joinedPriceEpisode, instalmentType, academicYear, attributeName));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to Add InstallmentAmountValues {attributeName} to PriceEpisodePeriodisedValues list.",
                ex);
        }
    }

    private static PriceEpisodePeriodisedValues BuildInstallmentAmountValues(JoinedPriceEpisode joinedPriceEpisode, InstalmentType instalmentType, short academicYear, string attributeName)
    {
        return BuildCoInvestmentValues(joinedPriceEpisode, instalmentType, academicYear, attributeName, 1);
    }

    public static void AddCoInvestmentValues(
        this List<PriceEpisodePeriodisedValues> list,
        string attributeName,
        JoinedPriceEpisode joinedPriceEpisode,
        short academicYear,
        decimal multiplier,
        InstalmentType instalmentType = InstalmentType.Regular)
    {
        try
        {
            list.Add(BuildCoInvestmentValues(joinedPriceEpisode, instalmentType, academicYear, attributeName, multiplier));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to Add CoInvestmentValues {attributeName} to PriceEpisodePeriodisedValues list.",
                ex);
        }
    }

    private static PriceEpisodePeriodisedValues BuildCoInvestmentValues(JoinedPriceEpisode joinedPriceEpisode, InstalmentType instalmentType, short academicYear, string attributeName, decimal multiplier)
    {
        var instalments = GetInstalmentsForAcademicYear(joinedPriceEpisode, instalmentType, academicYear);

        return BuildPriceEpisodePeriodisedValuesFromFunc(
            attributeName,
            (period) => (instalments.SingleOrDefault(i => i.DeliveryPeriod == period)?.Amount * multiplier).GetValueOrDefault()
        );
    }

    public static void AddNthIncentivePaymentValues(
        this List<PriceEpisodePeriodisedValues> list,
        JoinedLearnerData joinedLearnerData,
        JoinedPriceEpisode joinedPriceEpisode,
        short academicYear,
        string attributeName,
        string additionalPaymentType,
        int n)
    {
        try
        {
            list.Add(BuildNthIncentivePaymentValues(joinedLearnerData, joinedPriceEpisode, academicYear, attributeName, additionalPaymentType, n));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to Add NthIncentivePaymentValues {attributeName} to PriceEpisodePeriodisedValues list.",
                ex);
        }
    }

    private static PriceEpisodePeriodisedValues BuildNthIncentivePaymentValues(JoinedLearnerData joinedLearnerData, JoinedPriceEpisode joinedPriceEpisode, short academicYear, string attributeName, string additionalPaymentType, int n)
    {
        var allAdditionalPayments = joinedLearnerData.Episodes.SelectMany(x =>
            x.AdditionalPayments.Where(y => y.AdditionalPaymentType == additionalPaymentType))
            .OrderBy(i => i.AcademicYear)
            .ThenBy(i => i.DeliveryPeriod)
            .ToList();

        var nthPayment = allAdditionalPayments
            .Skip(n - 1)
            .FirstOrDefault();

        if (nthPayment != null && (nthPayment.DueDate < joinedPriceEpisode.StartDate || nthPayment.DueDate > joinedPriceEpisode.EndDate))
        {
            nthPayment = null;
        }

        return BuildPriceEpisodePeriodisedValuesFromFunc(
            attributeName,
            (period) => nthPayment?.AcademicYear == academicYear && nthPayment.DeliveryPeriod == period ? nthPayment.Amount : 0
        );
    }

    private static List<JoinedInstalment> GetInstalmentsForAcademicYear(JoinedPriceEpisode joinedPriceEpisode, InstalmentType instalmentType, short academicYear)
    {
        return joinedPriceEpisode.Instalments
            .Where(i => i.AcademicYear == academicYear && i.InstalmentType == instalmentType)
            .ToList();
    }

    private static List<JoinedAdditionalPayment> GetAdditionalPayments(JoinedPriceEpisode joinedPriceEpisode, string additionalPaymentType)
    {
        return joinedPriceEpisode.AdditionalPayments
            .Where(i => i.AdditionalPaymentType == additionalPaymentType)
            .ToList();
    }

    public static PriceEpisodePeriodisedValues BuildPriceEpisodePeriodisedValuesFromFunc(
        string attributeName,
        Func<byte, decimal> valueFunc)
    {
        return new PriceEpisodePeriodisedValues
        {
            AttributeName = attributeName,
            Period1 = valueFunc(1),
            Period2 = valueFunc(2),
            Period3 = valueFunc(3),
            Period4 = valueFunc(4),
            Period5 = valueFunc(5),
            Period6 = valueFunc(6),
            Period7 = valueFunc(7),
            Period8 = valueFunc(8),
            Period9 = valueFunc(9),
            Period10 = valueFunc(10),
            Period11 = valueFunc(11),
            Period12 = valueFunc(12)
        };
    }
}