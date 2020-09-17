using AutoFixture;
using AutoFixture.Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingControllersSample.Api;
using TestingControllersSample.ClientModels;
using TestingControllersSample.Controllers;
using TestingControllersSample.Core.Interfaces;
using TestingControllersSample.Core.Model;
using TestingControllersSample.Infrastructure;
using TestingControllersSample.ViewModels;
using Xunit;

namespace TestingControllersSample.Test
{
    public class IdeasControllerShould
    {
        [Fact]
        public async Task ForSession_ReturnsHttpNotFound_ForInvalidSession()
        {
            // Arrange
            var testSessionId = 123;
            var fixture = new Fixture();
            var newIdea = fixture.Create<NewIdeaModel>();
            var validatorMock = new Mock<IValidatorStrategy<NewIdeaModel>>();
            validatorMock.Setup(x => x.IsValid(newIdea)).Returns(true);
            var mockRepo = new Mock<IBrainstormSessionRepository>();
            mockRepo.Setup(repo => repo.GetByIdAsync(testSessionId))
                .ReturnsAsync((BrainstormSession)null);
            var controller = new IdeasController(mockRepo.Object, validatorMock.Object);

            // Act
            var result = await controller.ForSession(testSessionId);

            // Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(testSessionId, notFoundObjectResult.Value);
        }


        [Theory]
        [InlineData(1)]
        public async Task ForSession_ReturnsIdeasForSession(int testSessionId)
        {
            // Arrange

            var fixture = new Fixture();
            var newIdea = fixture.Create<NewIdeaModel>();
            var validatorMock = new Mock<IValidatorStrategy<NewIdeaModel>>();
            validatorMock.Setup(x => x.IsValid(newIdea)).Returns(true);
            var mockRepo = new Mock<IBrainstormSessionRepository>();
            mockRepo.Setup(repo => repo.GetByIdAsync(testSessionId))
               .ReturnsAsync(GetTestSession());
            var controller = new IdeasController(mockRepo.Object, validatorMock.Object);

            // Act
            var result = await controller.ForSession(testSessionId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<IdeaDTO>>(okResult.Value);
            var idea = returnValue.FirstOrDefault();
            Assert.Equal("One", idea.Name);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_GivenInvalidModel()
        {
            // Arrange & Act
            var fixture = new Fixture();
            var newIdea = fixture.Create<NewIdeaModel>();
            var validatorMock = new Mock<IValidatorStrategy<NewIdeaModel>>();
            validatorMock.Setup(x => x.IsValid(newIdea)).Returns(false);
            var mockRepo = new Mock<IBrainstormSessionRepository>();
            var controller = new IdeasController(mockRepo.Object, validatorMock.Object);
            // controller.ModelState.AddModelError("error", "some error");
            var model = fixture.Build<NewIdeaModel>().Without(x => x.Name).Without(x => x.Description).Create();
            // Act
            var result = await controller.Create(model);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }


        private BrainstormSession GetTestSession()
        {
            var session = new BrainstormSession()
            {
                DateCreated = new DateTime(2016, 7, 2),
                Id = 1,
                Name = "Test One"
            };

            var idea = new Idea() { Name = "One" };
            session.AddIdea(idea);
            return session;
        }
    }
}
