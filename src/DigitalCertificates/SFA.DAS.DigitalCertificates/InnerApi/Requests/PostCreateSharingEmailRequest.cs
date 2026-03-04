using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.DigitalCertificates.InnerApi.Requests
{
    public class PostCreateSharingEmailRequest : IPostApiRequest<PostCreateSharingEmailRequestData>
    {
        public PostCreateSharingEmailRequestData Data { get; set; }

        public string PostUrl { get; set; }

        public PostCreateSharingEmailRequest(PostCreateSharingEmailRequestData data, string sharingId)
        {
            Data = data;
            PostUrl = $"api/sharing/{sharingId}/email";
        }
    }

    public class PostCreateSharingEmailRequestData
    {
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public string LinkDomain { get; set; }
        public string MessageText { get; set; }
        public string TemplateId { get; set; }

        public static implicit operator PostCreateSharingEmailRequestData(SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmail.CreateSharingEmailCommand command)
        {
            return new PostCreateSharingEmailRequestData
            {
                EmailAddress = command.EmailAddress,
                UserName = command.UserName,
                LinkDomain = command.LinkDomain,
                MessageText = command.MessageText,
                TemplateId = command.TemplateId
            };
        }
    }
}
