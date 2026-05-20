using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateSharing
{
    public class CreateSharingCommand : IRequest<CreateSharingResult>
    {
        public Guid UserId { get; set; }
        public Guid CertificateId { get; set; }
        public string CertificateType { get; set; }

        [RegularExpression(@"^[^<>]*$", ErrorMessage = "CourseName contains invalid characters.")]
        public string CourseName { get; set; }
    }
}