using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserMatch;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.DigitalCertificates.InnerApi.Requests
{
    public class PostCreateUserMatchRequest : IPostApiRequest<PostCreateUserMatchRequestData>
    {
        public PostCreateUserMatchRequestData Data { get; set; }

        public string PostUrl { get; set; }

        public PostCreateUserMatchRequest(PostCreateUserMatchRequestData data, Guid userId)
        {
            Data = data;
            PostUrl = $"api/users/{userId}/match";
        }
    }

    public class PostCreateUserMatchRequestData
    {
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

        public static implicit operator PostCreateUserMatchRequestData(CreateUserMatchCommand command)
        {
            return new PostCreateUserMatchRequestData
            {
                Uln = command.Uln,
                FamilyName = command.FamilyName,
                DateOfBirth = command.DateOfBirth,
                CertificateType = command.CertificateType,
                CourseCode = command.CourseCode,
                CourseName = command.CourseName,
                CourseLevel = command.CourseLevel,
                DateAwarded = command.DateAwarded,
                ProviderName = command.ProviderName,
                Ukprn = command.Ukprn,
                IsMatched = command.IsMatched,
                IsFailed = command.IsFailed
            };
        }
    }
}
