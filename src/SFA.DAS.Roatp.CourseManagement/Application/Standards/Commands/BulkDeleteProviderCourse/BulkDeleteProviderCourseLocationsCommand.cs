using MediatR;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Models.DeleteProviderCourseLocations;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Requests;

namespace SFA.DAS.Roatp.CourseManagement.Application.Standards.Commands.BulkDeleteProviderCourse
{
    public class BulkDeleteProviderCourseLocationsCommand : IRequest<Unit>
    {
        public int Ukprn { get; set; }
        public int LarsCode { get; set; }

        public string UserId { get; set; }
        public DeleteProviderCourseLocationOption DeleteProviderCourseLocationOption { get; set; }

        public static implicit operator BulkDeleteProviderCourseLocationsRequest(BulkDeleteProviderCourseLocationsCommand command) 
            => new BulkDeleteProviderCourseLocationsRequest
            {
                DeleteProviderCourseLocationOption = command.DeleteProviderCourseLocationOption,
                UserId = command.UserId,
                LarsCode = command.LarsCode,
                Ukprn = command.Ukprn
            };
    }
}
