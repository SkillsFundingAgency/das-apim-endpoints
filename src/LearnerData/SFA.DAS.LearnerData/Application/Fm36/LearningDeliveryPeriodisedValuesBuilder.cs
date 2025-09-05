using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;

namespace SFA.DAS.LearnerData.Application.Fm36;

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
        JoinedEarningsApprenticeship apprenticeship,
        short academicYear,
        InstalmentType instalmentType = InstalmentType.Regular)
    {
        try
        {
            list.Add(BuildInstPerPeriodValues(apprenticeship, academicYear, instalmentType));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to Add InstPerPeriodValues {EarningsFM36Constants.PeriodisedAttributes.InstPerPeriod} to LearningDeliveryPeriodisedValues list.",
                ex);
        }
    }

    private static LearningDeliveryPeriodisedValues BuildInstPerPeriodValues(JoinedEarningsApprenticeship apprenticeship, short academicYear, InstalmentType instalmentType)
    {
        var instalments = GetInstalmentsForAcademicYear(apprenticeship, academicYear, instalmentType);

        return new LearningDeliveryPeriodisedValues
        {
            AttributeName = EarningsFM36Constants.PeriodisedAttributes.InstPerPeriod,
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

    public static void AddInstallmentAmountValues(
        this List<LearningDeliveryPeriodisedValues>
        list, JoinedEarningsApprenticeship apprenticeship,
        short academicYear,
        string attributeName,
        InstalmentType instalmentType = InstalmentType.Regular)
    {
        try
        {
            list.Add(BuildCoInvestmentValues(apprenticeship, academicYear, attributeName, 1, instalmentType));
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
        JoinedEarningsApprenticeship apprenticeship,
        short academicYear,
        string attributeName,
        decimal multiplier,
        InstalmentType instalmentType = InstalmentType.Regular)
    {
        try
        {
            list.Add(BuildCoInvestmentValues(apprenticeship, academicYear, attributeName, multiplier, instalmentType));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to Add CoInvestmentValues {attributeName} to LearningDeliveryPeriodisedValues list.",
                ex);
        }
    }

    private static LearningDeliveryPeriodisedValues BuildCoInvestmentValues(JoinedEarningsApprenticeship apprenticeship, short academicYear, string attributeName, decimal multiplier, InstalmentType instalmentType)
    {
        var instalments = GetInstalmentsForAcademicYear(apprenticeship, academicYear, instalmentType);

        return new LearningDeliveryPeriodisedValues
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
        this List<LearningDeliveryPeriodisedValues> list,
        JoinedEarningsApprenticeship apprenticeship,
        short academicYear,
        string attributeName,
        string additionalPaymentType,
        int n)
    {
        try
        {
            list.Add(BuildNthIncentivePaymentValues(apprenticeship, academicYear, attributeName, additionalPaymentType, n));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to Add NthIncentivePaymentValues {attributeName} to LearningDeliveryPeriodisedValues list.",
                ex);
        }
    }

    private static LearningDeliveryPeriodisedValues BuildNthIncentivePaymentValues(JoinedEarningsApprenticeship apprenticeship, short academicYear, string attributeName, string additionalPaymentType, int n)
    {
        var additionalPayments = GetAdditionalPayments(apprenticeship, additionalPaymentType);

        var nthPayment = additionalPayments
            .OrderBy(i => i.AcademicYear)
            .ThenBy(i => i.DeliveryPeriod)
            .Skip(n - 1)
            .FirstOrDefault();

        return new LearningDeliveryPeriodisedValues
        {
            AttributeName = attributeName,
            Period1 = nthPayment?.AcademicYear == academicYear && nthPayment.DeliveryPeriod == 1 ? nthPayment.Amount : 0,
            Period2 = nthPayment?.AcademicYear == academicYear && nthPayment.DeliveryPeriod == 2 ? nthPayment.Amount : 0,
            Period3 = nthPayment?.AcademicYear == academicYear && nthPayment.DeliveryPeriod == 3 ? nthPayment.Amount : 0,
            Period4 = nthPayment?.AcademicYear == academicYear && nthPayment.DeliveryPeriod == 4 ? nthPayment.Amount : 0,
            Period5 = nthPayment?.AcademicYear == academicYear && nthPayment.DeliveryPeriod == 5 ? nthPayment.Amount : 0,
            Period6 = nthPayment?.AcademicYear == academicYear && nthPayment.DeliveryPeriod == 6 ? nthPayment.Amount : 0,
            Period7 = nthPayment?.AcademicYear == academicYear && nthPayment.DeliveryPeriod == 7 ? nthPayment.Amount : 0,
            Period8 = nthPayment?.AcademicYear == academicYear && nthPayment.DeliveryPeriod == 8 ? nthPayment.Amount : 0,
            Period9 = nthPayment?.AcademicYear == academicYear && nthPayment.DeliveryPeriod == 9 ? nthPayment.Amount : 0,
            Period10 = nthPayment?.AcademicYear == academicYear && nthPayment.DeliveryPeriod == 10 ? nthPayment.Amount : 0,
            Period11 = nthPayment?.AcademicYear == academicYear && nthPayment.DeliveryPeriod == 11 ? nthPayment.Amount : 0,
            Period12 = nthPayment?.AcademicYear == academicYear && nthPayment.DeliveryPeriod == 12 ? nthPayment.Amount : 0
        };
    }

    public static void AddAdditionalPaymentPerPeriodValues(
        this List<LearningDeliveryPeriodisedValues> list,
        JoinedEarningsApprenticeship apprenticeship,
        short academicYear,
        string attributeName,
        string additionalPaymentType)
    {
        try
        {
            list.Add(BuildAdditionalPaymentPerPeriodValues(apprenticeship, academicYear, attributeName, additionalPaymentType));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to Add AdditionalPaymentPerPeriodValues {attributeName} to LearningDeliveryPeriodisedValues list.",
                ex);
        }
    }

    private static LearningDeliveryPeriodisedValues BuildAdditionalPaymentPerPeriodValues(JoinedEarningsApprenticeship apprenticeship, short academicYear, string attributeName, string additionalPaymentType)
    {
        var additionalPayments = GetAdditionalPayments(apprenticeship, additionalPaymentType);

        return new LearningDeliveryPeriodisedValues
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

    public static void AddAdditionalPaymentPerPeriodIndicators(
        this List<LearningDeliveryPeriodisedValues> list,
        JoinedEarningsApprenticeship apprenticeship,
        short academicYear,
        string attributeName,
        string additionalPaymentType)
    {
        try
        {
            list.Add(BuildAdditionalPaymentPerPeriodIndicators(apprenticeship, academicYear, attributeName, additionalPaymentType));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to Add AdditionalPaymentPerPeriodIndicators {attributeName} to LearningDeliveryPeriodisedValues list.",
                ex);
        }
    }

    private static LearningDeliveryPeriodisedValues BuildAdditionalPaymentPerPeriodIndicators(JoinedEarningsApprenticeship apprenticeship, short academicYear, string attributeName, string additionalPaymentType)
    {
        var additionalPayments = GetAdditionalPayments(apprenticeship, additionalPaymentType);

        return new LearningDeliveryPeriodisedValues
        {
            AttributeName = attributeName,
            Period1 = additionalPayments.Exists(x => x.AcademicYear == academicYear && x.DeliveryPeriod == 1) ? 1 : 0,
            Period2 = additionalPayments.Exists(x => x.AcademicYear == academicYear && x.DeliveryPeriod == 2) ? 1 : 0,
            Period3 = additionalPayments.Exists(x => x.AcademicYear == academicYear && x.DeliveryPeriod == 3) ? 1 : 0,
            Period4 = additionalPayments.Exists(x => x.AcademicYear == academicYear && x.DeliveryPeriod == 4) ? 1 : 0,
            Period5 = additionalPayments.Exists(x => x.AcademicYear == academicYear && x.DeliveryPeriod == 5) ? 1 : 0,
            Period6 = additionalPayments.Exists(x => x.AcademicYear == academicYear && x.DeliveryPeriod == 6) ? 1 : 0,
            Period7 = additionalPayments.Exists(x => x.AcademicYear == academicYear && x.DeliveryPeriod == 7) ? 1 : 0,
            Period8 = additionalPayments.Exists(x => x.AcademicYear == academicYear && x.DeliveryPeriod == 8) ? 1 : 0,
            Period9 = additionalPayments.Exists(x => x.AcademicYear == academicYear && x.DeliveryPeriod == 9) ? 1 : 0,
            Period10 = additionalPayments.Exists(x => x.AcademicYear == academicYear && x.DeliveryPeriod == 10) ? 1 : 0,
            Period11 = additionalPayments.Exists(x => x.AcademicYear == academicYear && x.DeliveryPeriod == 11) ? 1 : 0,
            Period12 = additionalPayments.Exists(x => x.AcademicYear == academicYear && x.DeliveryPeriod == 12) ? 1 : 0
        };
    }

    private static List<JoinedInstalment> GetInstalmentsForAcademicYear(JoinedEarningsApprenticeship apprenticeship, short academicYear, InstalmentType instalmentType)
    {
        return apprenticeship.Episodes
            .SelectMany(episode => episode.Instalments)
            .Where(i => i.AcademicYear == academicYear && i.InstalmentType == instalmentType)
            .ToList();
    }

    private static List<JoinedAdditionalPayment> GetAdditionalPayments(JoinedEarningsApprenticeship apprenticeship, string additionalPaymentType)
    {
        return apprenticeship.Episodes
            .SelectMany(episode => episode.AdditionalPayments)
            .Where(i => i.AdditionalPaymentType == additionalPaymentType)
            .ToList();
    }
}
