﻿using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
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

    public static PriceEpisodePeriodisedValues BuildAdditionalPaymentsValues(Apprenticeship earningsApprenticeship, short academicYear, string attributeName, string additionalPaymentType)
    {
        var additionalPayments = GetAdditionalPaymentsForAcademicYear(earningsApprenticeship, academicYear, additionalPaymentType);

        return new PriceEpisodePeriodisedValues
        {
            AttributeName = attributeName,
            Period1 = (additionalPayments.SingleOrDefault(i => i.DeliveryPeriod == 1)?.Amount).GetValueOrDefault(),
            Period2 = (additionalPayments.SingleOrDefault(i => i.DeliveryPeriod == 2)?.Amount).GetValueOrDefault(),
            Period3 = (additionalPayments.SingleOrDefault(i => i.DeliveryPeriod == 3)?.Amount).GetValueOrDefault(),
            Period4 = (additionalPayments.SingleOrDefault(i => i.DeliveryPeriod == 4)?.Amount).GetValueOrDefault(),
            Period5 = (additionalPayments.SingleOrDefault(i => i.DeliveryPeriod == 5)?.Amount).GetValueOrDefault(),
            Period6 = (additionalPayments.SingleOrDefault(i => i.DeliveryPeriod == 6)?.Amount).GetValueOrDefault(),
            Period7 = (additionalPayments.SingleOrDefault(i => i.DeliveryPeriod == 7)?.Amount).GetValueOrDefault(),
            Period8 = (additionalPayments.SingleOrDefault(i => i.DeliveryPeriod == 8)?.Amount).GetValueOrDefault(),
            Period9 = (additionalPayments.SingleOrDefault(i => i.DeliveryPeriod == 9)?.Amount).GetValueOrDefault(),
            Period10 = (additionalPayments.SingleOrDefault(i => i.DeliveryPeriod == 10)?.Amount).GetValueOrDefault(),
            Period11 = (additionalPayments.SingleOrDefault(i => i.DeliveryPeriod == 11)?.Amount).GetValueOrDefault(),
            Period12 = (additionalPayments.SingleOrDefault(i => i.DeliveryPeriod == 12)?.Amount).GetValueOrDefault()
        };
    }

    private static List<Instalment> GetInstalmentsForAcademicYear(Apprenticeship earningsApprenticeship, short academicYear)
    {
        return earningsApprenticeship.Episodes
            .SelectMany(episode => episode.Instalments)
            .Where(i => i.AcademicYear == academicYear)
            .ToList();
    }

    private static List<AdditionalPayment> GetAdditionalPaymentsForAcademicYear(Apprenticeship earningsApprenticeship, short academicYear, string additionalPaymentType)
    {
        return earningsApprenticeship.Episodes
            .SelectMany(episode => episode.AdditionalPayments)
            .Where(i => i.AcademicYear == academicYear
                        && i.AdditionalPaymentType == additionalPaymentType)
            .ToList();
    }
}