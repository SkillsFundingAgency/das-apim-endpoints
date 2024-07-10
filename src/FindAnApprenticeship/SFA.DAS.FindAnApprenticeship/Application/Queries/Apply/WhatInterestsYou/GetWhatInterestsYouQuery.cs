using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.WhatInterestsYou
{
    public class GetWhatInterestsYouQuery : IRequest<GetWhatInterestsYouQueryResult>
    {
        public Guid CandidateId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
