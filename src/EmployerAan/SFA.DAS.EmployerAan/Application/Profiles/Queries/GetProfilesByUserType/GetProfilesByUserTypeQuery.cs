using MediatR;

namespace SFA.DAS.EmployerAan.Application.Profiles.Queries.GetProfilesByUserType;

public class GetProfilesByUserTypeQuery : IRequest<GetProfilesByUserTypeQueryResult>
{
    public GetProfilesByUserTypeQuery(string userType) => UserType = userType;
    public string UserType { get; }
}
