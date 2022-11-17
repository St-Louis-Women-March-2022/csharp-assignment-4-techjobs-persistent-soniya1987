using System;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TechJobsPersistentAutograded.Controllers;
using TechJobsPersistentAutograded.Data;
using TechJobsPersistentAutograded.Models;
using TechJobsPersistentAutograded.ViewModels;
using Xunit;

namespace TechJobsPersistentAutograded.Tests
{
    public class AddJobViewsTests
    {
        public AddJobViewsTests() { }

// ------ Index View Testing 

        [Fact]
        public void IndexAction_Executes_ReturnsViewForIndex()
        {
            //arrange
            var mockRepo = new Mock<JobRepository>();
            var controller = new HomeController(mockRepo.Object);

            //act
            var result = controller.Index();

            //assert
            Assert.IsType<ViewResult>(result);
        }

// ------- Add View Testing

        [Fact]
        public void Add_ActionExecutes_ReturnsViewForAdd()
        {
            //arrange
            var mockRepo = new Mock<JobRepository>();
            var controller = new HomeController(mockRepo.Object);

            //act
            var result = controller.AddJob();


            //assert
            Assert.IsType<ViewResult>(result);
        }

// ------- ProcessAddJobForm View Testing

        [Fact]
        public void ProcessAddJobFormMethod_ReturnsViewForNewJob()
        {
            //arrange
            var mockRepo = new Mock<JobRepository>();
            var controller = new HomeController(mockRepo.Object);

            AddJobViewModel addJobViewModel = new AddJobViewModel { Name = "Test Driver" };
            string[] testSelectedSkills = { "1", "2" };

            //act
            var result = controller.ProcessAddJobForm(addJobViewModel, testSelectedSkills);
            var redirectToActionResult = Assert.IsType<RedirectResult>(result);

            //assert
            Assert.IsType<RedirectResult>(result);
            Assert.NotNull(result);
            Assert.Equal("Index", redirectToActionResult.Url);
        }

        [Fact]
        public void ProcessAddJobForm_InvalidModelState_ReturnsView()
        {
            //arrange
            var mockRepo = new Mock<JobRepository>();
            var controller = new HomeController(mockRepo.Object);
            controller.ModelState.AddModelError("SkillId", "SkillId is required");

            var newTestJobVM = new AddJobViewModel { Name = "Tester of Many Things"};
            string[] testSelectedSkills = { };

            //act
            var result = controller.ProcessAddJobForm(newTestJobVM, testSelectedSkills);
            var viewRequest = Assert.IsType<ViewResult>(result);
            var testJob = Assert.IsType<AddJobViewModel>(viewRequest.Model);

            //assert
            Assert.Equal(newTestJobVM.Name, testJob.Name);
            Assert.Null(newTestJobVM.SkillId);
        }
    }
}
