using MediatR;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.DeleteProviderCourse
{
    public class DeleteProviderCourseCommand : IRequest<Unit>
    {
        public int Ukprn { get; set; }
        public int LarsCode { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public static implicit operator DeleteProviderCourseRequest(DeleteProviderCourseCommand command) 
            => new DeleteProviderCourseRequest
            {
                Ukprn = command.Ukprn,
                LarsCode = command.LarsCode,
                UserId = command.UserId,
                UserDisplayName = command.UserDisplayName
            };
    }
}
