using MediatR;
using SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor;
using SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetFrameworkLearner
{
    public class GetFrameworkLearnerQueryHandler : IRequestHandler<GetFrameworkLearnerQuery, GetFrameworkLearnerQueryResult>
    {
        private readonly IAssessorsApiClient<AssessorsApiConfiguration> _assessorsApiClient;

        public GetFrameworkLearnerQueryHandler(IAssessorsApiClient<AssessorsApiConfiguration> assessorsApiClient)
        {
            _assessorsApiClient = assessorsApiClient;
        }

        public async Task<GetFrameworkLearnerQueryResult> Handle(GetFrameworkLearnerQuery request, CancellationToken cancellationToken)
        {
            var response = await _assessorsApiClient.GetWithResponseCode<GetFrameworkLearnerResponse>(new GetFrameworkLearnerRequest(request.FrameworkLearnerId));

            if (response == null || response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            var body = response.Body;

            return body;
        }
    }
}
