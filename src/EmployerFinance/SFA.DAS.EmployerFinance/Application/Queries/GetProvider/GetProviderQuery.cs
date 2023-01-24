using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetProvider
{
    public class GetProviderQuery : IRequest<GetProviderQueryResult>
    {
        public int Id { get; set; }
    }
}