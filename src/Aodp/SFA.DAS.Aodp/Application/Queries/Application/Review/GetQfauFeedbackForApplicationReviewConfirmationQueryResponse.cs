namespace SFA.DAS.Aodp.Application.Queries.Application.Review
{
    public class GetQfauFeedbackForApplicationReviewConfirmationQueryResponse
    {
        public Qualification? RelatedQualification { get; set; }
        public string? Owner { get; set; }
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

        public class Qualification
        {
            public string? Qan { get; set; }
            public string? Status { get; set; }
            public string? Name { get; internal set; }
        }

        public static GetQfauFeedbackForApplicationReviewConfirmationQueryResponse Map(GetFeedbackForApplicationReviewByIdQueryResponse feedback, GetRelatedQualificationForApplicationQueryResponse? qualification)
        {
            GetQfauFeedbackForApplicationReviewConfirmationQueryResponse model = new()
            {
                Status = feedback.Status,
                Comments = feedback.Comments,
                Owner = feedback.Owner,
            };

            foreach (var funding in feedback.FundedOffers ?? [])
            {
                model.FundedOffers.Add(new()
                {
                    Comments = funding.Comments,
                    EndDate = funding.EndDate,
                    StartDate = funding.StartDate,
                    FundingOfferId = funding.FundingOfferId,
                    FundedOfferName = funding.FundedOfferName,
                    Id = funding.Id
                });
            }

            if (qualification != null)
            {
                model.RelatedQualification = new()
                {
                    Qan = qualification.Qan,
                    Status = qualification.Status,
                    Name = qualification.Name,
                };
            }

            return model;

        }
    }
}

