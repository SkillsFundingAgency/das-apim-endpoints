using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.FundingOffer
{
    public class GetFundingOffersQueryHandler : IRequestHandler<GetFundingOffersQuery, BaseMediatrResponse<GetFundingOffersQueryResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

        public GetFundingOffersQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<BaseMediatrResponse<GetFundingOffersQueryResponse>> Handle(GetFundingOffersQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<GetFundingOffersQueryResponse>();
            response.Success = false;
            try
            {
                var result = await _apiClient.GetWithResponseCode<GetFundingOffersQueryResponse>(new GetFundingOffersApiRequest());

                result.EnsureSuccessStatusCode();
                response.Value = result.Body;
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

