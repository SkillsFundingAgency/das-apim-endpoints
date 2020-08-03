using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.Application.Command.CreateApplication;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Models.Commitments;
using SFA.DAS.EmployerIncentives.Models.EmployerIncentives;

namespace SFA.DAS.EmployerIncentives.Application.Commands.CreateApplication
{
    public class CreateApplicationHandler : IRequestHandler<CreateApplicationCommand, Guid>
    {
        private readonly ICommitmentsService _commitmentsService;
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public CreateApplicationHandler(ICommitmentsService commitmentsService, IEmployerIncentivesService employerIncentivesService)
        {
            _commitmentsService = commitmentsService;
            _employerIncentivesService = employerIncentivesService;
        }

        public async Task<Guid> Handle(CreateApplicationCommand command, CancellationToken cancellationToken)
        {
            var apprenticeships = await _commitmentsService.GetApprenticeshipDetails(command.AccountId, command.ApprenticeshipIds);

            var request = CreateIncentiveApplicationRequest(command, apprenticeships);

            await _employerIncentivesService.CreateIncentiveApplication(request);

            return command.ApplicationId;
        }

        private CreateIncentiveApplication CreateIncentiveApplicationRequest(CreateApplicationCommand command, IEnumerable<ApprenticeshipResponse> apprenticeships)
        {
            return new CreateIncentiveApplication
            {
                IncentiveApplicationId = command.ApplicationId,
                AccountId = command.AccountId,
                AccountLegalEntityId = command.AccountLegalEntityId,
                Apprenticeships = apprenticeships.Select(MapApprenticeship).ToArray()
            };
        }

        private IncentiveClaimApprenticeshipDto MapApprenticeship(ApprenticeshipResponse from)
        {
            return new IncentiveClaimApprenticeshipDto
            {
                ApprenticeshipId = from.Id,
                FirstName = from.FirstName,
                LastName = from.LastName,
                DateOfBirth = from.DateOfBirth,
                Uln = from.Uln,
                PlannedStartDate = from.OriginalStartDate ?? from.StartDate,
                ApprenticeshipEmployerTypeOnApproval = MapLevyType(from.ApprenticeshipEmployerTypeOnApproval)
            };
        }

        private Models.EmployerIncentives.ApprenticeshipEmployerType MapLevyType(Models.Commitments.ApprenticeshipEmployerType? from)
        {

            switch (from)
            {
                case null:
                    return Models.EmployerIncentives.ApprenticeshipEmployerType.Unknown;
                case Models.Commitments.ApprenticeshipEmployerType.Levy:
                    return Models.EmployerIncentives.ApprenticeshipEmployerType.Levy;
                case Models.Commitments.ApprenticeshipEmployerType.NonLevy:
                    return Models.EmployerIncentives.ApprenticeshipEmployerType.NonLevy;
                default:
                    throw new InvalidCastException($"Unable to convert from Commitments.ApprenticeshipEmployerType {from}");
            }
        }
    }
}