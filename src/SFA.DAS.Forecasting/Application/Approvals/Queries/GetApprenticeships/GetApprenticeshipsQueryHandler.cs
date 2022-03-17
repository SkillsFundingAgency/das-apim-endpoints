using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Forecasting.InnerApi.Requests;
using SFA.DAS.Forecasting.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Forecasting.Application.Approvals.Queries.GetApprenticeships
{
    public class GetApprenticeshipsQueryHandler : IRequestHandler<GetApprenticeshipsQuery, GetApprenticeshipsQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsV2Api;

        public GetApprenticeshipsQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2Api)
        {
            _commitmentsV2Api = commitmentsV2Api;
        }

        public async Task<GetApprenticeshipsQueryResult> Handle(GetApprenticeshipsQuery request, CancellationToken cancellationToken)
        {
            var apiRequest = new GetApprenticeshipsRequest(request.AccountId, request.Status, request.PageNumber,
                request.PageItemCount);
            var response = await _commitmentsV2Api.Get<GetApprenticeshipsResponse>(apiRequest);

            return new GetApprenticeshipsQueryResult
            {
                TotalApprenticeshipsFound = response.TotalApprenticeshipsFound,
                Apprenticeships = response.Apprenticeships.Select(a => new GetApprenticeshipsQueryResult.Apprenticeship
                {
                    Id = a.Id,
                    TransferSenderId = a.TransferSenderId,
                    Uln = a.Uln,
                    ProviderId = a.ProviderId,
                    ProviderName = a.ProviderName,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    CourseCode = a.CourseCode,
                    CourseName = a.CourseName,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate,
                    Cost = a.Cost,
                    PledgeApplicationId = a.PledgeApplicationId
                })
            };
        }
    }
}