using System;
using System.Collections.Generic;
using SFA.DAS.FindAnApprenticeship.Application.Queries.WithdrawApplication;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class GetWithdrawnApplicationApiResponse
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
    public AvailableWhere? EmploymentLocationOption { get; set; }

    public static implicit operator GetWithdrawnApplicationApiResponse(WithdrawApplicationQueryResult source)
    {
        return new GetWithdrawnApplicationApiResponse
        {
            ApplicationId = source.ApplicationId,
            ClosingDate = source.ClosingDate,
            ClosedDate = source.ClosedDate,
            EmployerName = source.EmployerName,
            SubmittedDate = source.SubmittedDate,
            AdvertTitle = source.AdvertTitle,
            Address = source.Address,
            OtherAddresses = source.OtherAddresses,
            EmploymentLocationOption = source.EmploymentLocationOption,
            EmploymentLocationInformation = source.EmploymentLocationInformation,
        };
    }
}