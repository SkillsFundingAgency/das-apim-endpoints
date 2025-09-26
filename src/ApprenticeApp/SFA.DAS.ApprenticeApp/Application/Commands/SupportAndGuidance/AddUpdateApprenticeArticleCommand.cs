using System;
using MediatR;

namespace SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeSubscriptions
{
    public class AddUpdateApprenticeArticleCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string EntryId { get; set; }
        public string EntryTitle { get; set; } 
        public bool? IsSaved { get; set; }
        public bool? LikeStatus { get; set; }
    }
}