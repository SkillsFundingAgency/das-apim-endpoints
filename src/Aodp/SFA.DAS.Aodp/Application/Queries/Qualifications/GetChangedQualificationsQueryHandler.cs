using MediatR;
using SFA.DAS.Aodp.Application.Queries.Jobs;
using SFA.DAS.Aodp.InnerApi.AodpApi.Jobs;
using SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Qualifications
{
    public class GetChangedQualificationsQueryHandler : IRequestHandler<GetChangedQualificationsQuery, BaseMediatrResponse<GetChangedQualificationsQueryResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

        public GetChangedQualificationsQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
{
            _apiClient = apiClient;
        }

        public async Task<BaseMediatrResponse<GetChangedQualificationsQueryResponse>> Handle(GetChangedQualificationsQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetChangedQualificationsQueryResponse>();
        response.Success = false;

        try
        {
                var qualificationsResponse = await _apiClient.Get<GetChangedQualificationsApiResponse>(new GetChangedQualificationsApiRequest()
                {
                    Skip = request.Skip,
                    Take = request.Take,
                    Name = request.Name,
                    Organisation = request.Organisation,
                    QAN = request.QAN,
                    ProcessStatusFilter = request.ProcessStatusFilter
                });

                if (qualificationsResponse?.Data != null)
            {
                    response.Value.TotalRecords = qualificationsResponse.TotalRecords;
                    response.Value.Take = qualificationsResponse.Take;
                    response.Value.Skip = qualificationsResponse.Skip;
                    response.Value.Data = qualificationsResponse.Data;
                response.Success = true;
            }

                var jobResponse = await _apiClient.Get<GetJobByNameQueryResponse>(new GetJobByNameApiRequest("RegulatedQualifications"));
                if (jobResponse != null)
            {
                    response.Value.Job.Status = jobResponse.Status;
                    response.Value.Job.LastRunTime = jobResponse.LastRunTime;
                    response.Value.Job.Name = jobResponse.Name;
            }

        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
        }

        return response;
    }
    }
}
