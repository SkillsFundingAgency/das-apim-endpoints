using MediatR;
using RestEase;

namespace SFA.DAS.AdminAan.Application.Schools.Queries;

public class GetSchoolsQuery : IRequest<GetSchoolsQueryApiResult>, IRequest<Response<GetSchoolsQueryResult>>
{
    public string SearchTerm { get; init; }

}
