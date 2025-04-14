using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SFA.DAS.Approvals.Application.Apprentices.Queries.GetApprenticeshipsCSV;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Learners.Queries
{
    public class GetLearnersForProviderQueryHandler(
        ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient, IMapper mapper)
        : IRequestHandler<GetApprenticeshipsCSVQuery, GetApprenticeshipsCSVQueryResult>
    {
        public async Task<GetApprenticeshipsCSVQueryResult> Handle(GetApprenticeshipsCSVQuery request, CancellationToken cancellationToken)
        {
            var apprenticeshipResponse = await apiClient.GetWithResponseCode<GetApprenticeshipsResponse>(
                new GetApprenticeshipsCSVRequest(
                    request.ProviderId,
                    request.SearchTerm,
                    request.EmployerName,
                    request.CourseName,
                    request.Status,
                    request.StartDate,
                    request.EndDate,
                    request.Alert,
                    request.ApprenticeConfirmationStatus,
                    request.DeliveryModel
                    ));

            if (apprenticeshipResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            return mapper.Map<GetApprenticeshipsCSVQueryResult>(apprenticeshipResponse.Body);
        }
    }
}
