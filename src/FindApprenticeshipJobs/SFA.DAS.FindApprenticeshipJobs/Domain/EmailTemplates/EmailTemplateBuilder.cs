using SFA.DAS.FindApprenticeshipJobs.Application.Commands.SavedSearch.SendNotification;
using System.Text;

namespace SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates
{
    public class EmailTemplateBuilder()
    {
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
                sb.AppendLine(vacancy.Address.AddressLine1);

                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("<ul>");
                sb.AppendLine($"<li>Distance: {vacancy.Distance} miles</li>");
                sb.AppendLine($"<li>Training course: {vacancy.TrainingCourse}</li>");
                sb.AppendLine($"<li>Annual wage: {vacancy.Wage}</li>");
                sb.AppendLine("</ul>");

                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine($"{vacancy.ClosingDate}");

                sb.AppendLine();
                sb.AppendLine("<hr/>");
            }

            return sb.ToString();
        }
    }
}
