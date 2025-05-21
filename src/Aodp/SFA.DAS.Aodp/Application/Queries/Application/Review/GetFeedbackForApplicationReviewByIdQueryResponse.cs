namespace SFA.DAS.Aodp.Application.Queries.Application.Review
{
    public class GetFeedbackForApplicationReviewByIdQueryResponse
    {
        public string? Owner { get; set; }
        public string Status { get; set; }
        public bool NewMessage { get; set; }
        public string UserType { get; set; }
        public string? Comments { get; set; }
        public Guid ApplicationId { get; set; }


        public List<Funding> FundedOffers { get; set; } = new();

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

