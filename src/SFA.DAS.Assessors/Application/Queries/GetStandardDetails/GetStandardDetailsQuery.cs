using MediatR;

namespace SFA.DAS.Assessors.Application.Queries.GetStandardDetails
{
    public class GetStandardDetailsQuery : IRequest<GetStandardDetailsResult>
    {
        public string StandardUId { get; }
        public GetStandardDetailsQuery(string standardUId)
        {
            StandardUId = standardUId;
        }
    }
}
