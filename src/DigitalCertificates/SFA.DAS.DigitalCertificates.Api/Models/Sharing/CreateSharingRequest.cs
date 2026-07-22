using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharing;
using System;

namespace SFA.DAS.DigitalCertificates.Api.Models.Sharing
{
    public class CreateSharingRequest
    {
        public Guid UserId { get; set; }
        public Guid CertificateId { get; set; }
        public string CertificateType { get; set; }
        public string CourseName { get; set; }

        public static implicit operator CreateSharingCommand(CreateSharingRequest source)
        {
            return new CreateSharingCommand
            {
                UserId = source.UserId,
                CertificateId = source.CertificateId,
                CertificateType = source.CertificateType,
                CourseName = source.CourseName
            };
        }
    }
}
