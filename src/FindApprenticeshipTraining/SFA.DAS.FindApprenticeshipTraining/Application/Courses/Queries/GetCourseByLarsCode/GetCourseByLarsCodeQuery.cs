using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseByLarsCode;

public class GetCourseByLarsCodeQuery : IRequest<GetCourseByLarsCodeQueryResult>
{
    [FromRoute]
    public int LarsCode { get; set; }

    [FromQuery]
    public decimal? Lon { get; set; }

    [FromQuery]
    public decimal? Lat { get; set; }

    [FromQuery]
    public int? Distance { get; set; }
}