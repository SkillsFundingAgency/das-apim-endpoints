﻿namespace SFA.DAS.ApprenticeAan.Application.Employer.Queries.GetEmployerMemberSummary;

public class GetEmployerMemberSummaryQueryResult
{
    public int ActiveCount { get; set; }
    public IEnumerable<string> Sectors { get; set; } = Enumerable.Empty<string>();
}
