using MediatR;

namespace SFA.DAS.AODP.Application.Queries.Test
{
    public class TestQuery : IRequest<TestQueryResponse>
    {
        public int Id { get; set; }
    }
}
