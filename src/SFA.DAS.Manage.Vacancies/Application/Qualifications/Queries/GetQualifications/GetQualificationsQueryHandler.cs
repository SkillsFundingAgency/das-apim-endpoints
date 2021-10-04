using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Vacancies.Manage.Configuration;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;
using SFA.DAS.Vacancies.Manage.InnerApi.Responses;
using SFA.DAS.Vacancies.Manage.Interfaces;

namespace SFA.DAS.Vacancies.Manage.Application.Qualifications.Queries.GetQualifications
{
    public class GetQualificationsQueryHandler : IRequestHandler<GetQualificationsQuery, GetQualificationsQueryResponse>
    {
        private readonly IQualificationsApiClient<QualificationsApiConfiguration> _apiClient;

        public GetQualificationsQueryHandler (IQualificationsApiClient<QualificationsApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<GetQualificationsQueryResponse> Handle(GetQualificationsQuery request, CancellationToken cancellationToken)
        {
            var response =
                await _apiClient.Get<GetQualificationsResponse>(
                    new GetQualificationsRequest(request.Ukprn));

            return new GetQualificationsQueryResponse
            {
                Qualifications = response.Qualifications
            };
        }
    }
}