using System;
using SFA.DAS.Assessor.Application.Commands.CreateOrUpdateUser;

namespace SFA.DAS.Assessor.Api.Models.Users
{
    public class CreateOrUpdateUserResponse
    {
        public Guid UserId { get; set; }

        public static implicit operator CreateOrUpdateUserResponse(
            CreateOrUpdateUserResult source)
        {
            return new CreateOrUpdateUserResponse
            {
                UserId = source.UserId
            };
        }
    }
}