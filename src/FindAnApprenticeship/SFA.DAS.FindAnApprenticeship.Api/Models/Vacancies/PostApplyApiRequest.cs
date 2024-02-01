using System;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Vacancies;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Vacancies
{
    public class PostApplyApiRequest
    {
        public Guid CandidateId { get; set; }
    }

    public class PostApplyApiResponse
    {
        public static implicit operator PostApplyApiResponse(ApplyCommandResponse source)
        {
            return new PostApplyApiResponse
            {
                ApplicationId = source.ApplicationId
            };
        }

        public Guid ApplicationId { get; set; }
    }
}
