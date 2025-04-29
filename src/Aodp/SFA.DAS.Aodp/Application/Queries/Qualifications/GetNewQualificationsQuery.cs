using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Qualifications
{
    public class GetNewQualificationsQuery : IRequest<BaseMediatrResponse<GetNewQualificationsQueryResponse>>
    {
        public string? Name { get; set; }
        public string? Organisation { get; set; }
        public string? QAN { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public string? ProcessStatusFilter { get; set; }
    }
}
