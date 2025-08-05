using System;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.DeleteApplication;

public record ConfirmDeleteApplicationQueryResult
{
    public static readonly ConfirmDeleteApplicationQueryResult None = new() { ApplicationId = Guid.Empty };
    
    public Address Address { get; set; }
    public Guid ApplicationId { get; init; }
    public DateTime? ApplicationStartDate { get; init; }
    public ApprenticeshipTypes? ApprenticeshipType { get; init; }
    public AvailableWhere? EmployerLocationOption { get; set; }
    public string EmployerName { get; init; }
    public List<Address>? OtherAddresses { get; set; } = [];
    public DateTime? VacancyClosedDate { get; init; }
    public DateTime? VacancyClosingDate { get; init; }
    public string VacancyTitle { get; init; }
}