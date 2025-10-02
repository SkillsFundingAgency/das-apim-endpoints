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

        return new PriceEpisodePeriodisedValues
        {
            AttributeName = EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeInstalmentsThisPeriod,
            Period1 = (instalments.SingleOrDefault(i => i.DeliveryPeriod == 1)?.Amount).GetValueOrDefault() != 0 ? 1 : 0,
            Period2 = (instalments.SingleOrDefault(i => i.DeliveryPeriod == 2)?.Amount).GetValueOrDefault() != 0 ? 1 : 0,
            Period3 = (instalments.SingleOrDefault(i => i.DeliveryPeriod == 3)?.Amount).GetValueOrDefault() != 0 ? 1 : 0,
            Period4 = (instalments.SingleOrDefault(i => i.DeliveryPeriod == 4)?.Amount).GetValueOrDefault() != 0 ? 1 : 0,
            Period5 = (instalments.SingleOrDefault(i => i.DeliveryPeriod == 5)?.Amount).GetValueOrDefault() != 0 ? 1 : 0,
            Period6 = (instalments.SingleOrDefault(i => i.DeliveryPeriod == 6)?.Amount).GetValueOrDefault() != 0 ? 1 : 0,
            Period7 = (instalments.SingleOrDefault(i => i.DeliveryPeriod == 7)?.Amount).GetValueOrDefault() != 0 ? 1 : 0,
            Period8 = (instalments.SingleOrDefault(i => i.DeliveryPeriod == 8)?.Amount).GetValueOrDefault() != 0 ? 1 : 0,
            Period9 = (instalments.SingleOrDefault(i => i.DeliveryPeriod == 9)?.Amount).GetValueOrDefault() != 0 ? 1 : 0,
            Period10 = (instalments.SingleOrDefault(i => i.DeliveryPeriod == 10)?.Amount).GetValueOrDefault() != 0 ? 1 : 0,
            Period11 = (instalments.SingleOrDefault(i => i.DeliveryPeriod == 11)?.Amount).GetValueOrDefault() != 0 ? 1 : 0,
            Period12 = (instalments.SingleOrDefault(i => i.DeliveryPeriod == 12)?.Amount).GetValueOrDefault() != 0 ? 1 : 0,
        };
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

        return new PriceEpisodePeriodisedValues
        {
            AttributeName = attributeName,
            Period1 = additionalPayments.SingleOrDefault(x => x.AcademicYear == academicYear && x.DeliveryPeriod == 1)?.Amount ?? 0,
            Period2 = additionalPayments.SingleOrDefault(x => x.AcademicYear == academicYear && x.DeliveryPeriod == 2)?.Amount ?? 0,
            Period3 = additionalPayments.SingleOrDefault(x => x.AcademicYear == academicYear && x.DeliveryPeriod == 3)?.Amount ?? 0,
            Period4 = additionalPayments.SingleOrDefault(x => x.AcademicYear == academicYear && x.DeliveryPeriod == 4)?.Amount ?? 0,
            Period5 = additionalPayments.SingleOrDefault(x => x.AcademicYear == academicYear && x.DeliveryPeriod == 5)?.Amount ?? 0,
            Period6 = additionalPayments.SingleOrDefault(x => x.AcademicYear == academicYear && x.DeliveryPeriod == 6)?.Amount ?? 0,
            Period7 = additionalPayments.SingleOrDefault(x => x.AcademicYear == academicYear && x.DeliveryPeriod == 7)?.Amount ?? 0,
            Period8 = additionalPayments.SingleOrDefault(x => x.AcademicYear == academicYear && x.DeliveryPeriod == 8)?.Amount ?? 0,
            Period9 = additionalPayments.SingleOrDefault(x => x.AcademicYear == academicYear && x.DeliveryPeriod == 9)?.Amount ?? 0,
            Period10 = additionalPayments.SingleOrDefault(x => x.AcademicYear == academicYear && x.DeliveryPeriod == 10)?.Amount ?? 0,
            Period11 = additionalPayments.SingleOrDefault(x => x.AcademicYear == academicYear && x.DeliveryPeriod == 11)?.Amount ?? 0,
            Period12 = additionalPayments.SingleOrDefault(x => x.AcademicYear == academicYear && x.DeliveryPeriod == 12)?.Amount ?? 0
        };
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

        return new PriceEpisodePeriodisedValues
        {
            AttributeName = attributeName,
            Period1 = (instalments.SingleOrDefault(i => i.DeliveryPeriod == 1)?.Amount * multiplier).GetValueOrDefault(),
            Period2 = (instalments.SingleOrDefault(i => i.DeliveryPeriod == 2)?.Amount * multiplier).GetValueOrDefault(),
            Period3 = (instalments.SingleOrDefault(i => i.DeliveryPeriod == 3)?.Amount * multiplier).GetValueOrDefault(),
            Period4 = (instalments.SingleOrDefault(i => i.DeliveryPeriod == 4)?.Amount * multiplier).GetValueOrDefault(),
            Period5 = (instalments.SingleOrDefault(i => i.DeliveryPeriod == 5)?.Amount * multiplier).GetValueOrDefault(),
            Period6 = (instalments.SingleOrDefault(i => i.DeliveryPeriod == 6)?.Amount * multiplier).GetValueOrDefault(),
            Period7 = (instalments.SingleOrDefault(i => i.DeliveryPeriod == 7)?.Amount * multiplier).GetValueOrDefault(),
            Period8 = (instalments.SingleOrDefault(i => i.DeliveryPeriod == 8)?.Amount * multiplier).GetValueOrDefault(),
            Period9 = (instalments.SingleOrDefault(i => i.DeliveryPeriod == 9)?.Amount * multiplier).GetValueOrDefault(),
            Period10 = (instalments.SingleOrDefault(i => i.DeliveryPeriod == 10)?.Amount * multiplier).GetValueOrDefault(),
            Period11 = (instalments.SingleOrDefault(i => i.DeliveryPeriod == 11)?.Amount * multiplier).GetValueOrDefault(),
            Period12 = (instalments.SingleOrDefault(i => i.DeliveryPeriod == 12)?.Amount * multiplier).GetValueOrDefault()
        };
    }

    public static void AddNthIncentivePaymentValues(
        this List<PriceEpisodePeriodisedValues> list,
        JoinedEarningsApprenticeship joinedApprenticeship,
        JoinedPriceEpisode joinedPriceEpisode,
        short academicYear,
        string attributeName,
        string additionalPaymentType,
        int n)
    {
        try
        {
            list.Add(BuildNthIncentivePaymentValues(joinedApprenticeship, joinedPriceEpisode, academicYear, attributeName, additionalPaymentType, n));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to Add NthIncentivePaymentValues {attributeName} to PriceEpisodePeriodisedValues list.",
                ex);
        }
    }

    private static PriceEpisodePeriodisedValues BuildNthIncentivePaymentValues(JoinedEarningsApprenticeship joinedApprenticeship, JoinedPriceEpisode joinedPriceEpisode, short academicYear, string attributeName, string additionalPaymentType, int n)
    {
        var allAdditionalPayments = joinedApprenticeship.Episodes.SelectMany(x =>
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

        var periodFunction = (int period) => nthPayment?.AcademicYear == academicYear && nthPayment.DeliveryPeriod == period ? nthPayment.Amount : 0;

        return new PriceEpisodePeriodisedValues
        {
            AttributeName = attributeName,
            Period1 = periodFunction(1),
            Period2 = periodFunction(2),
            Period3 = periodFunction(3),
            Period4 = periodFunction(4),
            Period5 = periodFunction(5),
            Period6 = periodFunction(6),
            Period7 = periodFunction(7),
            Period8 = periodFunction(8),
            Period9 = periodFunction(9),
            Period10 = periodFunction(10),
            Period11 = periodFunction(11),
            Period12 = periodFunction(12)
        };
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
}