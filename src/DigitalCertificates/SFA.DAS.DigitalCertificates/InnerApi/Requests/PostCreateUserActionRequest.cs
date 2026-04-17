using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAction;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.DigitalCertificates.InnerApi.Requests
{
    public class PostCreateUserActionRequest : IPostApiRequest<PostCreateUserActionRequestData>
    {
        public PostCreateUserActionRequestData Data { get; set; }

        public string PostUrl { get; set; }

        public PostCreateUserActionRequest(PostCreateUserActionRequestData data, Guid userId)
        {
            Data = data;
            PostUrl = $"api/users/{userId}/actions";
        }
    }

    public class PostCreateUserActionRequestData
    {
        public string ActionType { get; set; }
        public string FamilyName { get; set; }
        public string GivenNames { get; set; }
        public Guid? CertificateId { get; set; }
        public string CertificateType { get; set; }
        public string CourseName { get; set; }

        public static implicit operator PostCreateUserActionRequestData(CreateUserActionCommand command)
        {
            return new PostCreateUserActionRequestData
            {
                ActionType = command.ActionType,
                FamilyName = command.FamilyName,
                GivenNames = command.GivenNames,
                CertificateId = command.CertificateId,
                CertificateType = command.CertificateType,
                CourseName = command.CourseName
            };
        }
    }
}
