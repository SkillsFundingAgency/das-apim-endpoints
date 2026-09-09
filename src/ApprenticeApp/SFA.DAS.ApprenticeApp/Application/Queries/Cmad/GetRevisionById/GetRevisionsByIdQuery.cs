using MediatR;
using System;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Cmad.GetRevisionById
{
    public class GetRevisionsByIdQuery : IRequest<GetRevisionsByIdQueryResult>
    {
        public Guid ApprenticeId { get; set; }
        public long ApprenticeshipId { get; set; }
        public long RevisionId { get; set; }
    }
}
