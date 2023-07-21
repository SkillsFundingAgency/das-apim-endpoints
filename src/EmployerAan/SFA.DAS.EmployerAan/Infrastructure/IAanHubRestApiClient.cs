﻿using RestEase;
using SFA.DAS.EmployerAan.Application.Employer.Commands.CreateEmployerMember;
using SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMember;
using SFA.DAS.EmployerAan.Application.InnerApi.Attendances;
using SFA.DAS.EmployerAan.Application.Profiles.Queries.GetProfilesByUserType;
using SFA.DAS.EmployerAan.Application.Regions.Queries.GetRegions;
namespace SFA.DAS.EmployerAan.Infrastructure;

public interface IAanHubRestApiClient
{
    public const string RequestedByMemberId = Constants.ApiHeaders.RequestedByMemberIdHeader;

    [Get("/profiles/{userType}")]
    Task<GetProfilesByUserTypeQueryResult> GetProfiles([Path] string userType, CancellationToken cancellationToken);

    [Get("/regions")]
    Task<GetRegionsQueryResult> GetRegions(CancellationToken cancellationToken);

    [Get("/employers/{userRef}")]
    [AllowAnyStatusCode]
    Task<Response<GetEmployerMemberQueryResult?>> GetEmployer([Path] Guid userRef, CancellationToken cancellationToken);
    
    [Post("/employers")]
    Task<CreateEmployerMemberCommandResult> PostEmployerMember([Body] CreateEmployerMemberCommand command, CancellationToken cancellationToken);

    [Get("attendances")]
    Task<GetAttendancesResponse> GetAttendances(
        [Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
        string FromDate,
        string ToDate,
        CancellationToken cancellationToken);
}
