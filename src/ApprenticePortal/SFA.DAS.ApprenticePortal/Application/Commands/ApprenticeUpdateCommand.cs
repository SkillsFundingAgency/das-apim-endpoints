using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.ApprenticePortal.Models;
using System;

namespace SFA.DAS.ApprenticePortal.Application.Commands.ApprenticeUpdate
{
    public class ApprenticeUpdateCommand : IRequest<Unit>
    {
        public JsonPatchDocument<Apprentice> Patch { get; set; }
        public Guid ApprenticeId { get; set; }
    }
}