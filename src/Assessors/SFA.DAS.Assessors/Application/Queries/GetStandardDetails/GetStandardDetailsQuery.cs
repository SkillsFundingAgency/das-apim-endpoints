using MediatR;

namespace SFA.DAS.Assessors.Application.Queries.GetStandardDetails
{
    public class GetStandardDetailsQuery : IRequest<GetStandardDetailsResult>
    {
        public string Id { get; }
        public GetStandardDetailsQuery(string id)
        {
            Id = id;
        }
    }
}
