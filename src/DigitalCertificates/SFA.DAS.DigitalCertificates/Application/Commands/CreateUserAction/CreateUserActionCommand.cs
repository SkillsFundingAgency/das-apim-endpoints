using System;
using MediatR;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAction
{
    public class CreateUserActionCommand : IRequest<CreateUserActionResult>
    {
        public Guid UserId { get; set; }
        public string ActionType { get; set; }
        public string FamilyName { get; set; }
        public string GivenNames { get; set; }
        public Guid? CertificateId { get; set; }
        public string CertificateType { get; set; }
        public string CourseName { get; set; }
    }
}
