using System;
using MediatR;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.CreateShortlistForUser
{
    public class CreateShortlistForUserCommand : IRequest<Unit>
    {    
        public float? Lat { get ; set ; }
        public float? Lon { get ; set ; }
        public int Ukprn { get ; set ; }
        public string LocationDescription { get ; set ; }
        public int StandardId { get ; set ; }
        public Guid ShortlistUserId { get ; set ; }
    }
}