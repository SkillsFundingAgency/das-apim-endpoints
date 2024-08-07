using MediatR;

namespace SFA.DAS.ApprenticeApp.Application.Queries.CourseOptionKsbs
{
    public class GetStandardOptionKsbsQuery : IRequest<GetStandardOptionKsbsQueryResult>
    {
       public string Id { get; set; }
       public string Option { get; set; }
    }
}
