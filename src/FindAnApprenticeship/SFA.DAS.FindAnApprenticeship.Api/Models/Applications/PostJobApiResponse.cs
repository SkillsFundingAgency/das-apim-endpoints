using System;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateJob;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class PostJobApiResponse
{
    public Guid Id { get; set; }

    public static implicit operator PostJobApiResponse(CreateJobCommandResult source)
    {
        return new PostJobApiResponse
        {
            Id = source.Id
        };
    }
}
