using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingControllersSample.ClientModels;
using TestingControllersSample.Controllers;
using TestingControllersSample.Core.Interfaces;
using TestingControllersSample.Core.Model;
using Xunit;

namespace TestingControllersSample.Test
{
    public class SessionControllerShould
    {
        [Fact]
      
        public async Task IndexReturnsAViewResultWhenId()
        {
            int id = 1;
            var fixture = new Fixture();
            var session = fixture.Create<BrainstormSession>();
            var mockRepo = new Mock<IBrainstormSessionRepository>();
            mockRepo.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(session);

            // Arrange
            var controller = new SessionController(mockRepo.Object);

            // Act
            var result = await controller.Index(id);

            // Assert
            var redirectToActionResult =
                Assert.IsType<ViewResult>(result);
            // Assert.Equal("Home", redirectToActionResult.ControllerName);
            //Assert.Equal("Index", redirectToActionResult);
        }

        [Fact]
        public async Task IndexReturnsARedirectToIndexHomeWhenIdIsNull()
        {
            
            var mockRepo = new Mock<IBrainstormSessionRepository>();
           

            // Arrange
            var controller = new SessionController(mockRepo.Object);

            // Act
            var result = await controller.Index(id: null);

            // Assert
            var redirectToActionResult =
                Assert.IsType<RedirectToActionResult>(result);

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
