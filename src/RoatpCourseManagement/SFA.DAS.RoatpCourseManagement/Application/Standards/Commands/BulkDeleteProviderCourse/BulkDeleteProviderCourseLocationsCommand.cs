using MediatR;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models.DeleteProviderCourseLocations;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.BulkDeleteProviderCourse
{
    public class BulkDeleteProviderCourseLocationsCommand : IRequest<Unit>
    {
        public int Ukprn { get; set; }
        public int LarsCode { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }

        public DeleteProviderCourseLocationOption DeleteProviderCourseLocationOption { get; set; }

        public static implicit operator ProviderCourseLocationsBulkDeleteRequest(BulkDeleteProviderCourseLocationsCommand command) 
            => new ProviderCourseLocationsBulkDeleteRequest
            {
                DeleteProviderCourseLocationOption = command.DeleteProviderCourseLocationOption,
                Ukprn = command.Ukprn,
                LarsCode = command.LarsCode,
                UserId = command.UserId,
                UserDisplayName = command.UserDisplayName
            };
    }
}
