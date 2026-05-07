using System.Linq.Expressions;
using System.Reflection;
using SFA.DAS.SharedOuterApi.Types.Domain.Recruit;

namespace SFA.DAS.SharedOuterApi.Recruit.Services;

public interface IVacancyComparerService
{
    VacancyComparerResult Compare(Vacancy a, Vacancy b);
}

public class VacancyComparerService: IVacancyComparerService
{
    public VacancyComparerResult Compare(Vacancy a, Vacancy b)
    {
        var fields = new List<VacancyComparerField>
        {
            CompareValue(a, b, v => v.VacancyReference, FieldIdResolver.ToFieldId(v => v.VacancyReference)),
            CompareValue(a, b, v => v.AccountId, FieldIdResolver.ToFieldId(v => v.AccountId)),
            CompareValue(a, b, v => v.AnonymousReason, FieldIdResolver.ToFieldId(v => v.AnonymousReason)),
            CompareValue(a, b, v => v.ApplicationInstructions, FieldIdResolver.ToFieldId(v => v.ApplicationInstructions)),
            CompareValue(a, b, v => v.ApplicationMethod, FieldIdResolver.ToFieldId(v => v.ApplicationMethod)),
            CompareValue(a, b, v => v.ApplicationUrl, FieldIdResolver.ToFieldId(v => v.ApplicationUrl)),
            CompareValue(a, b, v => v.ClosingDate, FieldIdResolver.ToFieldId(v => v.ClosingDate)),
            CompareValue(a, b, v => v.Description, FieldIdResolver.ToFieldId(v => v.Description)),
            CompareValue(a, b, v => v.DisabilityConfident, FieldIdResolver.ToFieldId(v => v.DisabilityConfident)),
            CompareValue(a, b, v => v.Contact?.Email, FieldIdResolver.ToFieldId(v => v.Contact.Email)),
            CompareValue(a, b, v => v.Contact?.Name, FieldIdResolver.ToFieldId(v => v.Contact.Name)),
            CompareValue(a, b, v => v.Contact?.Phone, FieldIdResolver.ToFieldId(v => v.Contact.Phone)),
            CompareValue(a, b, v => v.EmployerDescription, FieldIdResolver.ToFieldId(v => v.EmployerDescription)),
            CompareList(a, b, v => v.EmployerLocations,  FieldIdResolver.ToFieldId(v => v.EmployerLocations)),
            CompareValue(a, b, v => v.EmployerLocationInformation,  FieldIdResolver.ToFieldId(v => v.EmployerLocationInformation)),
            CompareValue(a, b, v => v.EmployerName, FieldIdResolver.ToFieldId(v => v.EmployerName)),
            CompareValue(a, b, v => v.EmployerWebsiteUrl, FieldIdResolver.ToFieldId(v => v.EmployerWebsiteUrl)),
            CompareValue(a, b, v => v.NumberOfPositions, FieldIdResolver.ToFieldId(v => v.NumberOfPositions)),
            CompareValue(a, b, v => v.OutcomeDescription, FieldIdResolver.ToFieldId(v => v.OutcomeDescription)),
            CompareValue(a, b, v => v.ProgrammeId, FieldIdResolver.ToFieldId(v => v.ProgrammeId)),
            CompareList(a, b, v => v.Qualifications, FieldIdResolver.ToFieldId(v => v.Qualifications)),
            CompareValue(a, b, v => v.ShortDescription, FieldIdResolver.ToFieldId(v => v.ShortDescription)),
            CompareList(a, b, v => v.Skills!, FieldIdResolver.ToFieldId(v => v.Skills)),
            CompareValue(a, b, v => v.StartDate, FieldIdResolver.ToFieldId(v => v.StartDate)),
            CompareValue(a, b, v => v.ThingsToConsider, FieldIdResolver.ToFieldId(v => v.ThingsToConsider)),
            CompareValue(a, b, v => v.Title, FieldIdResolver.ToFieldId(v => v.Title)),
            CompareValue(a, b, v => v.TrainingDescription, FieldIdResolver.ToFieldId(v => v.TrainingDescription)),
            CompareValue(a, b, v => v.TrainingProvider?.Ukprn, FieldIdResolver.ToFieldId(v => v.TrainingProvider.Ukprn)),
            CompareValue(a, b, v => v.Wage?.WeeklyHours, FieldIdResolver.ToFieldId(v => v.Wage.WeeklyHours)),
            CompareValue(a, b, v => v.Wage?.WorkingWeekDescription, FieldIdResolver.ToFieldId(v => v.Wage.WorkingWeekDescription)),
            CompareValue(a, b, v => v.Wage?.WageAdditionalInformation, FieldIdResolver.ToFieldId(v => v.Wage.WageAdditionalInformation)),
            CompareValue(a, b, v => v.Wage?.WageType, FieldIdResolver.ToFieldId(v => v.Wage.WageType)),
            CompareValue(a, b, v => v.Wage?.FixedWageYearlyAmount, FieldIdResolver.ToFieldId(v => v.Wage.FixedWageYearlyAmount)),
            CompareValue(a, b, v => v.Wage?.Duration, FieldIdResolver.ToFieldId(v => v.Wage.Duration)),
            CompareValue(a, b, v => v.Wage?.DurationUnit, FieldIdResolver.ToFieldId(v => v.Wage.DurationUnit)),
            CompareValue(a, b, v => v.AdditionalQuestion1, FieldIdResolver.ToFieldId(v => v.AdditionalQuestion1)),
            CompareValue(a, b, v => v.AdditionalQuestion2, FieldIdResolver.ToFieldId(v => v.AdditionalQuestion2))
        };
        
        return new VacancyComparerResult {Fields = fields };
    }

    private static VacancyComparerField CompareValue<T, TP>(T a, T b, Func<T, TP> valueFunc, string fieldName)
    {
        var aValue = valueFunc(a);
        var bValue = valueFunc(b);

        var areEqual = EqualityComparer<TP>.Default.Equals(aValue, bValue);
        return new VacancyComparerField(fieldName, areEqual);
    }
    
    private static VacancyComparerField CompareList<T, TP>(T a, T b, Func<T, IEnumerable<TP>?> valueFunc, string fieldName)
    {
        var aValue = valueFunc(a) ?? [];
        var bValue = valueFunc(b) ?? [];

        var areEqual = aValue.SequenceEqual(bValue);
        return new VacancyComparerField(fieldName, areEqual);
    }
}

public record VacancyComparerField(string FieldName, bool AreEqual);

public class VacancyComparerResult
{
    public IEnumerable<VacancyComparerField> Fields = [];
}

public static class FieldIdResolver
{
    public static string ToFieldId<TP>(Expression<Func<Vacancy, TP>> fieldName)
    {
        return fieldName.GetQualifiedFieldId();
    }
}

public static class ExpressionExtensions
{
    public static string GetQualifiedFieldId<P>(this Expression<Func<P>> property)
    {
        return GetFieldIdForProperty(property.Body);
    }
        
    public static string GetQualifiedFieldId<T,P>(this Expression<Func<T, P>> property)
    {
        return GetFieldIdForProperty(property.Body);
    }

    private static string GetFieldIdForProperty(Expression propertyBody)
    {
        var memberExpression = propertyBody as MemberExpression;
        var fieldId = string.Empty;

        while (memberExpression is { Member.MemberType: MemberTypes.Property })
        {
            fieldId = memberExpression.Member.Name + (fieldId != string.Empty ? "." : string.Empty) + fieldId;
            memberExpression = memberExpression.Expression as MemberExpression;
        }

        return fieldId;
    }
}