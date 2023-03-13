using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerFinance.InnerApi.Requests;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderCourses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetProvider
{
    public class GetProviderQueryHandler : IRequestHandler<GetProviderQuery, GetProviderQueryResult>
    {
        private readonly IProviderCoursesApiClient<ProviderCoursesApiConfiguration> _roatpServiceApiClient;

        public GetProviderQueryHandler (IProviderCoursesApiClient<ProviderCoursesApiConfiguration> roatpServiceApiClient)
        {
            _roatpServiceApiClient = roatpServiceApiClient;
        }
        public async Task<GetProviderQueryResult> Handle(GetProviderQuery request, CancellationToken cancellationToken)
        {

            var response = await _roatpServiceApiClient.Get<GetProviderResponse>(
                new SharedOuterApi.InnerApi.Requests.ProviderCourses.GetProviderRequest(request.Id));

            return new GetProviderQueryResult
            {
                Ukprn = response.Ukprn,
                Name = response.Name,
                ContactUrl = response.ContactUrl,
                Email = response.Email,
                Phone = response.Phone
            };
        }
    }
}