using MediatR;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseByLarsCode;

public class GetCourseByLarsCodeQuery : IRequest<GetCourseByLarsCodeQueryResult>
{
    public int LarsCode { get; set; }

    public decimal? Lon { get; set; }

    public decimal? Lat { get; set; }

    public int? Distance { get; set; }
}