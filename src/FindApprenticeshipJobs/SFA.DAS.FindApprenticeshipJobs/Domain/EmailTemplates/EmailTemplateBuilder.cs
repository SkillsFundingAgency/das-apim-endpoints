using SFA.DAS.FindApprenticeshipJobs.Application.Commands.SavedSearch.SendNotification;
using SFA.DAS.FindApprenticeshipJobs.Domain.Constants;
using SFA.DAS.SharedOuterApi.Extensions;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Models;
using AvailableWhere = SFA.DAS.FindApprenticeshipJobs.Application.Shared.AvailableWhere;

namespace SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates;

public static partial class EmailTemplateBuilder
{
    [GeneratedRegex(@"\d+\.\d{2}")]
    private static partial Regex NhsWageAmountRegex();

    private const decimal NhsLowerWageAmountLimit = 100.00M;
    private const decimal NhsUpperWageAmountLimit = 5000.00M;

    public static string GetSavedSearchSearchParams(
        string? searchTerm,
        decimal? distance,
        string? location,
        List<string>? categories,
        List<string>? levels,
        bool? disabilityConfident,
        bool? excludeNational,
        List<ApprenticeshipTypes>? apprenticeshipTypes)
    {
        var sb = new StringBuilder();

        sb.AppendLine();
        if (!string.IsNullOrEmpty(searchTerm)) sb.AppendLine($"What: {searchTerm}");
            
        var locationText = location?.Trim() switch
        {
            "" => "Where: All of England",
            not null when distance is > 1 => $"Where: {location} (within {distance} miles)",
            not null when distance is 1 => $"Where: {location} (within 1 mile)",
            not null => $"Where: {location} (Across England)",
            null => "Where: All of England"
        };

        if (distance != null && excludeNational != null && excludeNational.Value)
        {
            sb.AppendLine($"{locationText} - hide companies recruiting nationally");
        }
        else
        {
            sb.AppendLine(locationText);
        }

        var apprenticeshipTypesStrings = apprenticeshipTypes?.Select(GetApprenticeshipTypeText).ToList();
        
        if (categories is { Count: > 0 }) sb.AppendLine($"Job category: {string.Join(", ", categories)}");
        if (apprenticeshipTypesStrings is { Count: > 0 }) sb.AppendLine($"Apprenticeship type: {string.Join(", ", apprenticeshipTypesStrings)}");
        if (levels is { Count: > 0 }) sb.AppendLine($"Apprenticeship level: {string.Join(", ", levels)}");
        if (disabilityConfident != null && disabilityConfident.Value) sb.AppendLine("Only show Disability Confident apprenticeships");
        sb.AppendLine();

        return sb.ToString();
    }

    public static string GetSavedSearchUrl(
        string? searchTerm,
        decimal? distance,
        string? location,
        List<string>? categoryIds,
        List<string>? levelCodes,
        bool? disabilityConfident,
        bool? excludeNational,
        List<ApprenticeshipTypes>? apprenticeshipTypes)
    {
        var queryParameters = new Dictionary<string, StringValues>
        {
            { "sort", "AgeAsc" },
            { "searchTerm", searchTerm },
            { "location", location },
            { "distance", distance > 0 ? $"{distance}" : null },
            { "DisabilityConfident", disabilityConfident?.ToString().ToLowerInvariant() },
            { "excludeNational", excludeNational?.ToString().ToLowerInvariant() },
            { "routeIds", categoryIds?.ToArray() },
            { "levelIds", levelCodes?.ToArray() },
            { "apprenticeshipTypes", apprenticeshipTypes?.Select(x => $"{x}").ToArray() },
        };
        
        return QueryHelpers.AddQueryString(string.Empty, queryParameters);
    }

    public static string GetSavedSearchVacanciesSnippet(
        EmailEnvironmentHelper environmentHelper,
        List<PostSendSavedSearchNotificationCommand.Vacancy> vacancies,
        bool hasSearchLocation)
    {
        var sb = new StringBuilder();

        foreach (var vacancy in vacancies)
        {
            string? trainingCourseText;
            string? wageText;
            var employmentWorkLocation = vacancy.EmploymentLocationOption switch
            {
                AvailableWhere.AcrossEngland => EmailTemplateBuilderConstants.RecruitingNationally,
                AvailableWhere.MultipleLocations => EmailTemplateAddressExtension.GetEmploymentLocations(
                    vacancy.OtherAddresses is { Count: > 0 }
                        ? new List<Address> { vacancy.EmployerLocation! }.Concat(vacancy.OtherAddresses).ToList()
                        : [ vacancy.EmployerLocation! ]),
                _ => EmailTemplateAddressExtension.GetOneLocationCityName(vacancy.EmployerLocation)
            };

            sb.AppendLine();

            if (vacancy.VacancySource != null && vacancy.VacancySource.Equals(nameof(VacancyDataSource.Nhs), StringComparison.CurrentCultureIgnoreCase))
            {
                sb.AppendLine($"#[{vacancy.Title} (from NHS Jobs)]({environmentHelper.VacancyDetailsUrl.Replace("{vacancy-reference}", vacancy.VacancyReference)})");
                trainingCourseText = "See more details on NHS Jobs";
                wageText = GetWageText(vacancy.Wage!);
            }
            else
            {
                sb.AppendLine($"#[{vacancy.Title}]({environmentHelper.VacancyDetailsUrl.Replace("{vacancy-reference}", vacancy.VacancyReference)})");
                trainingCourseText = vacancy.TrainingCourse;
                wageText = (vacancy.WageType == "Competitive") ? vacancy.WageType : vacancy.Wage;
            }
            sb.AppendLine(vacancy.EmployerName);
            sb.AppendLine(employmentWorkLocation);

            sb.AppendLine();

            if (vacancy.ApprenticeshipType is ApprenticeshipTypes.Foundation)
            {
                sb.AppendLine("Foundation apprenticeship");
                sb.AppendLine();
            }
            
            if (hasSearchLocation)
            {
                if (vacancy is {Distance: 1})
                {
                    sb.AppendLine($"* Distance: {vacancy.Distance} mile");
                }
                else
                {
                    sb.AppendLine($"* Distance: {vacancy.Distance} miles");
                }

            }

            if (vacancy.VacancySource != null
                && !string.IsNullOrEmpty(vacancy.StartDate)
                && vacancy.VacancySource.Equals(nameof(VacancyDataSource.Raa), StringComparison.CurrentCultureIgnoreCase))
            {
                sb.AppendLine($"* Start date: {vacancy.StartDate}");
            }
            sb.AppendLine($"* Training course: {trainingCourseText}");
            sb.AppendLine($"* Wage: {wageText}");

            sb.AppendLine();
            sb.AppendLine($"{vacancy.ClosingDate}");

            sb.AppendLine();
            sb.AppendLine("---");
        }

        return sb.ToString();
    }

    public static string BuildSearchAlertDescriptor(PostSendSavedSearchNotificationCommand command)
    {
        var definingCharacteristic = command switch
        {
            { SearchTerm: not null } => command.SearchTerm,
            { Categories: { Count: 1 } } => command.Categories[0].Name,
            { Categories: { Count: > 1 } } => $"{command.Categories.Count} categories",
            { ApprenticeshipTypes: { Count: 1 } } => GetApprenticeshipTypeText(command.ApprenticeshipTypes[0]),
            { ApprenticeshipTypes: { Count: > 1 } } => $"{command.ApprenticeshipTypes.Count} apprenticeship types",
            { Levels.Count: 1 } => $"Level {command.Levels[0].Code}",
            { Levels.Count: > 1 } => $"{command.Levels.Count} apprenticeship levels",
            { DisabilityConfident: true } => "Disability Confident",
            { ExcludeNational: true } => "Exclude National",
            _ => "All apprenticeships"
        };

        var location = command.Location is null
            ? "all of England"
            : $"{command.Location}";

        return $"{definingCharacteristic} in {location}";
    }
        
    private static string GetApprenticeshipTypeText(ApprenticeshipTypes apprenticeshipType)
    {
        return apprenticeshipType switch
        {
            ApprenticeshipTypes.Standard => "Apprenticeships",
            ApprenticeshipTypes.Foundation => "Foundation apprenticeships",
            _ => throw new ArgumentOutOfRangeException(nameof(apprenticeshipType), apprenticeshipType, null)
        };
    }

    private static string GetWageText(string wageAmountText)
    {
        if (string.IsNullOrEmpty(wageAmountText)) return $"{wageAmountText}";

        var encodedBytes = Encoding.UTF8.GetBytes(wageAmountText);
        var decoded = Encoding.UTF8.GetString(encodedBytes);

        var poundRemovedStr = decoded
            .Replace("�", string.Empty) // Application env & Pipeline doesn't recognise the Pound Sign
            .Replace("£", string.Empty)
            .Replace("\u00A3", string.Empty) // Unicode for Pound Sign
            .Replace("u+00A3", string.Empty);

        var matches = NhsWageAmountRegex().Matches(wageAmountText);

        if (matches.Count == 2)
        {
            var lowerBound = decimal.Parse(matches[0].Value, CultureInfo.InvariantCulture);
            var upperBound = decimal.Parse(matches[1].Value, CultureInfo.InvariantCulture);

            var lowerBoundText = lowerBound % 1 == 0
                ? string.Format(CultureInfo.InvariantCulture, "£{0:#,##}", lowerBound)
                : string.Format(CultureInfo.InvariantCulture, "£{0:#,##.00}", lowerBound);

            var upperBoundText = upperBound % 1 == 0
                ? string.Format(CultureInfo.InvariantCulture, "£{0:#,##}", upperBound)
                : string.Format(CultureInfo.InvariantCulture, "£{0:#,##.00}", upperBound);

            return upperBound switch
            {
                > NhsUpperWageAmountLimit => $"{lowerBoundText} to {upperBoundText} a year",
                < NhsLowerWageAmountLimit => $"{lowerBoundText} to {upperBoundText} an hour",
                _ => $"{lowerBoundText} to {upperBoundText}",
            };
        }

        if (!decimal.TryParse(poundRemovedStr, out var wageAmount)) return $"{wageAmountText}";

        return wageAmount switch
        {
            < NhsLowerWageAmountLimit => wageAmount % 1 == 0 ? string.Format(CultureInfo.InvariantCulture, "£{0:#,##} an hour", wageAmount) : string.Format(CultureInfo.InvariantCulture, "£{0:#,##.00} an hour", wageAmount),
            > NhsUpperWageAmountLimit => wageAmount % 1 == 0 ? string.Format(CultureInfo.InvariantCulture, "£{0:#,##} a year", wageAmount) : string.Format(CultureInfo.InvariantCulture, "£{0:#,##.00} a year", wageAmount),
            _ => wageAmountText
        };
    }
}