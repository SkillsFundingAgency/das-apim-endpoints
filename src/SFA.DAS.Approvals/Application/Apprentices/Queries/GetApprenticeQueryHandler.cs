using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries
{
    public class GetApprenticeQueryHandler : IRequestHandler<GetApprenticeQuery, GetApprenticeResult>
    {
        private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _apiClient;

        public GetApprenticeQueryHandler(IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<GetApprenticeResult> Handle(GetApprenticeQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.Get<GetApprenticeResponse>(new GetApprenticeRequest(request.ApprenticeId));

            if (result == null) 
                return null;

            return new GetApprenticeResult
            {
                Id = result.Id,
                FirstName = result.FirstName,
                LastName = result.LastName,
                DateOfBirth = result.DateOfBirth,
                Email = result.Email
            };
        }
    }
}