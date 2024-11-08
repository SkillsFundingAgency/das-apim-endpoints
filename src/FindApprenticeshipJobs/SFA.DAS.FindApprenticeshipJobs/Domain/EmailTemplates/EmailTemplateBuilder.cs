using System.Text;
using SFA.DAS.FindApprenticeshipJobs.Application.Commands.SavedSearch.SendNotification;

namespace SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates
{
    public static class EmailTemplateBuilder
    {
        public static string GetSavedSearchSearchParams(
            string? searchTerm,
            decimal? distance,
            string? location,
            List<string?>? categories,
            List<string?>? levels,
            bool? disabilityConfident)
        {
            var sb = new StringBuilder();

            sb.AppendLine();
            if (!string.IsNullOrEmpty(searchTerm)) sb.AppendLine($"What: {searchTerm}");
            if (!string.IsNullOrEmpty(location)) sb.AppendLine($"Where: {location} (within {distance} miles)");
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
            bool? disabilityConfident)
        {
            var queryParameters = string.Empty;

            if (!string.IsNullOrEmpty(searchTerm)) queryParameters += $"&searchTerm={searchTerm}";
            if (!string.IsNullOrEmpty(location)) queryParameters += $"&location={location}";
            if (distance > 0) queryParameters += $"&distance={distance}";
            if (categoryIds is { Count: > 0 }) queryParameters += "&routeIds=" + string.Join("&routeIds=", categoryIds);
            if (levelCodes is { Count: > 0 }) queryParameters += "&levelIds=" + string.Join("&levelIds=", levelCodes);
            if (disabilityConfident != null && disabilityConfident.Value) queryParameters += "&DisabilityConfident=true";

            return queryParameters;
        }

        public static string GetSavedSearchVacanciesSnippet(
            EmailEnvironmentHelper environmentHelper,
            List<PostSendSavedSearchNotificationCommand.Vacancy> vacancies)
        {
            var sb = new StringBuilder();

            foreach (var vacancy in vacancies)
            {
                sb.AppendLine();
                sb.AppendLine($"#[{vacancy.Title}]({environmentHelper.VacancyDetailsUrl.Replace("{vacancy-reference}", vacancy.VacancyReference)})");
                sb.AppendLine(vacancy.EmployerName);
                sb.AppendLine(!string.IsNullOrEmpty(vacancy.Address.AddressLine4) ? $"{vacancy.Address.AddressLine4}, {vacancy.Address.Postcode}" :
                    !string.IsNullOrEmpty(vacancy.Address.AddressLine3) ? $"{vacancy.Address.AddressLine3}, {vacancy.Address.Postcode}" :
                    !string.IsNullOrEmpty(vacancy.Address.AddressLine2) ? $"{vacancy.Address.AddressLine2}, {vacancy.Address.Postcode}" :
                    !string.IsNullOrEmpty(vacancy.Address.AddressLine1) ? $"{vacancy.Address.AddressLine1}, {vacancy.Address.Postcode}" :
                    vacancy.Address.Postcode);

                sb.AppendLine();
                sb.AppendLine($"* Distance: {vacancy.Distance} miles");
                sb.AppendLine($"* Training course: {vacancy.TrainingCourse}");
                sb.AppendLine($"* Annual wage: {vacancy.Wage}");

                sb.AppendLine();
                sb.AppendLine($"{vacancy.ClosingDate}");

                sb.AppendLine();
                sb.AppendLine("---");
            }

            return sb.ToString();
        }
    }
}