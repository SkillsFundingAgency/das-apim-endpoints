using System;
using SFA.DAS.FindAnApprenticeship.Application.Queries.WithdrawApplication;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class GetWithdrawnApplicationApiResponse
{
    public Guid ApplicationId { get; set; }
    public string AdvertTitle { get; set; }
    public string EmployerName { get; set; }
    public DateTime SubmittedDate { get; set; }
    public DateTime ClosingDate { get; set; }
    
    public static implicit operator GetWithdrawnApplicationApiResponse(WithdrawApplicationQueryResult source)
    {
        return new GetWithdrawnApplicationApiResponse
        {
            ApplicationId = source.ApplicationId,
            ClosingDate = source.ClosingDate,
            EmployerName = source.EmployerName,
            SubmittedDate = source.SubmittedDate,
            AdvertTitle = source.AdvertTitle
        };
    }
}