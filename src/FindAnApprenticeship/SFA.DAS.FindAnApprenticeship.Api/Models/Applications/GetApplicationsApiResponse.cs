using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplications;
using SFA.DAS.FindAnApprenticeship.Domain.Models;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class GetApplicationsApiResponse
{
    public List<Application> Applications { get; set; } = [];
    
    public static GetApplicationsApiResponse From(GetApplicationsQueryResult source)
    {
        var result = new GetApplicationsApiResponse
        {
            Applications = source.Applications.Select(Application.From).ToList()
        };

        return result;
    }

    public record Application(
        Guid Id,
        string Title,
        string VacancyReference,
        string EmployerName,
        DateTime CreatedDate,
        DateTime? SubmittedDate,
        DateTime ClosingDate,
        DateTime? ClosedDate,
        DateTime? ResponseDate,
        string? ResponseNotes,
        ApplicationStatus Status,
        DateTime? WithdrawnDate
    )
    {
        public static Application From(GetApplicationsQueryResult.Application source)
        {
            return new Application(
                source.Id,
                source.Title,
                source.VacancyReference,
                source.EmployerName,
                source.CreatedDate,
                source.SubmittedDate,
                source.ClosingDate,
                source.ClosedDate,
                source.ResponseDate,
                source.ResponseNotes,
                source.Status,
                source.WithdrawnDate
            );
        }
    }
}