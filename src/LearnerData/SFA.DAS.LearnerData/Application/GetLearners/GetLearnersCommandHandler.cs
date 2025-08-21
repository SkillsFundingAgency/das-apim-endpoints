using MediatR;
using SFA.DAS.LearnerData.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.Application.GetLearners;

public class GetLearnersQuery : PagedQuery, IRequest<GetLearnersQueryResult>
{
    public string Ukprn { get; set; } = "";
    public int AcademicYear { get; set; }
}

public class GetLearnersQueryHandler(
    ILearningApiClient<LearningApiConfiguration> LearnerApiClient)
    : IRequestHandler<GetLearnersQuery, GetLearnersQueryResult>
{
    public async Task<GetLearnersQueryResult> Handle(GetLearnersQuery request, CancellationToken cancellationToken)
    {
        LearnerApiClient.GenerateServiceToken("LearnersManage");

        var applicationsResponse = await LearnerApiClient.Get<GetLearnersQueryResult>(
            new GetAllLearningsRequest(
                request.Ukprn,
                request.AcademicYear,
                request.Page,
                request.PageSize
                )
            );

        return applicationsResponse;
    }
}

public class GetLearnersQueryResult : PagedQueryResult<Learning>
{

}

public class Learning
{
    public string Uln { get; set; } = "";
    public Guid Key { get; set; }
}


public class GetLearnersResponse
{
    public List<Learning> Learners { get; set; } = [];
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }

    public static implicit operator GetLearnersResponse(GetLearnersQueryResult source)
    {
        if (source == null) return new GetLearnersResponse();

        return new GetLearnersResponse()
        {
            Total = source.TotalItems,
            Page = source.Page,
            PageSize = source.PageSize,
            TotalPages = source.TotalPages,
            Learners = source.Items
        };
    }
}