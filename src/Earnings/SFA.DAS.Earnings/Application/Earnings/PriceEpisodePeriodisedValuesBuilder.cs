using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using Microsoft.Azure.Amqp.Framing;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;

namespace SFA.DAS.Earnings.Application.Earnings;

public static class PriceEpisodePeriodisedValuesBuilder
{
    public static PriceEpisodePeriodisedValues BuildWithSameValues(string attributeName, decimal value)
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

    public static PriceEpisodePeriodisedValues BuildPriceEpisodeInstalmentsThisPeriodValues(Apprenticeship earningsApprenticeship, short academicYear)
    {
        var instalments = GetInstalmentsForAcademicYear(earningsApprenticeship, academicYear);

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

    public static PriceEpisodePeriodisedValues BuildInstallmentAmountValues(Apprenticeship earningsApprenticeship, short academicYear, string attributeName)
    {
        return BuildCoInvestmentValues(earningsApprenticeship, academicYear, attributeName, 1);
    }

    public static PriceEpisodePeriodisedValues BuildCoInvestmentValues(Apprenticeship earningsApprenticeship, short academicYear, string attributeName, decimal multiplier)
    {
        var instalments = GetInstalmentsForAcademicYear(earningsApprenticeship, academicYear);

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
    
    public static PriceEpisodePeriodisedValues BuildNthIncentivePaymentValues(Apprenticeship earningsApprenticeship, short academicYear, string attributeName, string additionalPaymentType, int n)
    {
        var additionalPayments = GetAdditionalPayments(earningsApprenticeship, additionalPaymentType);

        var nthPayment = additionalPayments
            .OrderBy(i => i.AcademicYear)
            .ThenBy(i => i.DeliveryPeriod)
            .Skip(n - 1)
            .FirstOrDefault();

        return new PriceEpisodePeriodisedValues
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

    private static List<Instalment> GetInstalmentsForAcademicYear(Apprenticeship earningsApprenticeship, short academicYear)
    {
        return earningsApprenticeship.Episodes
            .SelectMany(episode => episode.Instalments)
            .Where(i => i.AcademicYear == academicYear)
            .ToList();
    }

    private static List<AdditionalPayment> GetAdditionalPayments(Apprenticeship earningsApprenticeship, string additionalPaymentType)
    {
        return earningsApprenticeship.Episodes
            .SelectMany(episode => episode.AdditionalPayments)
            .Where(i => i.AdditionalPaymentType == additionalPaymentType)
            .ToList();
    }
}