using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeSubscriptions;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class AddUpdateApprenticeArticleCommandHandlerTests
    {
        [Test, MoqAutoData]
        public async Task AddAppSubCommandHandlerTest(
            AddUpdateApprenticeArticleCommandHandler sut,
            AddUpdateApprenticeArticleCommand command,
            CancellationToken cancellationToken)
        {


            ApprenticeArticle apprenticeArticleData = new()
            {
                LikeStatus = true,
                IsSaved = false
            };

            command.EntryId = "123";

            await sut.Handle(command, cancellationToken);

            sut.Should().NotBeNull();
        }
    }
}