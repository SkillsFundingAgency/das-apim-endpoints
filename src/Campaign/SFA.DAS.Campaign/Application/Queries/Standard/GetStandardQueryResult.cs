using MediatR;

namespace SFA.DAS.Campaign.Application.Queries.Standard
{
    public class GetStandardQueryResult : IRequest<GetStandardQuery>
    {
        public Standard Standard { get; set; }
    }

    public class Standard
    {
        public string Title { get; set; }
        public int Level { get; set; }
        public int TimeToComplete { get; set; }
        public string StandardUId { get; set; }
        public int LarsCode { get; set; }
        public int MaxFunding { get; set; }
    }
}
