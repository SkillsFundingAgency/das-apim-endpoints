using System;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserMatch;

namespace SFA.DAS.DigitalCertificates.Api.Models.Users
{
    public class CreateUserMatchRequest
    {
        public long? Uln { get; set; }
        public Guid? UserIdentityId { get; set; }
        public string CertificateType { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string CourseLevel { get; set; }
        public int? YearAwarded { get; set; }
        public string ProviderName { get; set; }
        public int? Ukprn { get; set; }
        public bool IsMatched { get; set; }
        public bool IsFailed { get; set; }

        public static implicit operator CreateUserMatchCommand(CreateUserMatchRequest source)
        {
            return new CreateUserMatchCommand
            {
                Uln = source.Uln,
                UserIdentityId = source.UserIdentityId,
                CertificateType = source.CertificateType,
                CourseCode = source.CourseCode,
                CourseName = source.CourseName,
                CourseLevel = source.CourseLevel,
                YearAwarded = source.YearAwarded,
                ProviderName = source.ProviderName,
                Ukprn = source.Ukprn,
                IsMatched = source.IsMatched,
                IsFailed = source.IsFailed
            };
        }
    }
}
