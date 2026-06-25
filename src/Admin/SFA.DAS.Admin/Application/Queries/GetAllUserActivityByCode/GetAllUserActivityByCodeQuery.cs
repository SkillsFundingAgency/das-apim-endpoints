using MediatR;

namespace SFA.DAS.Admin.Application.Queries.GetAllUserActivityByCode
{
    public class GetAllUserActivityByCodeQuery : IRequest<GetAllUserActivityByCodeQueryResult>
    {
        public string Code { get; set; }
    }
}
