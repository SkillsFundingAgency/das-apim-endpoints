using MediatR;
using SFA.DAS.LearnerData.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.Application.GetShortCourseLearners;

public class GetShortCourseLearnersQuery : PagedQuery, IRequest<GetShortCourseLearnersQueryResult>
{
    public string Ukprn { get; set; } = "";
    public int AcademicYear { get; set; }
}

public class GetShortCourseLearnersQueryResult : PagedQueryResult<Learner>{}

public class Learner
{
    public string Uln { get; set; } = "";
    public Guid Key { get; set; }
}

public class GetShortCourseLearnersQueryHandler(
    ILearningApiClient<LearningApiConfiguration> learnerApiClient)
    : IRequestHandler<GetShortCourseLearnersQuery, GetShortCourseLearnersQueryResult>
{
    public async Task<GetShortCourseLearnersQueryResult> Handle(GetShortCourseLearnersQuery request, CancellationToken cancellationToken)
    {
        var applicationsResponse = await learnerApiClient.Get<GetShortCourseLearnersQueryResult>(
            new GetAllShortCourseLearningsRequest(
                request.Ukprn,
                request.AcademicYear,
                request.Page,
                request.PageSize
                )
            );

        return applicationsResponse;
    }
}

public class GetShortCourseLearnersResponse
{
    public List<Learner> Learners { get; set; } = [];
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }

    public static implicit operator GetShortCourseLearnersResponse(GetShortCourseLearnersQueryResult source)
    {
        if (source == null) return new GetShortCourseLearnersResponse();

        return new GetShortCourseLearnersResponse()
        {
            Total = source.TotalItems,
            Page = source.Page,
            PageSize = source.PageSize,
            TotalPages = source.TotalPages,
            Learners = source.Items
        };
    }
}