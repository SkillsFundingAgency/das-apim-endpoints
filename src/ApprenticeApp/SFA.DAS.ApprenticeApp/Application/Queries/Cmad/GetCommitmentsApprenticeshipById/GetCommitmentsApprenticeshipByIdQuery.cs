using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Cmad.GetCommitmentsApprenticeshipById
{
    public class GetCommitmentsApprenticeshipByIdQuery : IRequest<GetApprenticeshipResponse>
    {
        public long ApprenticeshipId { get; set; }
    }
}
