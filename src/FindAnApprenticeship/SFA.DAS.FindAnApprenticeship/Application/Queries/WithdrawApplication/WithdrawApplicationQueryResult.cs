using System;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.WithdrawApplication;

public class WithdrawApplicationQueryResult
{
    public Guid ApplicationId { get; set; }
    public string AdvertTitle { get; set; }
    public string EmployerName { get; set; }
    public DateTime? SubmittedDate { get; set; }
    public DateTime ClosingDate { get; set; }
    public DateTime? ClosedDate { get; set; }
    public Address Address { get; set; }
    public List<Address>? OtherAddresses { get; set; } = [];
    public string? EmploymentLocationInformation { get; set; }
    public AvailableWhere? EmployerLocationOption { get; set; }
    public ApprenticeshipTypes? ApprenticeshipType { get; set; } = ApprenticeshipTypes.Standard;
}