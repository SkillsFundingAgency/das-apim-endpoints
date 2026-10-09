using System;

namespace SFA.DAS.EmployerFinance.InnerApi.Responses.Finance;

public sealed record GetLevyLastSubmissionDateResponse
{
    public DateTime? LastSubmissionDate { get; init; }
}