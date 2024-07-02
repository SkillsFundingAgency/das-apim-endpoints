using MediatR;
using System;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Details
{
    public class GetUserSavedArticlesQuery : IRequest<GetUserSavedArticlesQueryResult>
    {
        public Guid ApprenticeId { get; set; }
    }
}