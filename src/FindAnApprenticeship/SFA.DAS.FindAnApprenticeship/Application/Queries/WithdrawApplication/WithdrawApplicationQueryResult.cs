using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.WithdrawApplication;

public class WithdrawApplicationQueryResult
{
    public Guid ApplicationId { get; set; }
    public string AdvertTitle { get; set; }
    public string EmployerName { get; set; }
    public DateTime SubmittedDate { get; set; }
    public DateTime ClosingDate { get; set; }
}