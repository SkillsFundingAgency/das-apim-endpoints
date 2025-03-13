namespace SFA.DAS.Aodp.Application.Queries.Qualifications
{
    public class GetFeedbackForQualificationFundingByIdQueryResponse
    {
        public string Status { get; set; }
        public string? Comments { get; set; }

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

