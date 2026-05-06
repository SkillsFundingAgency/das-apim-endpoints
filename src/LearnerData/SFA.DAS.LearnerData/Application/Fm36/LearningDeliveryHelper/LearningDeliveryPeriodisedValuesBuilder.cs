using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using SFA.DAS.LearnerData.Application.Fm36.Common;

namespace SFA.DAS.LearnerData.Application.Fm36.LearningDeliveryHelper;

public static class LearningDeliveryPeriodisedValuesBuilder
{
    public static void AddWithSamePeriodisedValues(
        this List<LearningDeliveryPeriodisedValues> list,
        string attributeName,
        decimal value)
    {
        list.Add(BuildWithSameValues(attributeName, value));
    }

    private static LearningDeliveryPeriodisedValues BuildWithSameValues(string attributeName, decimal value)
    {
        return new LearningDeliveryPeriodisedValues
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

    public static void AddInstPerPeriodValues(
        this List<LearningDeliveryPeriodisedValues> list,
        JoinedLearningDelivery joinedLearningDelivery,
        short academicYear,
        InstalmentType instalmentType = InstalmentType.Regular)
    {
        try
        {
            list.Add(BuildInstPerPeriodValues(joinedLearningDelivery, academicYear, instalmentType));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to Add InstPerPeriodValues {EarningsFM36Constants.PeriodisedAttributes.InstPerPeriod} to LearningDeliveryPeriodisedValues list.",
                ex);
        }
    }

    private static LearningDeliveryPeriodisedValues BuildInstPerPeriodValues(JoinedLearningDelivery joinedLearningDelivery, short academicYear, InstalmentType instalmentType)
    {
        var instalments = GetInstalmentsForAcademicYear(joinedLearningDelivery, academicYear, instalmentType);

        return BuildLearningDeliveryPeriodisedValuesFromFunc(
            EarningsFM36Constants.PeriodisedAttributes.InstPerPeriod,
            period => (instalments.SingleOrDefault(i => i.DeliveryPeriod == period)?.Amount).GetValueOrDefault() != 0 ? 1 : 0
        );
    }

    public static void AddInstallmentAmountValues(
        this List<LearningDeliveryPeriodisedValues> list,
        string attributeName,
        JoinedLearningDelivery joinedLearningDelivery,
        short academicYear,
        InstalmentType instalmentType = InstalmentType.Regular)
    {
        try
        {
            list.Add(BuildCoInvestmentValues(joinedLearningDelivery, academicYear, attributeName, 1, instalmentType));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to Add InstallmentAmountValues {attributeName} to LearningDeliveryPeriodisedValues list.",
                ex);
        }
    }

    public static void AddCoInvestmentValues(
        this List<LearningDeliveryPeriodisedValues> list,
        string attributeName,
        JoinedLearningDelivery joinedLearningDelivery,
        short academicYear,
        decimal multiplier,
        InstalmentType instalmentType = InstalmentType.Regular)
    {
        try
        {
            list.Add(BuildCoInvestmentValues(joinedLearningDelivery, academicYear, attributeName, multiplier, instalmentType));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to Add CoInvestmentValues {attributeName} to LearningDeliveryPeriodisedValues list.",
                ex);
        }
    }

    private static LearningDeliveryPeriodisedValues BuildCoInvestmentValues(JoinedLearningDelivery joinedLearningDelivery, short academicYear, string attributeName, decimal multiplier, InstalmentType instalmentType)
    {
        var instalments = GetInstalmentsForAcademicYear(joinedLearningDelivery, academicYear, instalmentType);

        return BuildLearningDeliveryPeriodisedValuesFromFunc(
            attributeName,
            period => (instalments.SingleOrDefault(i => i.DeliveryPeriod == period)?.Amount * multiplier).GetValueOrDefault()
        );
    }

    public static void AddNthIncentivePaymentValues(
        this List<LearningDeliveryPeriodisedValues> list,
        string attributeName,
        JoinedLearningDelivery joinedLearningDelivery,
        short academicYear,
        string additionalPaymentType,
        int n)
    {
        try
        {
            list.Add(BuildNthIncentivePaymentValues(joinedLearningDelivery, academicYear, attributeName, additionalPaymentType, n));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to Add NthIncentivePaymentValues {attributeName} to LearningDeliveryPeriodisedValues list.",
                ex);
        }
    }

    private static LearningDeliveryPeriodisedValues BuildNthIncentivePaymentValues(JoinedLearningDelivery joinedLearningDelivery, short academicYear, string attributeName, string additionalPaymentType, int n)
    {
        var additionalPayments = GetAdditionalPayments(joinedLearningDelivery, additionalPaymentType);

        var nthPayment = additionalPayments
            .OrderBy(i => i.AcademicYear)
            .ThenBy(i => i.DeliveryPeriod)
            .Skip(n - 1)
            .FirstOrDefault();

        return BuildLearningDeliveryPeriodisedValuesFromFunc(
            attributeName,
            period => nthPayment?.AcademicYear == academicYear && nthPayment.DeliveryPeriod == period ? nthPayment.Amount : 0
        );
    }

    public static void AddAdditionalPaymentPerPeriodValues(
        this List<LearningDeliveryPeriodisedValues> list,
        string attributeName,
        JoinedLearningDelivery joinedLearningDelivery,
        short academicYear,
        string additionalPaymentType)
    {
        try
        {
            list.Add(BuildAdditionalPaymentPerPeriodValues(joinedLearningDelivery, academicYear, attributeName, additionalPaymentType));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to Add AdditionalPaymentPerPeriodValues {attributeName} to LearningDeliveryPeriodisedValues list.",
                ex);
        }
    }

    private static LearningDeliveryPeriodisedValues BuildAdditionalPaymentPerPeriodValues(JoinedLearningDelivery joinedLearningDelivery, short academicYear, string attributeName, string additionalPaymentType)
    {
        var additionalPayments = GetAdditionalPayments(joinedLearningDelivery, additionalPaymentType);

        return BuildLearningDeliveryPeriodisedValuesFromFunc(
            attributeName,
            period => additionalPayments.SingleOrDefault(x => x.AcademicYear == academicYear && x.DeliveryPeriod == period)?.Amount ?? 0
        );
    }

    public static void AddAdditionalPaymentPerPeriodIndicators(
        this List<LearningDeliveryPeriodisedValues> list,
        string attributeName,
        JoinedLearningDelivery joinedLearningDelivery,
        short academicYear,
        string additionalPaymentType)
    {
        try
        {
            list.Add(BuildAdditionalPaymentPerPeriodIndicators(joinedLearningDelivery, academicYear, attributeName, additionalPaymentType));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to Add AdditionalPaymentPerPeriodIndicators {attributeName} to LearningDeliveryPeriodisedValues list.",
                ex);
        }
    }

    private static LearningDeliveryPeriodisedValues BuildAdditionalPaymentPerPeriodIndicators(JoinedLearningDelivery joinedLearningDelivery, short academicYear, string attributeName, string additionalPaymentType)
    {
        var additionalPayments = GetAdditionalPayments(joinedLearningDelivery, additionalPaymentType);

        return BuildLearningDeliveryPeriodisedValuesFromFunc(
            attributeName,
            period => additionalPayments.Exists(x => x.AcademicYear == academicYear && x.DeliveryPeriod == period) ? 1 : 0
        );
    }

    private static List<JoinedInstalment> GetInstalmentsForAcademicYear(JoinedLearningDelivery joinedLearningDelivery, short academicYear, InstalmentType instalmentType)
    {
        return joinedLearningDelivery.Instalments
            .Where(i => i.AcademicYear == academicYear && i.InstalmentType == instalmentType)
            .ToList();
    }

    private static List<JoinedAdditionalPayment> GetAdditionalPayments(JoinedLearningDelivery joinedLearningDelivery, string additionalPaymentType)
    {
        return joinedLearningDelivery.AdditionalPayments
            .Where(i => i.AdditionalPaymentType == additionalPaymentType)
            .ToList();
    }

    public static LearningDeliveryPeriodisedValues BuildLearningDeliveryPeriodisedValuesFromFunc(
        string attributeName,
        Func<byte, decimal> valueFunc)
    {
        return new LearningDeliveryPeriodisedValues
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