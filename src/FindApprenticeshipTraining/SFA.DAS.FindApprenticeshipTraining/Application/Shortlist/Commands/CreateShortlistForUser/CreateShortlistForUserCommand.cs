using System;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.CreateShortlistForUser;

public class CreateShortlistForUserCommand : IRequest<PostShortListResponse>
{
    public int Ukprn { get; set; }
    public string LocationName { get; set; }
    public int LarsCode { get; set; }
    public Guid ShortlistUserId { get; set; }
}