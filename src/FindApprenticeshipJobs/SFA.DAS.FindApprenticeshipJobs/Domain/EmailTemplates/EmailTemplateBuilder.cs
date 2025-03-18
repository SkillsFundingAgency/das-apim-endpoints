using SFA.DAS.FindApprenticeshipJobs.Application.Commands.SavedSearch.SendNotification;
using SFA.DAS.FindApprenticeshipJobs.Application.Shared;
using SFA.DAS.FindApprenticeshipJobs.Domain.Constants;
using SFA.DAS.SharedOuterApi.Extensions;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates
{
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
            List<string?>? categories,
            List<string?>? levels,
            bool? disabilityConfident,
            bool? excludeNational)
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
            
            
            if (categories is { Count: > 0 }) sb.AppendLine($"Categories: {string.Join(", ", categories)}");
            if (levels is { Count: > 0 }) sb.AppendLine($"Apprenticeship levels: {string.Join(", ", levels)}");
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
            bool? excludeNational)
        {
            var queryParameters = string.Empty;

            if (!string.IsNullOrEmpty(searchTerm)) queryParameters += $"&searchTerm={searchTerm}";
            if (!string.IsNullOrEmpty(location)) queryParameters += $"&location={location}";
            if (distance > 0) queryParameters += $"&distance={distance}";
            if (categoryIds is { Count: > 0 }) queryParameters += "&routeIds=" + string.Join("&routeIds=", categoryIds);
            if (levelCodes is { Count: > 0 }) queryParameters += "&levelIds=" + string.Join("&levelIds=", levelCodes);
            if (disabilityConfident != null && disabilityConfident.Value) queryParameters += "&DisabilityConfident=true";
            if (excludeNational != null && excludeNational.Value) queryParameters += "&excludeNational=true";

            return queryParameters;
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
                    AvailableWhere.OneLocation => EmailTemplateAddressExtension.GetOneLocationCityName(vacancy.EmployerLocation),
                    _ => EmailTemplateAddressExtension.GetOneLocationCityName(vacancy.EmployerLocation)
                };

                sb.AppendLine();

                if (vacancy.VacancySource != null && vacancy.VacancySource.Equals("NHS", StringComparison.CurrentCultureIgnoreCase))
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
                { Categories: { Count: 1 } } => command.Categories.First().Name,
                { Categories: { Count: > 1 } } => $"{command.Categories.Count} categories",
                { Levels.Count: 1 } => $"Level {command.Levels.First().Code}",
                { Levels.Count: > 1 } => $"{command.Levels.Count} apprenticeship levels",
                { DisabilityConfident: true } => "Disability Confident",
                _ => "All apprenticeships"
            };

            var location = command.Location is null
                ? "all of England"
                : $"{command.Location}";

            return $"{definingCharacteristic} in {location}";
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
}