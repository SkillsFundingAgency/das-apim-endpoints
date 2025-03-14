namespace SFA.DAS.Aodp.Application.Queries.FundingOffer
{
    public class GetFundingOffersQueryResponse
    {
        public class FundingOffer
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public List<FundingOffer> Offers { get; set; } = new();
    }


}

