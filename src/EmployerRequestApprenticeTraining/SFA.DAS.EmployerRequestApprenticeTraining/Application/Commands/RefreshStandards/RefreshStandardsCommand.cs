using MediatR;
using SFA.DAS.SharedOuterApi.Models.RequestApprenticeTraining;
using System.Collections.Generic;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.RefreshStandards
{
    public class RefreshStandardsCommand : IRequest<Unit>
    {
        public List<Standard> Standards{ get; set; }
    }
}
