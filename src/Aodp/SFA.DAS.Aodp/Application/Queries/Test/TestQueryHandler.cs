using MediatR;

namespace SFA.DAS.AODP.Application.Queries.Test
{
    public class TestQueryHandler : IRequestHandler<TestQuery, TestQueryResponse>
    {
        public Task<TestQueryResponse> Handle(TestQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
