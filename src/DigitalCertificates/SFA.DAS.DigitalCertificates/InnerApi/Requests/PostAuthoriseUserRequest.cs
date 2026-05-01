using System;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAuthorise;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.DigitalCertificates.InnerApi.Requests
{
    public class PostAuthoriseUserRequest : IPostApiRequest<PostAuthoriseUserRequestData>
    {
        public PostAuthoriseUserRequestData Data { get; set; }

        public string PostUrl { get; set; }

        public PostAuthoriseUserRequest(PostAuthoriseUserRequestData data, Guid userId)
        {
            Data = data;
            PostUrl = $"api/users/{userId}/authorise";
        }
    }

    public class PostAuthoriseUserRequestData
    {
        public long Uln { get; set; }

        public static implicit operator PostAuthoriseUserRequestData(CreateUserAuthoriseCommand command)
        {
            return new PostAuthoriseUserRequestData
            {
                Uln = command.Uln
            };
        }
    }
}
