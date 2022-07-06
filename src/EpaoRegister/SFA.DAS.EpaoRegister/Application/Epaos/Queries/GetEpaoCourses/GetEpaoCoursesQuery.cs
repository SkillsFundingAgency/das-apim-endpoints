using MediatR;

namespace SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpaoCourses
{
    public class GetEpaoCoursesQuery : IRequest<GetEpaoCoursesResult>
    {
        public string EpaoId { get; set; }
    }
}