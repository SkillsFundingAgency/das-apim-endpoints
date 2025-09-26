using System;
using System.Collections.Generic;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindAnApprenticeship.Services;

public interface IVacancy
{
    string VacancyReference { get; }
    public string EmployerName { get; }
    public string Title { get; }
    public DateTime ClosingDate { get; }
    public DateTime? ClosedDate { get;}
    int CourseId { get; }
    string AdditionalQuestion1 { get; }
    string AdditionalQuestion2 { get; }
    bool IsDisabilityConfident { get; }
    string City { get; }
    string Postcode { get; }
    bool IsExternalVacancy { get; }
    string ExternalVacancyUrl { get; }
    VacancyDataSource VacancySource { get; }

    public Address? Address { get; }
    public List<Address>? OtherAddresses { get; } 
    public string? EmploymentLocationInformation { get; }
    public AvailableWhere? EmployerLocationOption { get; }
    public ApprenticeshipTypes ApprenticeshipType { get; }
}