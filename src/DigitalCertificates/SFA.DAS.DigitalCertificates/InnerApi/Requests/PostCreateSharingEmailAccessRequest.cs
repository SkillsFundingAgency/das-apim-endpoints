using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmailAccess;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.DigitalCertificates.InnerApi.Requests
{
    public class PostCreateSharingEmailAccessRequest : IPostApiRequest<PostCreateSharingEmailAccessRequestData>
    {
        public PostCreateSharingEmailAccessRequestData Data { get; set; }

        public PostCreateSharingEmailAccessRequest(PostCreateSharingEmailAccessRequestData data)
        {
            Data = data;
        }

        public string PostUrl => "api/sharingemailaccess";
    }

    public class PostCreateSharingEmailAccessRequestData
    {
        public Guid SharingEmailId { get; set; }

        public static implicit operator PostCreateSharingEmailAccessRequestData(CreateSharingEmailAccessCommand command)
        {
            return new PostCreateSharingEmailAccessRequestData
            {
                SharingEmailId = command.SharingEmailId
            };
        }
    }
}
