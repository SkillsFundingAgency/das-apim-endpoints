using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerDataJobs.InnerApi;
using SFA.DAS.LearnerDataJobs.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerDataJobs.Application.Queries;

public class GetAllLearnersQueryHandler(
    IInternalApiClient<LearnerDataInnerApiConfiguration> client,
    ILogger<GetAllLearnersQueryHandler> logger)
    : IRequestHandler<GetAllLearnersQuery, GetAllLearnersResponse>
{
    public async Task<GetAllLearnersResponse> Handle(GetAllLearnersQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetAllLearnersQuery for page {Page}, pageSize {PageSize}, excludeApproved {ExcludeApproved}", 
            request.Page, request.PageSize, request.ExcludeApproved);

        try
        {
            logger.LogInformation("Building GET request to retrieve all learners");
            var apiRequest = new GetAllLearnersRequest(request.Page, request.PageSize, request.ExcludeApproved);

            logger.LogInformation("Calling inner API to retrieve all learners");
            var response = await client.Get<GetAllLearnersResponse>(apiRequest);

            if (response == null)
            {
                logger.LogWarning("Received null response from LearnerData inner API");
                return new GetAllLearnersResponse
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    TotalItems = 0,
                    TotalPages = 0,
                    Data = new List<LearnerDataApiResponse>()
                };
            }

            logger.LogInformation("Successfully retrieved learners from inner API - page: {Page}, totalItems: {TotalItems}", 
                response.Page, response.TotalItems);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve learners from inner API for page {Page}, pageSize {PageSize}, excludeApproved {ExcludeApproved}", 
                request.Page, request.PageSize, request.ExcludeApproved);
            
            return new GetAllLearnersResponse
            {
                Page = request.Page,
                PageSize = request.PageSize,
                TotalItems = 0,
                TotalPages = 0,
                Data = new List<LearnerDataApiResponse>()
            };
        }
    }
}
