using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Requests;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Responses;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Reservations;
using SFA.DAS.SharedOuterApi.Interfaces;

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
        var learningTypeString = LearningTypeToString(learningType);
        
        if (string.IsNullOrEmpty(learningTypeString))
        {
            return new GetAssignAllowEmployerAddQueryResult { LearningType = learningType, AllowEmployerAdd = true };
        }

        var allowEmployerAddResponse = await courseTypesApiClient.Get<GetAllowEmployerAddResponse>(new GetAllowEmployerAddRequest(learningTypeString));
        return new GetAssignAllowEmployerAddQueryResult
        {
            LearningType = learningType,
            AllowEmployerAdd = allowEmployerAddResponse.AllowEmployerAdd
        };
    }

    private static string? LearningTypeToString(byte? learningType)
    {
        return learningType switch
        {
            0 => "Apprenticeship",
            1 => "FoundationApprenticeship",
            2 => "ApprenticeshipUnit",
            _ => null
        };
    }
}
