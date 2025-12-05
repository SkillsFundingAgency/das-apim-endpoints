using System;
using MediatR;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateCertificateSharing
{
    public class CreateCertificateSharingCommand : IRequest<CreateCertificateSharingResult>
    {
        public Guid UserId { get; set; }
        public Guid CertificateId { get; set; }
        public string CertificateType { get; set; }
        public string CourseName { get; set; }
    }
}