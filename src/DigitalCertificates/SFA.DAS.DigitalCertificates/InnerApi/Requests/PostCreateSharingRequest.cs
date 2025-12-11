using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharing;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using static SFA.DAS.DigitalCertificates.Models.Enums;

namespace SFA.DAS.DigitalCertificates.InnerApi.Requests
{
    public class PostCreateSharingRequest : IPostApiRequest<PostCreateSharingRequestData>
    {
        public PostCreateSharingRequestData Data { get; set; }

        public PostCreateSharingRequest(PostCreateSharingRequestData data)
        {
            Data = data;
        }

        public string PostUrl => "api/sharing";
    }

    public class PostCreateSharingRequestData
    {
        public Guid UserId { get; set; }
        public Guid CertificateId { get; set; }
        public CertificateType CertificateType { get; set; }
        public string CourseName { get; set; }

        public static implicit operator PostCreateSharingRequestData(CreateSharingCommand command)
        {
            return new PostCreateSharingRequestData
            {
                UserId = command.UserId,
                CertificateId = command.CertificateId,
                CertificateType = command.CertificateType,
                CourseName = command.CourseName
            };
        }
    }
}