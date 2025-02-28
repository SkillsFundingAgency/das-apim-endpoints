using System;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.CreateShortlistForUser
{
    public class CreateShortlistForUserCommand : IRequest<PostShortListResponse>
    {
        public float? Lat { get; set; }
        public float? Lon { get; set; }
        public int Ukprn { get; set; }
        public string LocationDescription { get; set; }
        public int StandardId { get; set; }
        public Guid ShortlistUserId { get; set; }
    }
}