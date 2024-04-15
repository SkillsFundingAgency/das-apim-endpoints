﻿namespace SFA.DAS.Apprenticeships.Responses;
public class ApprenticeshipStartDateResponse
{
    public Guid ApprenticeshipKey { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public DateTime? PlannedEndDate { get; set; }
    public string? EmployerName { get; set; }
    public string? ProviderName { get; set; }
}