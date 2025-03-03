using MediatR;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Details
{
    public class GetCategoriesByContentTypeQuery : IRequest<GetCategoriesByContentTypeQueryResult>
    {
        public string ContentType { get; set; }
    }
}