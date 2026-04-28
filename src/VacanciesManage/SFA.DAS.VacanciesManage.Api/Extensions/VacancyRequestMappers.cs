using SFA.DAS.Recruit.Contracts.ApiResponses;
using SFA.DAS.VacanciesManage.Api.Models;
using System;
using DurationUnit = SFA.DAS.VacanciesManage.Api.Models.DurationUnit;
using EmployerNameOption = SFA.DAS.VacanciesManage.Api.Models.EmployerNameOption;
using QualificationWeighting = SFA.DAS.VacanciesManage.Api.Models.QualificationWeighting;
using WageType = SFA.DAS.VacanciesManage.Api.Models.WageType;

namespace SFA.DAS.VacanciesManage.Api.Extensions;

public static class VacancyRequestMappers
{
    public static Address ToDomainAddress(this CreateVacancyAddress source)
    {
        return new Address
        {
            AddressLine1 = source.AddressLine1,
            AddressLine2 = source.AddressLine2,
            AddressLine3 = source.AddressLine3,
            AddressLine4 = source.AddressLine4,
            Postcode = source.Postcode,
        };
    }

    public static ApplicationMethod ToDomainApplicationMethod(this CreateVacancyApplicationMethod source)
    {
        return source switch
        {
            CreateVacancyApplicationMethod.ThroughExternalApplicationSite => ApplicationMethod.ThroughExternalApplicationSite,
            CreateVacancyApplicationMethod.ThroughFindAnApprenticeship => ApplicationMethod.ThroughFindAnApprenticeship,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
        };
    }

    public static Recruit.Contracts.ApiResponses.EmployerNameOption ToDomainEmployerNameOption(this EmployerNameOption source)
    {
        return source switch
        {
            EmployerNameOption.Anonymous => Recruit.Contracts.ApiResponses.EmployerNameOption.Anonymous,
            EmployerNameOption.RegisteredName => Recruit.Contracts.ApiResponses.EmployerNameOption.RegisteredName,
            EmployerNameOption.TradingName => Recruit.Contracts.ApiResponses.EmployerNameOption.TradingName,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
        };
    }

    public static Wage ToDomainWage(this CreateVacancyWage source)
    {
        return new Wage
        {
            Duration = source.Duration,
            DurationUnit = source.DurationUnit.ToDomainDurationUnit(),
            WageType = source.WageType.ToDomainWageType(),
            WeeklyHours = (double?)source.WeeklyHours,
            WorkingWeekDescription = source.WorkingWeekDescription,
            FixedWageYearlyAmount = (double?)(source.WageType == WageType.FixedWage ? source.FixedWageYearlyAmount : null),
            WageAdditionalInformation = source.WageAdditionalInformation,
            CompanyBenefitsInformation = source.CompanyBenefitsInformation,
        };
    }

    public static Qualification ToDomainQualifications(this CreateVacancyQualification source)
    {
        return new Qualification
        {
            Grade = source.Grade,
            Level = source.Level,
            Subject = source.Subject,
            Weighting = source.Weighting.ToDomainQualificationWeighting(),
            QualificationType = source.QualificationType,
        };
    }

    public static Recruit.Contracts.ApiResponses.QualificationWeighting ToDomainQualificationWeighting(this QualificationWeighting source)
    {
        return source switch
        {
            QualificationWeighting.Essential => Recruit.Contracts.ApiResponses.QualificationWeighting.Essential,
            QualificationWeighting.Desired => Recruit.Contracts.ApiResponses.QualificationWeighting.Desired,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
        };
    }

    public static Recruit.Contracts.ApiResponses.DurationUnit ToDomainDurationUnit(this DurationUnit source)
    {
        return source switch
        {
            DurationUnit.Month => Recruit.Contracts.ApiResponses.DurationUnit.Month,
            DurationUnit.Year => Recruit.Contracts.ApiResponses.DurationUnit.Year,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
        };
    }

    public static Recruit.Contracts.ApiResponses.WageType ToDomainWageType(this WageType source)
    {
        return source switch
        {
            WageType.FixedWage => Recruit.Contracts.ApiResponses.WageType.FixedWage,
            WageType.NationalMinimumWage => Recruit.Contracts.ApiResponses.WageType.NationalMinimumWage,
            WageType.CompetitiveSalary => Recruit.Contracts.ApiResponses.WageType.CompetitiveSalary,
            WageType.NationalMinimumWageForApprentices => Recruit.Contracts.ApiResponses.WageType.NationalMinimumWageForApprentices,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
        };
    }
}