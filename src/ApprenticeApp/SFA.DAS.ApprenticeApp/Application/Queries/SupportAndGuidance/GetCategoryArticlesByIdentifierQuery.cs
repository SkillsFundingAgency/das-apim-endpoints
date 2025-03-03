using System;
using MediatR;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Details
{
    public class GetCategoryArticlesByIdentifierQuery : IRequest<GetCategoryArticlesByIdentifierQueryResult>
    {
        public string Slug { get; set; }
        public Guid? Id { get; set; }
    }
}