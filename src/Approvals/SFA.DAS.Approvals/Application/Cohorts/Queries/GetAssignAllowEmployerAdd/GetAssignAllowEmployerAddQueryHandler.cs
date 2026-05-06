using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Requests;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Responses;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Constants;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Reservations;

namespace SFA.DAS.Approvals.Application.Cohorts.Queries.GetAssignAllowEmployerAdd;

public class GetAssignAllowEmployerAddQueryHandler(
    IReservationApiClient<ReservationApiConfiguration> reservationApiClient,
    ICourseTypesApiClient courseTypesApiClient) : IRequestHandler<GetAssignAllowEmployerAddQuery, GetAssignAllowEmployerAddQueryResult?>
{
    public async Task<GetAssignAllowEmployerAddQueryResult?> Handle(GetAssignAllowEmployerAddQuery request, CancellationToken cancellationToken)
    {
        var reservationResponse = await reservationApiClient.Get<GetReservationResponse>(new GetReservationRequest(request.ReservationId));
        
        if (reservationResponse?.Course == null)
        {
            return null;
        }

        var learningType = reservationResponse.Course.LearningType;
        
        if (!learningType.HasValue)
        {
            return new GetAssignAllowEmployerAddQueryResult { LearningType = null, AllowEmployerAdd = true };
        }

        var learningTypeString = LearningTypeToString(learningType);
        var allowEmployerAddResponse = await courseTypesApiClient.Get<GetAllowEmployerAddResponse>(new GetAllowEmployerAddRequest(learningTypeString));
        return new GetAssignAllowEmployerAddQueryResult
        {
            LearningType = (byte)learningType,
            AllowEmployerAdd = allowEmployerAddResponse.AllowEmployerAdd
        };
    }

    private static string? LearningTypeToString(LearningType? learningType)
    {
        return learningType switch
        {
            LearningType.Apprenticeship => "Apprenticeship",
            LearningType.FoundationApprenticeship => "FoundationApprenticeship",
            LearningType.ApprenticeshipUnit => "ApprenticeshipUnit",
            _ => null
        };
    }
}
