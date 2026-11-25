using MediatR;
using SFA.DAS.RoatpOversight.Infrastructure;
using SFA.DAS.RoatpOversight.InnerApi.Models;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.RoatpOversight.Application.Commands.AssignCourseTypesToProvider;

public class AssignCourseTypesToProviderCommandHandler(IRoatpV2ApiClient _apiClient) : IRequestHandler<AssignCourseTypesToProviderCommand>
{
    public async Task Handle(AssignCourseTypesToProviderCommand command, CancellationToken cancellationToken)
    {
        AddCourseTypesModel addCourseTypesCommand = new()
        {
            CourseTypes = [CourseType.Apprenticeship],
            UserId = command.UserId,
            UserDisplayName = command.UserDisplayName
        };
        var addCourseTypesResponse = await _apiClient.AddCourseTypes(command.Ukprn, addCourseTypesCommand, cancellationToken);
        addCourseTypesResponse.EnsureSuccessStatusCode();
    }
}
