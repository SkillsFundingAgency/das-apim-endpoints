using SFA.DAS.SharedOuterApi.Models.DfeSignIn;

namespace SFA.DAS.Aodp.Application.Queries.Application.Review
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
        public string ApplicationStatus { get; set; }

        public List<Funding> FundedOffers { get; set; } = new();
        public List<Feedback> Feedbacks { get; set; } = new();

        public string? Reviewer1 { get; set; }
        public string? Reviewer2 { get; set; }

        public List<Reviewer> AvailableReviewers { get; set; } = new();

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

        public class Reviewer
        {
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }

            public static implicit operator Reviewer(User user) => new Reviewer
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
        }
    }

}

