﻿using MediatR;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.SendEmails
{
    public class SendEmailsCommandHandler : IRequestHandler<SendEmailsCommand, Unit>
    {
        private readonly INotificationService _notificationsService;

        public SendEmailsCommandHandler(INotificationService notificationsService)
        {
            _notificationsService = notificationsService;
        }

        public async Task<Unit> Handle(SendEmailsCommand request, CancellationToken cancellationToken)
        {
            var emailTasks = new List<Task>();
            foreach(var emailData in request.EmailDataList)
            {
                emailTasks.Add(_notificationsService.Send(new SendEmailCommand(emailData.TemplateName, emailData.RecipientEmailAddress, emailData.Tokens)));
            }

            await Task.WhenAll(emailTasks);

            return Unit.Value;
        }
    }
}
