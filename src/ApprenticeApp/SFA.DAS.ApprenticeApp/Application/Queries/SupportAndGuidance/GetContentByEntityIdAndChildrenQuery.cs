using MediatR;
using System;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Details
{
    public class GetContentByEntityIdAndChildrenQuery : IRequest<GetContentByEntityIdAndChildrenQueryResult>
    {
        public string EntryId { get; set; }
    }
}