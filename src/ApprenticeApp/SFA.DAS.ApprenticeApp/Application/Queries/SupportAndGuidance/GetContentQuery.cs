using MediatR;
using System;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Details
{
    public class GetContentQuery : IRequest<GetContentQueryResult>
    {
        public string EntryId { get; set; }
    }
}