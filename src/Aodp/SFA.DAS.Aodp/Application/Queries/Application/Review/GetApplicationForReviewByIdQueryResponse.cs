﻿namespace SFA.DAS.Aodp.Application.Queries.Application.Review
{
    public class GetApplicationForReviewByIdQueryResponse
    {
        public Guid Id { get; set; }
        public Guid ApplicationReviewId { get; set; }

        public string Name { get; set; }
        public DateTime LastUpdated { get; set; }
        public int Reference { get; set; }
        public string? Qan { get; set; }
        public string? AwardingOrganisation { get; set; }

        public bool SharedWithSkillsEngland { get; set; }
        public bool SharedWithOfqual { get; set; }
        public string FormTitle { get; set; }

        public List<Funding> FundedOffers { get; set; } = new();
        public List<Feedback> Feedbacks { get; set; } = new();

        public class Feedback
        {
            public string? Owner { get; set; }
            public string Status { get; set; }
            public bool NewMessage { get; set; }
            public string UserType { get; set; }
            public string? Comments { get; set; }
            public bool LatestCommunicatedToAwardingOrganisation { get; set; }
        }

        public class Funding
        {
            public Guid Id { get; set; }
            public Guid FundingOfferId { get; set; }
            public string FundedOfferName { get; set; }
            public DateOnly? StartDate { get; set; }
            public DateOnly? EndDate { get; set; }
            public string? Comments { get; set; }
        }
    }

}

