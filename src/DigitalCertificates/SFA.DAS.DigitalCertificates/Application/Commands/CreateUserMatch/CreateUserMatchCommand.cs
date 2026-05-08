using System;
using MediatR;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateUserMatch
{
    public class CreateUserMatchCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public long? Uln { get; set; }
        public string FamilyName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CertificateType { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string CourseLevel { get; set; }
        public int? DateAwarded { get; set; }
        public string ProviderName { get; set; }
        public int? Ukprn { get; set; }
        public bool IsMatched { get; set; }
        public bool IsFailed { get; set; }
    }
}
