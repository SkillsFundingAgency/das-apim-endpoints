using MediatR;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using System;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.DeleteProviderCourseLocation
{
    public class DeleteProviderCourseLocationCommand : IRequest<Unit>
    {
        public int Ukprn { get; set; }
        public int LarsCode { get; set; }
        public Guid Id { get; set; }
        public string UserId { get; set; }

        public static implicit operator DeleteProviderCourseLocationRequest(DeleteProviderCourseLocationCommand command) 
            => new DeleteProviderCourseLocationRequest
            {
                Ukprn = command.Ukprn,
                LarsCode = command.LarsCode,
                Id = command.Id,
                UserId = command.UserId,
            };
    }
}
