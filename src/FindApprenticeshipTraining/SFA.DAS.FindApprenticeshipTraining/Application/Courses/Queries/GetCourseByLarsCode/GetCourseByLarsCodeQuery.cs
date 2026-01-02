using MediatR;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseByLarsCode;

public class GetCourseByLarsCodeQuery : IRequest<GetCourseByLarsCodeQueryResult>
{
    public string LarsCode { get; set; }

    public string Location { get; set; }

    public int? Distance { get; set; }
}