using System;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAction;

namespace SFA.DAS.DigitalCertificates.Api.Models.Users
{
    public class CreateUserActionRequest
    {
        public string ActionType { get; set; }
        public string FamilyName { get; set; }
        public string GivenNames { get; set; }
        public Guid? CertificateId { get; set; }
        public string CertificateType { get; set; }
        public string CourseName { get; set; }

        public static implicit operator CreateUserActionCommand(CreateUserActionRequest source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new CreateUserActionCommand
            {
                ActionType = source.ActionType,
                FamilyName = source.FamilyName,
                GivenNames = source.GivenNames,
                CertificateId = source.CertificateId,
                CertificateType = source.CertificateType,
                CourseName = source.CourseName
            };
        }
    }
}
