using MediatR;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.DeleteProviderCourseLocation
{
    public class DeleteProviderCourseLocationCommand : IRequest<Unit>
    {
        public int Ukprn { get; set; }
        public int LarsCode { get; set; }
        public int ProviderCourseLocationId { get; set; }
        public string UserId { get; set; }

        public static implicit operator DeleteProviderCourseLocationRequest(DeleteProviderCourseLocationCommand command) 
            => new DeleteProviderCourseLocationRequest
            {
                Ukprn = command.Ukprn,
                LarsCode = command.LarsCode,
                ProviderCourseLocationId = command.ProviderCourseLocationId,
                UserId = command.UserId,
            };
    }
}
