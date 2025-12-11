using System;
using MediatR;
using static SFA.DAS.DigitalCertificates.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateSharing
{
    public class CreateSharingCommand : IRequest<CreateSharingResult>
    {
        public Guid UserId { get; set; }
        public Guid CertificateId { get; set; }
        public CertificateType CertificateType { get; set; }
        public string CourseName { get; set; }
    }
}