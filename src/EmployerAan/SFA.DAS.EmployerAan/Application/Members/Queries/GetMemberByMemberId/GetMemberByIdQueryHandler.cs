using MediatR;
using SFA.DAS.EmployerAan.Infrastructure;
namespace SFA.DAS.EmployerAan.Application.Members.Queries.GetMemberByMemberId;

public class GetMemberByIdQueryHandler : IRequestHandler<GetMemberByIdQuery, GetMemberByIdQueryResult?>
{
    private readonly IAanHubRestApiClient _apiClient;

    public GetMemberByIdQueryHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetMemberByIdQueryResult?> Handle(GetMemberByIdQuery request,
        CancellationToken cancellationToken)
    {
        var member = await _apiClient.GetMember(request.MemberId, cancellationToken);

        return new GetMemberByIdQueryResult
        {
            MemberId = member.MemberId,
            OrganisationName = member.OrganisationName,
            FirstName = member.FirstName,
            LastName = member.LastName,
            Email = member.Email,
            EmployerAccountId = member.Employer!.AccountId,
            UserRef = member.Employer.UserRef,
            FullName = member.FullName,
            IsRegionalChair = member.IsRegionalChair,
            RegionId = member.RegionId,
            UserType = member.UserType
        };
    }
}