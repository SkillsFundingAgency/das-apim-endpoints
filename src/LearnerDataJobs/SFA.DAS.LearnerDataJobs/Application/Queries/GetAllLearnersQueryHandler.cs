using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerDataJobs.Responses;

namespace SFA.DAS.LearnerDataJobs.Application.Queries;

public class GetAllLearnersQueryHandler(ILogger<GetAllLearnersQueryHandler> logger)
    : IRequestHandler<GetAllLearnersQuery, GetAllLearnersResponse>
{
    public async Task<GetAllLearnersResponse> Handle(GetAllLearnersQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetAllLearnersQuery for page {Page}, pageSize {PageSize}, excludeApproved {ExcludeApproved}", 
            request.Page, request.PageSize, request.ExcludeApproved);
        
        return new GetAllLearnersResponse
        {
            Page = request.Page,
            PageSize = request.PageSize ?? 100,
            TotalCount = 0,
            TotalPages = 0,
            Data = new List<LearnerDataApiResponse>()
        };
    }
}
