using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.WhatInterestsYou;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications
{
    public class GetWhatInterestsYouApiResponse
    {
        public string EmployerName { get; set; }
        public string StandardName { get; set; }

        public static implicit operator GetWhatInterestsYouApiResponse(GetWhatInterestsYouQueryResult source)
        {
            return new GetWhatInterestsYouApiResponse
            {
                EmployerName = source.EmployerName,
                StandardName = source.StandardName
            };
        }
    }
}
