using SFA.DAS.FindAnApprenticeship.Application.Queries.WithdrawApplication;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Collections.Generic;

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
    public AvailableWhere? EmployerLocationOption { get; set; }
    public ApprenticeshipTypes ApprenticeshipType { get; set; } = ApprenticeshipTypes.Standard;

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
            EmployerLocationOption = source.EmployerLocationOption,
            EmploymentLocationInformation = source.EmploymentLocationInformation,
            ApprenticeshipType = source.ApprenticeshipType ?? ApprenticeshipTypes.Standard
        };
    }
}