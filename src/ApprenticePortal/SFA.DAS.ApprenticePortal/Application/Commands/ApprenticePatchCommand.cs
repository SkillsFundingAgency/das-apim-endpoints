using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.ApprenticePortal.Models;
using System;

namespace SFA.DAS.ApprenticePortal.Application.Commands.ApprenticePatch
{
    public class ApprenticePatchCommand : IRequest<Unit>
    {
        public JsonPatchDocument<Apprentice> Patch { get; set; }
        public Guid ApprenticeId { get; set; }
    }
}