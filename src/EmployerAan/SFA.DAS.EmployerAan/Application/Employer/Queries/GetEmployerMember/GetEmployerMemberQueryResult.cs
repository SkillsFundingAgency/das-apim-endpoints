﻿namespace SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMember;

public class GetEmployerMemberQueryResult
{
    public Guid MemberId { get; set; }
    public string Status { get; set; } = null!;
}
