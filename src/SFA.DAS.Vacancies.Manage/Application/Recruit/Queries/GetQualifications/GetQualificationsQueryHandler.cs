using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Vacancies.Manage.Configuration;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;
using SFA.DAS.Vacancies.Manage.Interfaces;

namespace SFA.DAS.Vacancies.Manage.Application.Recruit.Queries.GetQualifications
{
    public class GetQualificationsQueryHandler : IRequestHandler<GetQualificationsQuery, GetQualificationsQueryResponse>
    {
        private readonly IRecruitApiClient<RecruitApiConfiguration> _apiClient;

        public GetQualificationsQueryHandler (IRecruitApiClient<RecruitApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<GetQualificationsQueryResponse> Handle(GetQualificationsQuery request, CancellationToken cancellationToken)
        {
            var response =
                await _apiClient.Get<List<string>>(new GetQualificationsRequest());

            return new GetQualificationsQueryResponse
            {
                Qualifications = response
            };
        }
    }
}