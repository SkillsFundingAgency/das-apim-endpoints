using System;
using MediatR;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.RoatpCourseManagement.Application.Locations.Commands.DeleteProviderLocation;
public class DeleteProviderLocationCommand : IRequest<ApiResponse<Unit>>
{
    public int Ukprn { get; set; }
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string UserDisplayName { get; set; }
    public static implicit operator DeleteProviderLocationRequest(DeleteProviderLocationCommand command)
        => new()
        {
            Ukprn = command.Ukprn,
            Id = command.Id,
            UserId = command.UserId,
            UserDisplayName = command.UserDisplayName
        };
}

