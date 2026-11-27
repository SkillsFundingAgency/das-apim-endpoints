using System;

namespace SFA.DAS.EmployerFinance.InnerApi.Responses;

public sealed record GetImportJobStateResponse
{
    public DateTime LastSuccessfulImportDate { get; init; }
}
