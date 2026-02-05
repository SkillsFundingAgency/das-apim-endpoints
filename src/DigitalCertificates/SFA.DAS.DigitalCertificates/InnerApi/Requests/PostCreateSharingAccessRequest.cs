using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingAccess;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.DigitalCertificates.InnerApi.Requests
{
    public class PostCreateSharingAccessRequest : IPostApiRequest<PostCreateSharingAccessRequestData>
    {
        public PostCreateSharingAccessRequestData Data { get; set; }

        public PostCreateSharingAccessRequest(PostCreateSharingAccessRequestData data)
        {
            Data = data;
        }

        public string PostUrl => "api/sharingaccess";
    }

    public class PostCreateSharingAccessRequestData
    {
        public Guid SharingId { get; set; }

        public static implicit operator PostCreateSharingAccessRequestData(CreateSharingAccessCommand command)
        {
            return new PostCreateSharingAccessRequestData
            {
                SharingId = command.SharingId
            };
        }
    }
}
