using SFA.DAS.ToolsSupport.Application.Queries;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using SFA.DAS.ToolsSupport.Extensions;

namespace SFA.DAS.ToolsSupport.Mappers;

public interface IPendingChangesMapper
{
    IEnumerable<PendingChange> MapToPendingChanges(GetApprenticeshipPendingUpdatesResponse pendingUpdatesResponse, SupportApprenticeshipDetails apprenticeship);
}

public class PendingChangesMapper : IPendingChangesMapper
{
    public IEnumerable<PendingChange> MapToPendingChanges(GetApprenticeshipPendingUpdatesResponse pendingUpdatesResponse,
        SupportApprenticeshipDetails apprenticeship)
    {
        if (pendingUpdatesResponse?.ApprenticeshipUpdates == null ||
            !pendingUpdatesResponse.ApprenticeshipUpdates.Any())
        {
            yield break;
        }

        var updates = pendingUpdatesResponse.ApprenticeshipUpdates[0];

        if (!string.IsNullOrWhiteSpace(updates.FirstName) || !string.IsNullOrWhiteSpace(updates.LastName))
        {
            yield return new PendingChange
            {
                Name = "Name",
                OriginalValue = $"{apprenticeship.FirstName} {apprenticeship.LastName}",
                NewValue = $"{updates.FirstName} {updates.LastName}"
            };
        }
        if (!string.IsNullOrWhiteSpace(updates.Email))
        {
            yield return new PendingChange
            {
                Name = "Email",
                OriginalValue = apprenticeship.Email,
                NewValue = updates.Email
            };
        }
        if (updates.DateOfBirth.HasValue)
        {
            yield return new PendingChange
            {
                Name = "Date of birth",
                OriginalValue = apprenticeship.DateOfBirth.ToString("dd MMM yyyy"),
                NewValue = updates.DateOfBirth.Value.ToString("dd MMM yyyy")
            };
        }
        if (updates.DeliveryModel != null)
        {
            yield return new PendingChange
            {
                Name = "Apprenticeship delivery model",
                OriginalValue = apprenticeship.DeliveryModel.ToDescription(),
                NewValue = updates.DeliveryModel.Value.ToDescription()
            };
        }
        if (updates.EmploymentEndDate.HasValue)
        {
            yield return new PendingChange
            {
                Name = "Planned end date of this employment",
                OriginalValue = apprenticeship.EmploymentEndDate.HasValue ? apprenticeship.EmploymentEndDate.Value.ToString("dd MMM yyyy") : "Not applicable",
                NewValue = updates.EmploymentEndDate.Value.ToString("dd MMM yyyy")
            };
        }
        if (updates.EmploymentPrice.HasValue)
        {
            yield return new PendingChange
            {
                Name = "Training price for this employment",
                OriginalValue = apprenticeship.EmploymentPrice.HasValue ? $"£{apprenticeship.EmploymentPrice.Value:n0}" : "Not applicable",
                NewValue = $"£{updates.EmploymentPrice.Value:n0}"
            };
        }
        if (!string.IsNullOrWhiteSpace(updates.TrainingCode))
        {
            yield return new PendingChange
            {
                Name = "Apprenticeship training course",
                OriginalValue = apprenticeship.CourseName,
                NewValue = updates.TrainingName
            };
        }
        if (!string.IsNullOrWhiteSpace(updates.Version) || !string.IsNullOrWhiteSpace(updates.TrainingCode))
        {
            yield return new PendingChange
            {
                Name = "Version",
                OriginalValue = !string.IsNullOrEmpty(apprenticeship.TrainingCourseVersion) ? apprenticeship.TrainingCourseVersion : "Not applicable",
                NewValue = !string.IsNullOrEmpty(updates.Version) ? updates.Version : "Not applicable"
            };
        }
        if (!string.IsNullOrWhiteSpace(updates.Option) || !string.IsNullOrWhiteSpace(updates.TrainingCode))
        {
            var originalOption = apprenticeship.TrainingCourseOption switch
            {
                "" => "To be confirmed",
                null => "Not applicable",
                _ => apprenticeship.TrainingCourseOption
            };
            var newOption = updates.Option switch
            {
                "" => "To be confirmed",
                null => "Not applicable",
                _ => updates.Option
            };

            yield return new PendingChange
            {
                Name = "Option",
                OriginalValue = originalOption,
                NewValue = newOption
            };
        }
        if (updates.StartDate.HasValue)
        {
            yield return new PendingChange
            {
                Name = "Planned training start date",
                OriginalValue = apprenticeship.StartDate.ToString("MMM yyyy"),
                NewValue = updates.StartDate.Value.ToString("MMM yyyy")
            };
        }
        if (updates.EndDate.HasValue)
        {
            yield return new PendingChange
            {
                Name = "Planned training end date",
                OriginalValue = apprenticeship.EndDate.ToString("MMM yyyy"),
                NewValue = updates.EndDate.Value.ToString("MMM yyyy")
            };
        }
        if (updates.Cost.HasValue)
        {
            yield return new PendingChange
            {
                Name = "Cost",
                OriginalValue = apprenticeship.Cost.HasValue ? $"£{apprenticeship.Cost.Value:n0}" : "None",
                NewValue = $"£{updates.Cost.Value:n0}"
            };
        }
    }
}

