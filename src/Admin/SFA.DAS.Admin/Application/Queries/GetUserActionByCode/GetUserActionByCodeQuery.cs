using MediatR;

namespace SFA.DAS.Admin.Application.Queries.GetUserActionByCode
{
    public class GetUserActionByCodeQuery : IRequest<GetUserActionByCodeQueryResult>
    {
        public string Code { get; set; }
    }
}
