using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Application.Review
{
    public class GetQfauFeedbackForApplicationReviewConfirmationQueryHandler : IRequestHandler<GetQfauFeedbackForApplicationReviewConfirmationQuery, BaseMediatrResponse<GetQfauFeedbackForApplicationReviewConfirmationQueryResponse>>
    {
        private const string QfauUserType = "Qfau";
        private readonly IMediator _mediator;

        public GetQfauFeedbackForApplicationReviewConfirmationQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<BaseMediatrResponse<GetQfauFeedbackForApplicationReviewConfirmationQueryResponse>> Handle(GetQfauFeedbackForApplicationReviewConfirmationQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<GetQfauFeedbackForApplicationReviewConfirmationQueryResponse>();
            response.Success = false;
            try
            {
                var feedback = await _mediator.Send(new GetFeedbackForApplicationReviewByIdQuery(request.ApplicationReviewId, QfauUserType));
                if (!feedback.Success) throw new Exception(feedback.ErrorMessage);

                var relatedQual = await _mediator.Send(new GetRelatedQualificationForApplicationQuery(feedback.Value.ApplicationId));

                response.Value = GetQfauFeedbackForApplicationReviewConfirmationQueryResponse.Map(feedback.Value, relatedQual.Success ? relatedQual.Value : null);

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
    }
}

