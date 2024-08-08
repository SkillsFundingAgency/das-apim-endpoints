using MediatR;

namespace SFA.DAS.ProviderPR.Application.Queries.GetEasUserByEmail;
public class GetEasUserByEmailQuery : IRequest<GetEasUserByEmailQueryResult>
{
    public long Ukprn { get; }
    public string Email { get; }

    public GetEasUserByEmailQuery(string email, long ukprn)
    {
        Ukprn = ukprn;
        Email = email;
    }
}
