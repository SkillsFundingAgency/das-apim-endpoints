using MediatR;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetUkrlp;
public record GetUkrlpQuery(int Ukprn) : IRequest<GetUkrlpQueryResult>;