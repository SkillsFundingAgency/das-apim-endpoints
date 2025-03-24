using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.Jobs;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Jobs
{
    public class GetJobRunByIdQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient) : IRequestHandler<GetJobRunByIdQuery, BaseMediatrResponse<GetJobRunByIdQueryResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient = apiClient;       

        public async Task<BaseMediatrResponse<GetJobRunByIdQueryResponse>> Handle(GetJobRunByIdQuery query, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<GetJobRunByIdQueryResponse>();
            response.Success = false;
            try
            {
                var result = await _apiClient.GetWithResponseCode<GetJobRunByIdApiResponse>(new GetJobRunByIdApiRequest(query.Id));
                result.EnsureSuccessStatusCode();

                response.Value = new GetJobRunByIdQueryResponse()
                { 
                    Id = result.Body.Id,
                    EndTime = result.Body.EndTime,
                    JobId = result.Body.JobId,
                    RecordsProcessed = result.Body.RecordsProcessed,
                    StartTime = result.Body.StartTime,
                    Status = result.Body.Status,
                    User = result.Body.User
                };
                response.Success = true;

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
    }
}
