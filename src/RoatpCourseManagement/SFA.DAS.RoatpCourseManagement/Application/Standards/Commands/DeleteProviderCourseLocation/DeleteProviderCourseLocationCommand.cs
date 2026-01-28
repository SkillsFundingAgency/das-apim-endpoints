using System;
using MediatR;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.DeleteProviderCourseLocation
{
    public class DeleteProviderCourseLocationCommand : IRequest<Unit>
    {
        public int Ukprn { get; set; }
        public string LarsCode { get; set; }
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public static implicit operator DeleteProviderCourseLocationRequest(DeleteProviderCourseLocationCommand command)
            => new DeleteProviderCourseLocationRequest
            {
                Ukprn = command.Ukprn,
                LarsCode = command.LarsCode,
                Id = command.Id,
                UserId = command.UserId,
                UserDisplayName = command.UserDisplayName
            };
    }
}
